using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/EndMission")]
public class EndMissionEvent : MapEvent
{
	[SerializeField] private MissionObject _missionObject;
	[SerializeField] private bool _isFail;
	
	public override void CurrentEventActivate()
	{
		foreach (Mission mission in player.missionList)
		{
			if (mission.Name.Equals(_missionObject.Name))
			{
				if (_isFail)
					mission.FailMission();
				else
					mission.EndMission();
			}
		}
	}
}
