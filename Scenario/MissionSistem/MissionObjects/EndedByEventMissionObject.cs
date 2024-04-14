using UnityEngine;

[CreateAssetMenu(menuName = "Missions/EndedByEvent")]
public class EndedByEventMissionObject : MissionObject
{
	public override Mission AddMissionToPlayer(Player player)
	{
		Mission mission = new EndedByEventMission(this, player);
		player.missionList.Add(mission);
		
		return mission;
	}
}
