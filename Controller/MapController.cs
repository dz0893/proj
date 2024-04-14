using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
	[SerializeField] private ActionController _actionController;
	[SerializeField] private AIBehaviorSetter _aiBehaviorSetter;
	
	public BattleMap battleMap { get; private set; }
	
	public static Unit selectedUnit { get; private set; }
	public static GlobalAction selectedGlobalAction { get; private set; }
	
	public static bool controllerIsBlocked;
	public static bool objectIsSelected { get; private set; }
	
	public static GroundCell selectedObjectCell;
	
	public delegate void Select(GroundCell cell);
	public delegate void SelectGlobalAction(GlobalAction globalAction);
	public delegate void Action(GroundCell cell);
	public delegate void OnlineAction(int targetIndex, int unitPositionIndex, int unitActionIndex);
	public delegate void Global(GroundCell cell);
	public delegate void ActivateGlobalActionEvent(GroundCell target, Player player, GlobalAction action);
	public delegate void OnlineGlobalAction(int targetIndex, int actionIndex);
	public delegate void Clear();
	public delegate void SetInstance();

	public static Select select;
	public static SelectGlobalAction selectGlobalAction;
	public static Action action;
	public static OnlineAction onlineAction;
	public static Global global;
	public static ActivateGlobalActionEvent activateGlobalActionEvent;
	public static OnlineGlobalAction onlineGlobalAction;
	public static Clear clear;
	public static SetInstance setInstance;
	
	private AbstractAction lastAction;

	private void Start()
	{
		select = TryToSelectObject;
		selectGlobalAction = SetGlobalAction;
		action = StartAction;
		onlineAction = OnlineMakingAction;
		global = StartGlobalAction;
		activateGlobalActionEvent = GlobalActionEvent;
		onlineGlobalAction = NetworkGlobalAction;
		clear = ClearCash;
		setInstance = Init;
	}
	
	private void Init()
	{
		battleMap = BattleMap.instance;
		_actionController.battleMap = battleMap;
		
		if (BattleMap.instance.GetComponent<AIBehaviorSetter>() == null)
			BattleMap.instance.aiBehaviorSetter = _aiBehaviorSetter;
		else
			BattleMap.instance.aiBehaviorSetter = BattleMap.instance.GetComponent<AIBehaviorSetter>();
		
		ClearCash();
	}
	
	private void SetGlobalAction(GlobalAction globalAction)
	{
		if (!controllerIsBlocked)
		{
			ClearCash();
			
			selectedGlobalAction = globalAction;
			selectedGlobalAction.SetTargetList(TurnController.currentPlayer);
			
			foreach (GroundCell cell in selectedGlobalAction.areaList)
				cell.OnSelecter(ActionType.Offensive, false);
		}
	}
	
	private void TryToSelectObject(GroundCell cell)
	{
		if (cell.onCellObject != null)
		{
			SelectObject(cell);

			battleMap.lastSelectedObject = cell.onCellObject;
		}
		else if (cell.unmaterialOnCellObject != null)
			SelectUnmaterialObject(cell);
			
		else
			ClearCash();
	}
	
	private void SelectObject(GroundCell cell)
	{
		UnitListUI.clearCellCash.Invoke();
		
		if (!controllerIsBlocked && !TurnController.currentPlayerNotLocal)
		{
			ClearCash();
			
			selectedObjectCell = cell;
			battleMap.selectedObject = selectedObjectCell.onCellObject;
			battleMap.selectedObject.Select();
			objectIsSelected = true;
			
			if (cell.onCellObject != null && cell.onCellObject is Unit)
				selectedUnit = cell.onCellObject as Unit;
			else
				selectedUnit = null;
		}
		else
		{
			cell.onCellObject.OpenObjectUI();
		}

		BattleMap.instance.RenderObjectsState();
	}
	
	private void SelectUnmaterialObject(GroundCell cell)
	{
		if (CustomNetworkManager.IsOnlineGame || cell.unmaterialOnCellObject.player == TurnController.currentPlayer)
		{
			if (!controllerIsBlocked && !TurnController.currentPlayerNotLocal)
			{
				ClearCash();
			
				selectedObjectCell = cell;
				cell.unmaterialOnCellObject.Select();
				objectIsSelected = true;
			}
			else
			{
				if (cell.unmaterialOnCellObject.player.id == CustomNetworkManager.localPlayerId)
					cell.unmaterialOnCellObject.OpenObjectUI();
			}
		}

		BattleMap.instance.RenderObjectsState();
	}
	
	private void ClearCash()
	{
		if (!objectIsSelected)
			UnitListUI.clearCellCash.Invoke();
		
		battleMap.ClearMapCash();
		battleMap.selectedObject = null;
		objectIsSelected = false;
		selectedObjectCell = null;
		selectedUnit = null;
		selectedGlobalAction = null;
		
		BattleUIManager.offUI.Invoke();

		BattleMap.instance.RenderObjectsState();
	}
	
	private void StartAction(GroundCell groundCell)
	{
		if (battleMap.selectedObject != null && battleMap.selectedObject.canUseActions)
		{
			if (CustomNetworkManager.IsOnlineGame)
			{
				int targetIndex = battleMap.mapCellList.IndexOf(groundCell);
				int unitPositionIndex = battleMap.mapCellList.IndexOf(selectedUnit.position);
				int unitActionIndex = selectedUnit.actionList.IndexOf(selectedUnit.choosenAction);

				LobbyController.localPlayerController.commandLouncher.MakeAction(targetIndex, unitPositionIndex, unitActionIndex);
			}
			else
			{
				StartCoroutine(OfflineMakingAction(groundCell));
			}
		}
	}
	
	private IEnumerator OfflineMakingAction(GroundCell groundCell)
	{
		if (battleMap.selectedObject is Unit)
		{
			Unit unit = battleMap.selectedObject as Unit;
			
			if (unit.totalActionTargetList.Contains(groundCell))
			{
				controllerIsBlocked = true;
				
				lastAction = unit.choosenAction;

				StartCoroutine(_actionController.MakeAction(unit, groundCell));
				ClearCash();
				
				while (ActionController.controllerInAction)
					yield return null;
				
				controllerIsBlocked = false;
				
				if (!unit.turnEnded && !unit.isDead)
				{
					SelectObject(unit.position);

					if (lastAction.ChekActionForActive(unit) && lastAction != unit.moveAction)
					{
						unit.ChooseAction(lastAction);
						BattleUIManager.onUI.Invoke(unit);
					}
				}

				PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
				
				yield return null;
			}
		}
	}

	private void OnlineMakingAction(int targetIndex, int unitPositionIndex, int unitActionIndex)
	{
		Unit unit = battleMap.mapCellList[unitPositionIndex].onCellObject as Unit;
		
		if (CustomNetworkManager.localPlayerId == battleMap.mapCellList[unitPositionIndex].onCellObject.player.id)
		{
			StartCoroutine(OnlineLocalPlayerMakingAction(battleMap.mapCellList[targetIndex]));
		}
		else
		{
			StartCoroutine(OnlineNotLockalPlayerMakingAction(targetIndex, unitPositionIndex, unitActionIndex));
		}
	}
	private IEnumerator OnlineLocalPlayerMakingAction(GroundCell groundCell)
	{
		StartCoroutine(OfflineMakingAction(groundCell));

		while (ActionController.controllerInAction)
			yield return null;

		/*foreach (PlayerObjectController playerController in LobbyController.instance.manager.gamePlayers)
		{
			if (playerController.inAction)
			{
				Debug.Log("waiting other players");
				yield return null;
			}
		}
		Debug.Log("End waiting");
		yield return null;*/
	}

	private bool AllClientsEndAction()
	{
		foreach (PlayerObjectController playerController in LobbyController.instance.manager.gamePlayers)
		{
			if (playerController.inAction)
			{
				return true;
			}
		}

		return false;
	}
	
	private IEnumerator OnlineNotLockalPlayerMakingAction(int targetCellIndex, int unitPositionIndex, int unitActionIndex)
	{
		battleMap.ClearMapCash();
		Unit unit = battleMap.mapCellList[unitPositionIndex].onCellObject as Unit;
		GroundCell targetCell = battleMap.mapCellList[targetCellIndex];
		unit.choosenAction = unit.actionList[unitActionIndex];
		unit.SetRangeOfAction();
		controllerIsBlocked = true;

		StartCoroutine(_actionController.MakeAction(unit, targetCell));
		while (ActionController.controllerInAction)
			yield return null;

		controllerIsBlocked = false;
		yield return null;
	}

	private void StartGlobalAction(GroundCell groundCell)
	{
		if (selectedGlobalAction != null)
		{
			if (CustomNetworkManager.IsOnlineGame)
			{
				int targetIndex = battleMap.mapCellList.IndexOf(groundCell);
				int actionIndex = TurnController.currentPlayer.globalActionList.IndexOf(selectedGlobalAction);
				LobbyController.localPlayerController.commandLouncher.GlobalAction(targetIndex, actionIndex);
			}
			else
			{
				StartCoroutine(OfflineGlobalAction(groundCell));
			}
		}
	}
	
	private void GlobalActionEvent(GroundCell target, Player player, GlobalAction action)
	{
		StartCoroutine(GlobalActionEventNumerator(target, player, action));
	}

	private IEnumerator GlobalActionEventNumerator(GroundCell target, Player player, GlobalAction action)
	{
		controllerIsBlocked = true;
		action.Activate(player, target);
		ClearCash();
			
		yield return new WaitForSeconds(ActionSettings.GLOBALACTIONTIME);
			
		controllerIsBlocked = false;
			
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
			
		yield return null;
	}

	private IEnumerator OfflineGlobalAction (GroundCell target)
	{
		if (selectedGlobalAction.areaList.Contains(target))
		{
			CameraController.setCameraPosition.Invoke(target.transform.position);
			
			controllerIsBlocked = true;
			selectedGlobalAction.Activate(TurnController.currentPlayer, target);
			ClearCash();
			
			yield return new WaitForSeconds(ActionSettings.GLOBALACTIONTIME);
			
			controllerIsBlocked = false;
			
			PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
			
			yield return null;
		}
	}

	private void NetworkGlobalAction (int targetIndex, int actionIndex)
	{
		if (CustomNetworkManager.localPlayerId == TurnController.currentPlayer.id)
		{
			OnlineLocalPlayerGlobalAction(targetIndex);
		}
		else
		{
			OnlineNotLockalPlayerGlobalAction(targetIndex, actionIndex);
		}
	}

	private void OnlineLocalPlayerGlobalAction(int targetIndex)
	{
		StartCoroutine(OnlineGlobalActionIEnumerator(targetIndex));
	}

	private void OnlineNotLockalPlayerGlobalAction(int targetIndex, int actionIndex)
	{
		selectedGlobalAction = TurnController.currentPlayer.globalActionList[actionIndex];
		selectedGlobalAction.SetTargetList(TurnController.currentPlayer);

		StartCoroutine(OnlineGlobalActionIEnumerator(targetIndex));
	}

	private IEnumerator OnlineGlobalActionIEnumerator (int targetIndex)
	{
		GroundCell targetCell = battleMap.mapCellList[targetIndex];

		if (selectedGlobalAction.areaList.Contains(targetCell))
		{
			CameraController.setCameraPosition.Invoke(targetCell.transform.position);
			
			controllerIsBlocked = true;
			selectedGlobalAction.Activate(TurnController.currentPlayer, targetCell);
			ClearCash();
			
			yield return new WaitForSeconds(ActionSettings.GLOBALACTIONTIME);
			
			controllerIsBlocked = false;
			
			PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
			
			yield return null;
		}
	}
}
