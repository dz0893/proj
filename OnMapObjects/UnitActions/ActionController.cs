using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
	[SerializeField] private AnimationCurve _movingCurve;
	[SerializeField] private Transform _mapContainer;
	private float scale => _mapContainer.localScale.x;
	
	public static ActionController instance;
	
	private ActionGetter actionGetter = new ActionGetter();
	
	private ObjectFinder objectFinder = new ObjectFinder();
	
	public BattleMap battleMap { get; set; }
	
	private bool unitIsStopped;
	
	public static bool controllerInAction { get; private set; }

	private void Start()
	{
		instance = this;
	}
	
	private static void SetActionState(bool state)
	{
		controllerInAction = state;

		if (CustomNetworkManager.IsOnlineGame)
			(CustomNetworkManager.singleton as CustomNetworkManager).localPlayer.SetActionState(state);
	}

	public IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		SetActionState(true);

		unitIsStopped = false;
		
		if (ChekCameraForFollowingUnit(unit))
			CameraController.setCameraPosition.Invoke(unit.transform.position);
		
		unit.SetDirection(target);
		
		if (unit.choosenAction.usedOnCasterOnly && unit.position != target)
		{
			GroundCell movingTarget;
			
			if (unit.moveAction !is Fly)
				movingTarget = target;
			else
			{
				unit.SetRoadTo(target);
			
				movingTarget = CheckRoadToTrap(unit);
			}
			
			unit.PlayVoiceClip(unit.GetActionAudioClip(unit.choosenAction));
			unit.PlayActionCastSound(unit.choosenAction);
			StartCoroutine(unit.moveAction.MakeAction(unit, movingTarget));

			while (unit.inAction)
				yield return null;

			if (!unitIsStopped)
			{
				StartCoroutine(unit.choosenAction.MakeAction(unit, target));
				
				while (unit.inAction)
					yield return null;
			}
		}
		
		else if (!unit.currentActionTargetList.Contains(target) && unit.moveActionTargetList.Contains(target))
		{
			GroundCell movingTarget;
			
			if (unit.moveAction !is Fly)
				movingTarget = target;
			else
			{
				unit.SetRoadTo(target);
			
				movingTarget = CheckRoadToTrap(unit);
			}

			unit.choosenAction = unit.moveAction;

			unit.PlayVoiceClip(unit.GetActionAudioClip(unit.choosenAction));
			unit.PlayActionCastSound(unit.choosenAction);
			StartCoroutine(unit.choosenAction.MakeAction(unit, movingTarget));
			
			while (unit.inAction)
			{
				yield return null;
			}
		}
		else if (NeedToGoClosser(unit, target))
		{
			unit.SetRoadTo(target);
			GroundCell movingTarget = CheckRoadToTrap(unit);

			unit.PlayVoiceClip(unit.GetActionAudioClip(unit.choosenAction));
			StartCoroutine(unit.moveAction.MakeAction(unit, movingTarget));
			
			while (unit.inAction)
				yield return null;
			
			yield return new WaitForSeconds(0.1f);
			
			if (!unitIsStopped)
			{
				unit.PlayActionCastSound(unit.choosenAction);
				StartCoroutine(unit.choosenAction.MakeAction(unit, target));
				
				while (unit.inAction)
					yield return null;
			}
		}
		else
		{
		//	unit.PlayVoiceClip(unit.GetActionAudioClip(unit.choosenAction));
			unit.PlayActionCastSound(unit.choosenAction);
			StartCoroutine(unit.choosenAction.MakeAction(unit, target));
			
			while (unit.inAction)
				yield return null;
		}

		SetActionState(false);
		
		if (TurnController.currentPlayer.aiPlayer == null)
			TurnController.currentPlayer.PlayAllEvents();
	}
	
	private bool NeedToGoClosser(Unit unit, GroundCell target)
	{
		ActionType actionType = unit.choosenAction.actionType;
		
		if ((unit.choosenAction.range == ActionRange.Melee) && !unit.position.closestCellList.Contains(target)
		&& target != unit.position)
			return true;
		
		else
			return false;
	}
	
	public void OneCellMoving(Unit unit, GroundCell goal, float moveTime)
	{
		unit.SetDirection(goal);
		StartCoroutine(MoveOneCell(unit, goal, moveTime));
	}
	
	private IEnumerator MoveOneCell(Unit unit, GroundCell goal, float moveTime)
	{
		float progress = 0;
		
		Vector3 startedPosition = unit.transform.position;
		Vector3 goalPosition = new Vector3 (goal.transform.position.x, goal.transform.position.y, GroundSettings.OBJECTZPOSITION);
		Vector3 deltaPos = goalPosition - startedPosition;
		Vector3 moveProgress = new Vector3(0,0,startedPosition.z + GroundSettings.OBJECTONMOVEZPOSITION);
		
		while (progress < 1)
		{
			progress += Time.deltaTime / moveTime;
			
			moveProgress.x = startedPosition.x + deltaPos.x * progress;
			moveProgress.y = startedPosition.y + deltaPos.y * progress + _movingCurve.Evaluate(progress) * moveTime * scale / ActionSettings.MOVETIME;
			
			if (ChekCameraForFollowingUnit(unit))
				CameraController.setCameraPosition.Invoke(startedPosition + deltaPos * progress);
			
			unit.transform.position = moveProgress;
			yield return null;
		}
		
		unit.SetNewPosition(goal);
	}

	public void MoveUnitInScenarioEvent(Unit unit, GroundCell movingTarget)
	{
		StartCoroutine(unit.moveAction.MakeAction(unit, movingTarget));
	}
	
	private GroundCell CheckRoadToTrap(Unit unit)
	{
		GroundCell goal = null;
		
		foreach (GroundCell cell in unit.road)
		{
			goal = cell;
			
			if (cell.unmaterialOnCellObject != null)
			{
				if (cell.unmaterialOnCellObject is Trap && cell.unmaterialOnCellObject.team != unit.team)
				{
					unitIsStopped = true;
					unit.SetRoadTo(cell);
					break;
				}
			}
		}
		
		return goal;
	}

	private bool ChekCameraForFollowingUnit(Unit unit)
	{
		if (!unit.player.isAIPlayer && GameOptions.cameraFollowingPlayer || unit.player.isAIPlayer && GameOptions.cameraFollowingAI)
			return true;
		else
			return false;
	}
}