[System.Serializable]
public struct BuildingSaveInfo
{
	public int currentLevel;
	public int levelOfBlocking;
	public bool isBlocked;
	
	public BuildingSaveInfo(BuildingData buildingData)
	{
		currentLevel = buildingData.currentLevel;
		levelOfBlocking = buildingData.levelOfBlocking;
		isBlocked = buildingData.isBlocked;
	}
}
