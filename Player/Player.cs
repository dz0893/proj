using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player
{
	public int id;

	public string nickname { get; set; }

	public bool isDefeated { get; private set; }

	public bool isAIPlayer { get; private set; }
	public AIPlayer aiPlayer { get; private set; }
	
	public int team { get; private set; }
	
	public Capital capital { get; set; }
	public Unit hero { get; set; }
	public Race race { get; set; }
	
	public List<GlobalAction> globalActionList { get; private set; } = new List<GlobalAction>();
	
	public List<NullObject> objectList { get; private set; }
	public List<Unit> unitsWithExperience { get; set; }
	private List<NullObject> delayedStartTurnObjectList;
	
	public int gold { get; private set; }
	
	public int goldIncome { get; private set; }
	public int basicGoldIncome { get; private set; }
	
	public int ore { get; private set; }
	
	public int oreIncome { get; private set; }
	public int basicOreIncome { get; private set; }
	
	public Color color { get; private set; }
	public Color secondColor { get; private set; }
	
	public int currentUnitLimit { get; set; }
	
	public int maxUnitLimit { get; private set; }
	
	public static UnityEvent<Player> PlayerStartedTurn = new UnityEvent<Player>();
	
	public List<Mission> missionList { get; set; }
	public List<JournalSheet> journalSheetList { get; set; }
	
	private List<MapEvent> startedTurnEventList = new List<MapEvent>();
	
	public int countOfHeroReviving { get; set; }

	public List<Upgrade> upgradeList { get; private set; } = new List<Upgrade>();

	public void SearchUpgrade(Upgrade upgrade)
	{
		if (!upgradeList.Contains(upgrade))
		{
			upgrade.MakeUpgradeForPlayer(this);
			upgradeList.Add(upgrade);
		}
	}

	public void AddObjectToList(NullObject obj)
	{
		objectList.Add(obj);
	}

	public void RemoveObjectFromList(NullObject obj)
	{
		objectList.Remove(obj);
	}

	public void InitGlobals(List<GlobalActionObject> actionObjectList)
	{
		globalActionList = new List<GlobalAction>();
		
		foreach (GlobalActionObject actionObject in actionObjectList)
			AddGlobal(actionObject);
	}

	public void AddGlobal(GlobalActionObject actionObject)
	{
		globalActionList.Add(actionObject.GetGlobalAction());
	}

	public void RemoveGlobal(GlobalActionObject actionObject)
	{
		foreach (GlobalAction action in globalActionList)
		{
			if (action.Name.Equals(actionObject.Name))
			{
				globalActionList.Remove(action);
				break;
			}
		}
	}
	
	public int reviveHeroGoldCost
	{
		get
		{
			int totalCost = MapSettings.basicHeroRevivingGoldCost 
			+ hero.experience.currentLevel * MapSettings.deltaRevivingGoldCostPerLevel
			+ countOfHeroReviving * MapSettings.deltaRevivingGoldCostPerDeath;
			
			if (totalCost > MapSettings.maxRevivingGoldCost)
				totalCost = MapSettings.maxRevivingGoldCost;
			
			if (isAIPlayer)
				totalCost /= 2;
			
			return totalCost;
		}
	}

	public int reviveHeroOreCost
	{
		get
		{
			int totalCost = MapSettings.basicHeroRevivingOreCost 
			+ hero.experience.currentLevel * MapSettings.deltaRevivingOreCostPerLevel
			+ countOfHeroReviving * MapSettings.deltaRevivingOreCostPerDeath;
			
			if (totalCost > MapSettings.maxRevivingOreCost)
				totalCost = MapSettings.maxRevivingOreCost;
			
			if (isAIPlayer)
				totalCost /= 2;
			
			return totalCost;
		}
	}
	
	public void SetTeam(int team)
	{
		this.team = team;
		
		foreach(NullObject obj in objectList)
			obj.team = team;
	}
	
	public void SetAI(bool isActive)
	{
		isAIPlayer = true;
		aiPlayer = new AIPlayer();
		aiPlayer.active = isActive;
		aiPlayer.player = this;
	}
	
	public void SetNotComputerPlayer()
	{
		isAIPlayer = false;
		aiPlayer = null;
	}
	
	public void WasteGold(int gold)
	{
		this.gold -= gold;
		if (this.gold < 0)
			Debug.Log("CURRENTGOLD " + this.gold + " wasting " + gold);
	}
	
	public void ChangeGoldIncome(int income)
	{
		goldIncome += income;
	}
	
	public void ChangeBasicGoldIncome(int income)
	{
		basicGoldIncome += income;
		goldIncome += income;
	}
	
	public void WasteOre(int ore)
	{
		this.ore -= ore;
	}
	
	public void ChangeOreIncome(int income)
	{
		oreIncome += income;
	}
	
	public void ChangeBasicOreIncome(int income)
	{
		basicOreIncome += income;
		oreIncome += income;
	}
	
	public void StartTurn()
	{
		DotAll(false);
		StartedTurnActions();
			
		if (globalActionList.Count != 0)
		{
			foreach (GlobalAction action in globalActionList)
				action.usedAtThisTurn = false;
		}
		
		gold += goldIncome;
		ore += oreIncome;

		PlayerStartedTurn.Invoke(this);
		
		PlayAllEvents();
		BattleMap.instance.RenderObjectsState();

		if (!isAIPlayer)
		{
			if (capital != null)
				CameraController.setCameraPosition.Invoke(capital.transform.position);
			else if (hero != null && !hero.isDead)
				CameraController.setCameraPosition.Invoke(hero.transform.position);
		}
		
		else
			aiPlayer.MakeTurn();
	}
	
	public void EndTurn()
	{
		DotAll(true);
		
		foreach (NullObject obj in objectList)
		{
			if (!obj.turnEnded)
				obj.ActivateTurnEndedAction();
			
			obj.EndTurn();
		}
		
		PlayAllEvents();
	}
	
	public void Init(int team)
	{
		upgradeList = new List<Upgrade>();
		objectList = new List<NullObject>();
		unitsWithExperience = new List<Unit>();
		missionList = new List<Mission>();
		journalSheetList = new List<JournalSheet>();
		
		currentUnitLimit = 0;
		SetMaxUnitLimit(MapSettings.maxUnitLimit);
		
		this.team = team;
		
		if (capital != null)
		{
			WasteGold(-capital.basicGold);
			WasteOre(-capital.basicOre);
		}
	}
	
	public void Init(PlayerSaveInfo playerSaveInfo)
	{
		isDefeated = playerSaveInfo.isDefeated;

		if (playerSaveInfo.isAIPlayer)
			SetAI(playerSaveInfo.aiIsActive);
			
		else
			SetNotComputerPlayer();
		
		upgradeList = new List<Upgrade>();
		objectList = new List<NullObject>();
		unitsWithExperience = new List<Unit>();
		missionList = new List<Mission>();
		journalSheetList = new List<JournalSheet>();
		
		currentUnitLimit = 0;
		team = playerSaveInfo.team;
		gold = playerSaveInfo.gold;
		ore = playerSaveInfo.ore;
		countOfHeroReviving = playerSaveInfo.countOfHeroReviving;
		maxUnitLimit = playerSaveInfo.maxUnitLimit;
		
		SetColorWithIndex(playerSaveInfo.colorIndex);

		race = (Race)playerSaveInfo.race;
	}
	
	public void SetColorWithIndex(int index)
	{
		color = MapSettings.colors[index];
		secondColor = new Vector4(color.r, color.g, color.b, 0.5f);
	}

	public void SetMaxUnitLimit(int maxUnitLimit)
	{
		if (maxUnitLimit > 0)
			this.maxUnitLimit = maxUnitLimit;
		else
			this.maxUnitLimit = MapSettings.maxUnitLimit;
	}
	
	private void StartedTurnActions()
	{
		delayedStartTurnObjectList = new List<NullObject>();

		for (int i = 0; i < objectList.Count; i++)
		{
			NullObject obj = objectList[i] as NullObject;

			if (obj.startTurnWithDelay)
				delayedStartTurnObjectList.Add(obj);
			else
				obj.StartTurn();

			if (obj.team != team || obj.isDead)
			{
				i--;
			}
		}

		for (int i = 0; i < delayedStartTurnObjectList.Count; i++)
			delayedStartTurnObjectList[i].StartTurn();
	}

	private void DotAll(bool endOfTurnDot)
	{
		for (int i = 0; i < objectList.Count; i++)
		{
			if (objectList[i] is Unit)
			{
				Unit unit = objectList[i] as Unit;
				unit.DotAllEffects(endOfTurnDot);

				if (unit.team != team || unit.isDead)
				{
					i--;
				}
			}
		}
	}
	
	public void PlayAllEvents()
	{
		while (startedTurnEventList.Count > 0)
			ActivateEvent(startedTurnEventList[0]);
	}
	
	public void AddEventToRow(MapEvent mapEvent)
	{
		if (!startedTurnEventList.Contains(mapEvent))
			startedTurnEventList.Add(mapEvent);
	}
	
	public void RemoveEventFromRow(MapEvent mapEvent)
	{
		if (startedTurnEventList.Contains(mapEvent))
			startedTurnEventList.Remove(mapEvent);
	}

	private void ActivateEvent(MapEvent mapEvent)
	{
		Scenario.activateMapEvent.Invoke(mapEvent, TurnController.turnCounter);
		startedTurnEventList.Remove(mapEvent);
	}

	public void AddExpToUnits(int exp)
	{
		int unitCount = 0;

		foreach (Unit unit in unitsWithExperience)
		{
			if (!unit.isDead)
				unitCount++;
		}

		if (unitCount > 0)
			exp /= unitCount;

		foreach (Unit unit in unitsWithExperience)
		{
			if (!unit.isDead)
				unit.experience.AddExp(exp);
		}
	}

	public void LoseGame()
	{
		isDefeated = true;

		while (objectList.Count > 0)
			objectList[0].Death();
	}

	public void SetPlayerName()
	{
		if (isAIPlayer)
		{
			nickname = UISettings.AiPlayer + BattleMap.instance.playerList.IndexOf(this);
		}
		else
		{
			if (!CustomNetworkManager.IsOnlineGame)
			{
				nickname = UISettings.Player + BattleMap.instance.playerList.IndexOf(this);
			}
		}
	}
}
