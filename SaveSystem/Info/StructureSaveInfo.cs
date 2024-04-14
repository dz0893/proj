[System.Serializable]
public struct StructureSaveInfo
{
	public int index;
	public int currentHealth;
	public int currentMana;
	public int playerIndex;
	
	public int cellIndex;
	public bool turnEnded;
	public bool flipped;

	public StructureSaveInfo(Structure structure)
	{
		index = structure.index;
		currentHealth = structure.currentHealth;
		currentMana = structure.currentMana;
		playerIndex = BattleMap.instance.playerList.IndexOf(structure.player);
		
		cellIndex = BattleMap.instance.mapCellList.IndexOf(structure.position);
		turnEnded = structure.turnEnded;
		flipped = structure.spriteFlipped;
	}
}
