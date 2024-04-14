using System.Collections.Generic;

[System.Serializable]
public struct CapitalSaveInfo
{
	public int index;
	public int currentHealth;
	public int currentMana;
	public int playerIndex;
	
	public int cellIndex;
	public bool turnEnded;
	public bool flipped;
	
	public List<BuildingSaveInfo> buildingSaveInfoList;
	public List<bool> upgradeList;
	
	public CapitalSaveInfo(Capital capital)
	{
		index = capital.index;
		
		currentHealth = capital.currentHealth;
		currentMana = capital.currentMana;
		playerIndex = BattleMap.instance.playerList.IndexOf(capital.player);
		
		cellIndex = BattleMap.instance.mapCellList.IndexOf(capital.position);
		
		turnEnded = capital.turnEnded;
		flipped = capital.spriteFlipped;

		buildingSaveInfoList = new List<BuildingSaveInfo>();
		upgradeList = new List<bool>();
		
		foreach (BuildingData buildingData in capital.buildingList)
			buildingSaveInfoList.Add(new BuildingSaveInfo(buildingData));

		foreach (Upgrade upgrade in capital.upgradeList)
			upgradeList.Add(upgrade.isResearched);
	}
}
