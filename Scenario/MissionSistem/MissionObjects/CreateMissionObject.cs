using UnityEngine;

[CreateAssetMenu(menuName = "Missions/CreateObjects")]
public class CreateMissionObject : MissionObject
{
	[SerializeField] private NullObject _objectPref;
	[SerializeField] private int _count;
	
	public NullObject objectPref => _objectPref;
	public int count => _count;
	
	public override Mission AddMissionToPlayer(Player player)
	{
		Mission mission = new CreateMission(this, player);
		player.missionList.Add(mission);
		
		return mission;
	}
}
