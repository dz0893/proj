using System.Collections.Generic;
using UnityEngine;

public class GlobalTerramorf : GlobalAction
{
	public TerrainType terrainType { get; private set; }
	public bool areaEffect { get; private set; }
	
	public GlobalTerramorf(GlobalTerramorfObject actionObject)
	{
		this.actionObject = actionObject;
		
		terrainType = actionObject.terrainType;
		areaEffect = actionObject.areaEffect;
		
		InitDescriptionList();
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		return globalActionDistanceFinder.GetRemovedCellsForTerramorf(areaList, terrainType);
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		TerraformateCell(target);
		
		if (areaEffect)
		{
			foreach (GroundCell cell in target.closestCellList)
				TerraformateCell(cell);
		}
	}
	
	private void TerraformateCell(GroundCell cell)
	{
		if (cell.canBeTerraformated)
			cell.SetTerrainType(BattleMap.instance.groundFactory.GetTerrain(terrainType));
	}
}
