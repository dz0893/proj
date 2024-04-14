[System.Serializable]
public struct UnmaterialObjectSaveInfo
{
	public int index;
	public int playerIndex;
	public int cellIndex;
	
	public bool dontHaveIniter;
	public bool turnEnded;
	public bool flipped;
	
	public UnmaterialObjectSaveInfo(UnmaterialObject obj)
	{
		index = obj.index;
		
		playerIndex = BattleMap.instance.playerList.IndexOf(obj.player);
		cellIndex = BattleMap.instance.mapCellList.IndexOf(obj.position);
		
		if (obj.initer == null)
			dontHaveIniter = true;
		else
			dontHaveIniter = false;
		
		turnEnded = obj.turnEnded;
		flipped = obj.spriteFlipped;
	}
}
