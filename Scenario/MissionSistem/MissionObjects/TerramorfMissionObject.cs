using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Terramorfing")]
public class TerramorfMissionObject : MissionObject
{
	[SerializeField] private TerrainType _requiredTerrainType;
	[SerializeField] private int _cellIndex;
	
	public TerrainType requiredTerrainType => _requiredTerrainType;
	public int cellIndex => _cellIndex;
	
	public override Mission AddMissionToPlayer(Player player)
	{
		Mission mission = new TerramorfMission(this, player);
		player.missionList.Add(mission);
		
		return mission;
	}
}
