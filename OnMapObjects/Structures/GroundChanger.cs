using System.Collections.Generic;
using UnityEngine;

public class GroundChanger : Structure
{
	[SerializeField] private TerrainType _terrainType;
	public TerrainType terrainType => _terrainType;
	
	protected override void CurrentStructureInit(GroundCell positionCell)
	{
		SetGround(positionCell);
	}
	
	protected void SetGround(GroundCell positionCell)
	{
		if (terrainType != positionCell.terrainType && positionCell.canBeTerraformated)
			positionCell.SetTerrainType(map.groundFactory.GetTerrain(terrainType));
			
		foreach (GroundCell cell in positionCell.closestCellList)
		{
			if (terrainType != cell.terrainType && cell.canBeTerraformated)
				cell.SetTerrainType(map.groundFactory.GetTerrain(terrainType));
		}
	}
	
	public override void StartTurn()
	{
		SetGround(position);
		
		turnEnded = false;
	}
}
