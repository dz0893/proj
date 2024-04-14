using UnityEngine;

[CreateAssetMenu(menuName = "Missions/DestroyObject")]
public class DestroyObjectMissionObject : MissionObject
{
	[SerializeField] private NullObject _destroyedObject;
	[SerializeField] private int _playerIndex;
	
	public NullObject destroyedObject => _destroyedObject;
	public int playerIndex => _playerIndex;
	
	public override Mission AddMissionToPlayer(Player player)
	{
		Mission mission = new DestroyObjectMission(this, player);
		player.missionList.Add(mission);
		
		return mission;
	}
}
