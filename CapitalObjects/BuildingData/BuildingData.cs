using UnityEngine;

public class BuildingData
{
	public Building building { get; private set; }
	public string Name => building.Name;
	
	public int upgradeGoldCost { get; private set; }
	public int upgradeOreCost { get; private set; }
	
	public int currentLevel { get; private set; }
	
	public int levelOfBlocking { get; private set; }
	
	public bool isBlocked { get; private set; }
	
	public string statusDescription { get; private set; }
	public bool canBeBuilded { get; private set; }

	public bool maxLeveled
	{
		get
		{
			if (!isBlocked)
				return currentLevel >= building.maxLevel;
			else
				return currentLevel >= levelOfBlocking;
		}
	}
	
	public BuildingData(Building building, int currentLevel)
	{
		this.building = building;
		this.currentLevel = currentLevel;
		upgradeGoldCost = building.upgradeGoldCostList[currentLevel];
		upgradeOreCost = building.upgradeOreCostList[currentLevel];
	}

	public void SetStatus(Player player)
	{
		canBeBuilded = false;

		if (maxLeveled)
		{
			statusDescription = UISettings.maxLeveled;
		}
		else if (player.gold < upgradeGoldCost || player.ore < upgradeOreCost)
		{
			statusDescription = UISettings.notEnoughtResources;
		}
		else if (player.capital.turnEnded)
		{
			statusDescription = UISettings.cantBuildToday;
		}
		else if (player != TurnController.currentPlayer)
		{
			statusDescription = UISettings.cantBuildUnitInEnemyTurn;
		}
		else
		{
			statusDescription = UISettings.canBeBuilded;
			canBeBuilded = true;
		}
	}
	
	public void IncreaseLevel()
	{
		if (!maxLeveled)
		{
			currentLevel++;
			upgradeGoldCost = building.upgradeGoldCostList[currentLevel];
			upgradeOreCost = building.upgradeOreCostList[currentLevel];
		}
	}
	
	public void SetLevelOfBlocking(int levelOfBlocking)
	{
		this.levelOfBlocking = levelOfBlocking;
		isBlocked = true;
	}
}
