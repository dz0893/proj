using System.Collections.Generic;
using UnityEngine;

public class AIPlayer
{
	private AIBuildingUpgrader aiBuildingUpgrader = new AIBuildingUpgrader();
	
	public bool active { get; set; }
	
	public List<AIGoal> goals { get; private set; }
	
	public Player player { get; set; }
	
	public List<GroundCell> targetedDepositCells { get; set; }
	
	public List<UnitData> unitDataList => player.capital.unitDataList;

	public List<int> currentUnitsCountList { get; private set; }
	
	public bool movingAtFirstTurn { get; private set; }
	
	private List<UnitDataObject> globalUnitDataObjectList => BattleMap.instance.aiBehaviorSetter.unitDataList;
	private List<UnitDataObject> capitalUnitDataObjectList => player.capital.unitDataObjectList;
	private List<int> globalUnitCountList => BattleMap.instance.aiBehaviorSetter.currentUnitsCountList;

	private Scenario scenario => BattleMap.instance.GetComponent<Scenario>();
	
	public void Init()
	{
		InitBehavior();
		InitGoals();
	}
	
	public void InitBehavior()
	{
		SetUnitDataList();
		movingAtFirstTurn = BattleMap.instance.aiBehaviorSetter.movingAtFirstTurn;
	}

	private void SetUnitDataList()
	{
		if (player.capital == null)
			return;

		currentUnitsCountList = new List<int>();
		currentUnitsCountList.Add(0);

		BattleMap.instance.aiBehaviorSetter.SetUnitDataObjectList(player.race);
		
		for (int i = 0; i < capitalUnitDataObjectList.Count; i++)
		{
			for (int j = 0; j < globalUnitDataObjectList.Count; j++)
			{
				if (capitalUnitDataObjectList[i] == globalUnitDataObjectList[j])
				{
					currentUnitsCountList.Add(globalUnitCountList[j]);
					continue;
				}
			}
		}
	}
	
	private void InitGoals()
	{
		goals = new List<AIGoal>();
		
		if (scenario == null)
			return;
		
		List<AIGoalObject> goalObjectList = scenario.aiGoals;
		
		foreach (AIGoalObject goalObject in goalObjectList)
		{
			if (goalObject.playerIndex == BattleMap.instance.playerList.IndexOf(player))
				goals.Add(goalObject.GetGoal());
		}
	}
	
	public void MakeTurn()
	{
		targetedDepositCells = new List<GroundCell>();
		
		if (!active)
		{
			TurnStateUI.skipTurn.Invoke();
		}
		else
		{
			Debug.Log("Start turn gold " + player.gold + " income " + player.goldIncome);
			aiBuildingUpgrader.UpgradeCapitalBuilding(player);
			AIObjectController.makeTurn.Invoke(player);
		}
	}
}
