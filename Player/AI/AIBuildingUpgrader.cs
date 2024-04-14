using System.Collections;
using System.Collections.Generic;

public class AIBuildingUpgrader
{
	public void UpgradeCapitalBuilding(Player player)
	{
		BuildingData buildingData = GetBuildingForUpgrade(player);
		Upgrade upgrade = GetUpgradeForResearch(player);

		if (buildingData != null)
			player.capital.Build(buildingData);
			
		else if (upgrade != null)
			player.SearchUpgrade(upgrade);
	}
	
	private BuildingData GetBuildingForUpgrade(Player player)
	{
		if (player.capital == null)
			return null;
		
		BuildingData buildingData = player.capital.buildingList[0];
		
		foreach (BuildingData building in player.capital.buildingList)
		{
			if (building.currentLevel < buildingData.currentLevel && !building.maxLeveled)
				buildingData = building;
		}
		
		if (buildingData.maxLeveled)
			buildingData = null;
		
		return buildingData;
	}

	private Upgrade GetUpgradeForResearch(Player player)
	{
		if (player.capital == null)
			return null;
		
		Upgrade researchedUpgrade = null;

		foreach (Upgrade upgrade in player.capital.upgradeList)
		{
			upgrade.SetStatus(player);

			if (upgrade.canBeResearched)
			{
				researchedUpgrade = upgrade;
				break;
			}
		}

		return researchedUpgrade;
	}
}
