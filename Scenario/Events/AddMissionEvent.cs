using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/AddMission")]
public class AddMissionEvent : MapEvent
{
	[SerializeField] private MissionObject _missionObject;
	
	public override void CurrentEventActivate()
	{
		Mission mission = _missionObject.AddMissionToPlayer(player);
		mission.TryToEndMission();
	}
	
	protected override void PlayOnLoad(EventRowObjectSaveInfo objectSaveInfo)
	{
		_missionObject.AddMissionToPlayer(player);
	}
}
