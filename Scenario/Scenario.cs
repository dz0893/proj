using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour
{
	public EventSystemLoader eventSystemLoader { get; private set; } = new EventSystemLoader();
	
	[SerializeField] private string _name;
	//[SerializeField] private string _description;
	[SerializeField] private string _goal;
	[SerializeField] private Sprite _prologueIcon;
	[SerializeField] private Sprite _epilogueIcon;

	[SerializeField] private string _prologue;
	[SerializeField] private string _epilogue;
	
	public string Name => _name;
	public string prologue => _prologue;
	public string epilogue => _epilogue;
	public string goal => _goal;
	public Sprite prologueIcon => _prologueIcon;
	public Sprite epilogueIcon => _epilogueIcon;
	
	[SerializeField] private List<EventActivatorObject> _activatorObjectList;
	
	[SerializeField] private List<AIGoalObject> _aiGoals;
	public List<AIGoalObject> aiGoals => _aiGoals;
	
	private List<EventActivator> activatorList;
	
	[SerializeField] private List<int> _buildingBlockingLevelList = new List<int>();
	[SerializeField] private List<bool> _playerBlockingStateList;
	
	[SerializeField] private List<int> _playerStartedGoldBonus = new List<int>();
	[SerializeField] private List<int> _playerStartedOreBonus = new List<int>();
	[SerializeField] private List<int> _playerGoldIncomeBonus = new List<int>();
	[SerializeField] private List<int> _playerOreIncomeBonus = new List<int>();
	
	[SerializeField] private List<int> _maxUnitLimitList = new List<int>();
	
	public delegate void RemoveActivator(int index);
	public delegate void AddActivatorToList(EventActivatorObject activatorObject);
	public delegate void ActivateMapEvent(MapEvent mapEvent, int turnOfActivation);
	public static RemoveActivator removeActivator;
	public static AddActivatorToList addActivatorToList;
	public static ActivateMapEvent activateMapEvent;
	
	public void Init(List<Player> playerList, bool isLoading)
	{
		InitActivatorList();
		removeActivator = RemoveActivatorWithIndex;
		addActivatorToList = AddActivator;
		activateMapEvent = PlayEvent;
		InitPlayerSettings(playerList, isLoading);
	}
	
	public void LoadActivatorList(List<EventRowObjectSaveInfo> rowObjectInfoList)
	{
		eventSystemLoader.StartActivatorsRow(rowObjectInfoList, activatorList);
	}
	
	private void InitActivatorList()
	{
		activatorList = new List<EventActivator>();
		
		foreach (EventActivatorObject activatorObject in _activatorObjectList)
		{
			AddActivator(activatorObject);
		}
	}
	
	private void AddActivator(EventActivatorObject activatorObject)
	{
		activatorList.Add(activatorObject.GetActivator());
	}
	
	private void PlayEvent(MapEvent mapEvent, int turnOfActivation)
	{
		StartCoroutine(mapEvent.ActivateEvent(turnOfActivation));
	}

	private void InitPlayerSettings(List<Player> playerList, bool isLoading)
	{
		if (!isLoading)
			SetPlayerResources(playerList);
			
		SetBuildingBlocks(playerList);
		SetUnitLimits(playerList);
	}
	
	private void SetPlayerResources(List<Player> playerList)
	{
		if (_playerStartedGoldBonus.Count != 0)
		{
			for (int i = 0; i < playerList.Count; i++)
				playerList[i].WasteGold(-_playerStartedGoldBonus[i]);
		}
		
		if (_playerStartedOreBonus.Count != 0)
		{
			for (int i = 0; i < playerList.Count; i++)
				playerList[i].WasteOre(-_playerStartedOreBonus[i]);
		}
		
		if (_playerGoldIncomeBonus.Count != 0)
		{
			for (int i = 0; i < playerList.Count; i++)
				playerList[i].ChangeBasicGoldIncome(_playerGoldIncomeBonus[i]);
		}
		
		if (_playerOreIncomeBonus.Count != 0)
		{
			for (int i = 0; i < playerList.Count; i++)
				playerList[i].ChangeBasicOreIncome(_playerOreIncomeBonus[i]);
		}
	}
	
	private void SetBuildingBlocks(List<Player> playerList)
	{
		if (_buildingBlockingLevelList.Count == 0)
			return;
		
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].capital != null && _playerBlockingStateList[i])
				playerList[i].capital.SetBuildingBlockedLevels(_buildingBlockingLevelList);
		}	
	}
	
	private void SetUnitLimits(List<Player> playerList)
	{
		if (_maxUnitLimitList.Count == 0)
			return;
		
		for (int i = 0; i < playerList.Count; i++)
			playerList[i].SetMaxUnitLimit(_maxUnitLimitList[i]);
	}
	
	private void RemoveActivatorWithIndex(int index)
	{
		for (int i = 0; i < activatorList.Count; i++)
		{
			if (activatorList[i].activatorIndex == index)
			{
				activatorList[i].RemoveListener();
				activatorList.Remove(activatorList[i]);
				i--;
			}
		}
	}
	
	private void OnDisable()
	{
		CleanEvents();
		CleanMissions();
	}
	
	private void CleanEvents()
	{
		foreach (EventActivator activator in activatorList)
			activator.RemoveListener();
		
		activatorList = new List<EventActivator>();
		
		eventSystemLoader.RemoveListener();
	}
	
	private void CleanMissions()
	{
		foreach (Player player in BattleMap.instance.playerList)
		{
			if (player != null)
			{
				foreach (Mission mission in player.missionList)
					mission.RemoveListener();
				
				player.missionList = new List<Mission>();
			}
		}
	}
}
