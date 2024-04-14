[System.Serializable]
public struct ActionEffectSaveInfo
{
	public int index;
	public int casterPlayerIndex;
	public int targetPlayerIndex;
	public int counter;
	
	public ActionEffectSaveInfo(CurrentEffect effect)
	{
		index = effect.index;
		casterPlayerIndex = BattleMap.instance.playerList.IndexOf(effect.casterPlayer);
		targetPlayerIndex = BattleMap.instance.playerList.IndexOf(effect.targetPlayer);
		counter = effect.counter;
	}
}
