using System.Collections.Generic;

public class EventSystemLoader
{
	public List<IEventRowObject> eventRowLog { get; private set; }
	
	public EventSystemLoader()
	{
		eventRowLog = new List<IEventRowObject>();
		
		EventActivator.eventWasPlayed.AddListener(AddEventRowObject);
		Mission.missionEnded.AddListener(AddEventRowObject);
	}
	
	public void RemoveListener()
	{
		EventActivator.eventWasPlayed.RemoveListener(AddEventRowObject);
		Mission.missionEnded.RemoveListener(AddEventRowObject);
	}
	
	private void AddEventRowObject(IEventRowObject rowObject)
	{
		if (!eventRowLog.Contains(rowObject))
		{
			eventRowLog.Add(rowObject);
		}
	}
	
	public void StartActivatorsRow(List<EventRowObjectSaveInfo> eventRowSaveInfoList, List<EventActivator> activatorList)
	{
		eventRowLog = new List<IEventRowObject>();
		
		for (int i = 0; i < eventRowSaveInfoList.Count; i++)
		{
			PlayEventRowObject(eventRowSaveInfoList[i], activatorList);
		}
	}
	
	private void PlayEventRowObject(EventRowObjectSaveInfo objectSaveInfo, List<EventActivator> activatorList)
	{
		if (objectSaveInfo.isMission)
			PlayMissionEnd(objectSaveInfo);
		else
			PlayActivator(objectSaveInfo, activatorList);
	}
	
	private void PlayMissionEnd(EventRowObjectSaveInfo objectSaveInfo)
	{
		foreach (Player player in BattleMap.instance.turnController.playerList)
		{
			foreach (Mission mission in player.missionList)
			{
				if (mission.index == objectSaveInfo.index)
				{
					AddEventRowObject(mission);
					mission.PlayOnLoad(objectSaveInfo);
					break;
				}
			}
		}
	}
	
	private void PlayActivator(EventRowObjectSaveInfo objectSaveInfo, List<EventActivator> activatorList)
	{
		for (int i = 0; i < activatorList.Count; i++)
		{
			if (activatorList[i].activatorIndex == objectSaveInfo.index)
			{
				AddEventRowObject(activatorList[i]);
				activatorList[i].ActivateOnLoad(objectSaveInfo);
				break;
			}
		}
	}
}
