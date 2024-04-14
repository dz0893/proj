using UnityEngine;

[CreateAssetMenu(menuName = "Missions/DefeatEnemy")]
public class DefeatEnemyMissionObject : MissionObject
{
	[SerializeField] private bool _fullDestroing;
	[SerializeField] private bool _defeatTeam;
	[SerializeField] private int _playerIndex;
	
	public bool fullDestroing => _fullDestroing;
	public bool defeatTeam => _defeatTeam;
	public int playerIndex => _playerIndex;
	
	public override Mission AddMissionToPlayer(Player player)
	{
		Mission mission = new DefeatEnemyMission(this, player);
		player.missionList.Add(mission);
		
		return mission;
	}
}
