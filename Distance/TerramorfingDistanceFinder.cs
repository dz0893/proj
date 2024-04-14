using System.Collections.Generic;

public class TerramorfingDistanceFinder
{
	private MovingDistanceFinder movingDistanceFinder = new MovingDistanceFinder();
	
	private List<TerrainType> exclusiveTerrainTypeList = new List<TerrainType>();
	
	public List<GroundCell> GetTerramorfingDistance(Unit unit)
	{
		List<GroundCell> distance;
		
		if (unit.choosenAction.range == ActionRange.Melee)
			distance = GetMeleeTerramorfingDistance(unit);
		
		else
			distance = GetRangedTerramorfingDistance(unit);
		
		return distance;
	}
	
	public List<GroundCell> GetMeleeTerramorfingDistance(Unit unit)
	{
		List<GroundCell> distance = movingDistanceFinder.GetMoveDistance(unit);
		
		Terramorfing action = unit.choosenAction as Terramorfing;
		
		List<GroundCell> removedCells = GetRemovedCells(distance, action.terrainType);
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	public List<GroundCell> GetRangedTerramorfingDistance(Unit unit)
	{
		List<GroundCell> distance = movingDistanceFinder.GetMoveDistance(unit);
		
		Terramorfing action = unit.choosenAction as Terramorfing;
		
		List<GroundCell> removedCells = GetRemovedCells(distance, action.terrainType);
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	private List<GroundCell> GetRemovedCells(List<GroundCell> distance, TerrainType terrainType)
	{
		List<GroundCell> removedCellList = new List<GroundCell>();
		
		foreach (GroundCell cell in distance)
		{
			if (CheckCellForRemoving(cell, terrainType))
				removedCellList.Add(cell);
		}
		
		return removedCellList;
	}
	
	private bool CheckCellForRemoving(GroundCell cell, TerrainType terrainType)
	{
		bool needToRemove = false;
		
		if (!cell.canBeTerraformated || cell.terrainType == terrainType || cell.onCellObject != null)
			needToRemove = true;
		
		if (terrainType == TerrainType.DeadGround && !needToRemove)
			needToRemove = !CheckForDeadGround(cell);
		
		if (terrainType == TerrainType.HolyGround && !needToRemove)
			needToRemove = !CheckForHolyGround(cell);
		
		return needToRemove;
	}
	
	private bool CheckForHolyGround(GroundCell checkedCell)
	{
		if (checkedCell.terrainType == TerrainType.Grass)
			return true;
		
		return false;
	}
	
	private bool CheckForDeadGround(GroundCell checkedCell)
	{
		bool canBeTerraformited = false;
		
		if (checkedCell.grave.Count > 0)
			canBeTerraformited = true;
		
		else
		{
			foreach (GroundCell cell in checkedCell.closestCellList)
			{
				if (cell.terrainType == TerrainType.DeadGround)
				{
					canBeTerraformited = true;
					break;
				}
			}
		}
		
		return canBeTerraformited;
	}
}
