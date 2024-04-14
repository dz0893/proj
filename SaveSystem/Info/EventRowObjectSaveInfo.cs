[System.Serializable]
public struct EventRowObjectSaveInfo
{
	public int index;
	public int turnOfActivation;
	public bool isEnded;
	public bool isMission;
	public bool isFailed;
	
	public EventRowObjectSaveInfo(IEventRowObject rowObject)
	{
		index = rowObject.index;
		turnOfActivation = rowObject.turnOfActivation;
		isEnded = rowObject.isEnded;
		isMission = rowObject.isMission;
		isFailed = rowObject.isFailed;
	}
}
