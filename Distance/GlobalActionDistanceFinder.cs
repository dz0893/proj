using System.Collections.Generic;

public class GlobalActionDistanceFinder
{
	private BuildingDistanceFinder buildingDistanceFinder = new BuildingDistanceFinder();
	private AttackDistanceFinder attackDistanceFinder = new AttackDistanceFinder();
	
	public List<GroundCell> GetAreaInRange(GroundCell center, int range)
	{
		return attackDistanceFinder.GetRangedAttackDistance(center, range);
	}
	
	public List<GroundCell> GetFullMapCells()
	{
		List<GroundCell> areaList = new List<GroundCell>();
		
		foreach (GroundCell cell in BattleMap.instance.mapCellList)
			areaList.Add(cell);
		
		return areaList;
	}
	
	public List<GroundCell> GetAllObjects(Player player, bool alliedObjects)
	{
		List<GroundCell> areaList = new List<GroundCell>();
		
		foreach (NullObject obj in BattleMap.instance.objectList)
		{
			if (player != obj.player ^ alliedObjects && obj is MaterialObject)
				areaList.Add(obj.position);
		}
		
		return areaList;
	}
	
	public List<GroundCell> GetRemovedCellsForTerramorf(List<GroundCell> areaList, TerrainType terrainType)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in areaList)
		{
			if (!cell.canBeTerraformated || cell.onCellObject != null || cell.terrainType == terrainType)
				removedCells.Add(cell);
		}
		
		return removedCells;
	}
	
	public List<GroundCell> GetRemovedCellsForSummon(List<GroundCell> areaList, GlobalSummon action)
	{
		List<GroundCell> removedCells = new List<GroundCell>();

		if (action.terrainTypeList.Count == 0)
		{
			foreach (GroundCell cell in areaList)
			{
				if (cell.onCellObject != null || cell.movingType > MovingType.Walk)
					removedCells.Add(cell);
			}
		}

		else
		{
			foreach (GroundCell cell in areaList)
			{
				if (buildingDistanceFinder.CheckCellForRemoving(cell, action.needFreeSpace, action.terrainTypeList))
					removedCells.Add(cell);
			}
		}
		return removedCells;
	}
}
