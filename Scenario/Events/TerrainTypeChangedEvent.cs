using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/TerrainChanget")]
public class TerrainTypeChangedEvent : MapEvent
{
	private GroundFactory groundFactory = new GroundFactory();

	[SerializeField] private int _cellsIndex;
	[SerializeField] private TerrainType _newTerrainType;
	[SerializeField] private bool _canChangeUnterramorfedCells;
	
	private IGround ground;

	private void Start()
	{
		ground = groundFactory.GetTerrain(_newTerrainType);
	}

	public override void CurrentEventActivate()
	{
		foreach (GroundCell cell in BattleMap.instance.GetAllCellsWithIndex(_cellsIndex))
		{
			if (_canChangeUnterramorfedCells || cell.canBeTerraformated)
				cell.SetTerrainType(groundFactory.GetTerrain(_newTerrainType));
		}
	}
}
