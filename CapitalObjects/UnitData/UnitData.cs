using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData
{
	public UnitDataObject dataObject { get; private set; }
	public Player player { get; private set; }
    public int index { get; private set; }
    public int goldCost { get; private set; }
	public int oreCost { get; private set; }
    public Unit unit { get; private set; }
    public List<Building> requiredBuildingList { get; private set; }
    public List<int> buildingLevelList { get; private set; }

	private bool costChanget;

	public string statusDescription { get; private set; }
	public bool canBeRecruited { get; private set; }

	public void SetStatus(Player player)
	{
		if (!CheckRequiredBuildings(player.capital))
		{
			canBeRecruited = false;
			statusDescription = UISettings.cantBuildUnit;
		}
		else if (TurnController.currentPlayer != player)
		{
			canBeRecruited = false;
			statusDescription = UISettings.cantBuildUnitInEnemyTurn;
		}
		else if (player.maxUnitLimit - player.currentUnitLimit < unit.leadershipCost)
		{
			canBeRecruited = false;
			statusDescription = UISettings.notEnoughtLimit;
		}
		else if (goldCost > player.gold || oreCost > player.ore)
		{
			canBeRecruited = false;
			statusDescription = UISettings.notEnoughtResources;
		}
		else
		{
			canBeRecruited = true;
			statusDescription = UISettings.unitCanBeBuilded;
		}
	}

	public void ChangeCost(int goldCost, int oreCost)
	{
		if (!costChanget)
		{
			this.goldCost += goldCost;
			this.oreCost += oreCost;

			if (this.goldCost < 0)
				this.goldCost = 0;
			
			if (this.oreCost < 0)
				this.oreCost = 0;
			
			costChanget = true;
		}
	}

    public UnitData(UnitDataObject dataObject, Player player)
    {
		this.dataObject = dataObject;
		this.player = player;
        index = dataObject.index;
        goldCost = dataObject.goldCost;
		oreCost = dataObject.oreCost;
        unit = dataObject.unit;
        requiredBuildingList = dataObject.requiredBuildingList;
        buildingLevelList = dataObject.buildingLevelList;
    }

    public bool CheckRequiredBuildings(Capital capital)
	{
		if (capital == null)
			return true;
		
		for (int i = 0; i < capital.buildingList.Count; i++)
		{
			if (!CheckCurrentBuilding(capital.buildingList[i]))
				return false;
		}
		
		return true;
	}
	
	public bool CheckCurrentBuilding(BuildingData buildingData)
	{
		bool buildingHaveRequiredLevel = false;
		
		if (requiredBuildingList.Contains(buildingData.building))
		{
			if (buildingLevelList[requiredBuildingList.IndexOf(buildingData.building)] <= buildingData.currentLevel)
				buildingHaveRequiredLevel = true;
		}
		else
			buildingHaveRequiredLevel = true;
		
		return buildingHaveRequiredLevel;
	}
	
	public void UnlockUnit(Capital capital)
	{
		if (CheckRequiredBuildings(capital))
			capital.unitList.Add(unit);
	}
}
