using System.Collections.Generic;
using UnityEngine;

public class BuildingDistanceFinder
{
	private MovingDistanceFinder movingDistanceFinder = new MovingDistanceFinder();
	private AttackDistanceFinder attackDistanceFinder = new AttackDistanceFinder();
	
	public List<GroundCell> GetBuildObjectDistance(Unit unit)
	{
		List<GroundCell> distance;
		
		if (unit.choosenAction.range == ActionRange.Melee)
			distance = GetMeleeBuildingDistance(unit);
		
		else
			distance = GetRangedBuildingDistance(unit);
		
		return distance;
	}
	
	public List<GroundCell> GetTransformToBuildingDistance(Unit unit)
	{
		List<GroundCell> distance = movingDistanceFinder.GetMoveDistance(unit);
		
		Transformate action = unit.choosenAction as Transformate;
		
		List<GroundCell> removedCells = GetRemovedCells(distance, action);
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		if (!SoftCheckCellForRemoving(unit.position, action))
			distance.Add(unit.position);
		
		return distance;
	}
	
	private List<GroundCell> GetMeleeBuildingDistance(Unit unit)
	{
		List<GroundCell> distance = movingDistanceFinder.GetMoveDistance(unit);
		
		Build buildAction = unit.choosenAction as Build;
		
		if (!buildAction.isMoveCostedAction)
			SetCellsInMoveRadius(unit.position, distance, buildAction);
		
		List<GroundCell> removedCells = GetRemovedCells(distance, buildAction);
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	private void SetCellsInMoveRadius(GroundCell center, List<GroundCell> distance, Build buildAction)
	{
		List<GroundCell> addedCells = new List<GroundCell>();
		
		distance.Add(center);
		
		foreach (GroundCell cell in distance)
		{
			foreach (GroundCell closestCell in cell.closestCellList)
			{
				if (!distance.Contains(closestCell)
				&& !CheckCellForRemoving(closestCell, buildAction.needFreeSpace, buildAction.terrainTypeList))
					addedCells.Add(closestCell);
			}
		}
		
		distance.Remove(center);
		
		foreach (GroundCell cell in addedCells)
			distance.Add(cell);
	}
	
	private List<GroundCell> GetRangedBuildingDistance(Unit unit)
	{
		Build buildAction = unit.choosenAction as Build;
		
		List<GroundCell> distance = attackDistanceFinder.GetRangedAttackDistance(unit.position, unit.GetActionData(buildAction).attackRange);
		
		List<GroundCell> removedCells = GetRemovedCells(distance, buildAction);
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	private List<GroundCell> GetRemovedCells(List<GroundCell> cellList, Build buildAction)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		for (int i = 0; i < cellList.Count; i++)
		{
			if (CheckCellForRemoving(cellList[i], buildAction.needFreeSpace, buildAction.terrainTypeList)
			&& !removedCells.Contains(cellList[i]) || (buildAction.cellIndexList.Count > 0 && !buildAction.cellIndexList.Contains(cellList[i].index)))
				removedCells.Add(cellList[i]);

		//	if (buildAction.isMoveCostedAction && !unit.moveActionTargetList.Contains(cellList[i]))
		//		removedCells.Add(cellList[i]);
		}
		
		return removedCells;
	}
	
	public bool CheckCellForRemoving(GroundCell checkedCell, bool needFreeSpace, List<TerrainType> terrainTypeList)
	{
		bool needToRemove = false;
		
		if (checkedCell.onCellObject != null || (checkedCell.unmaterialOnCellObject != null 
		&& checkedCell.unmaterialOnCellObject.initer != null))
			needToRemove = true;
		
		else
		{
			foreach (GroundCell cell in checkedCell.closestCellList)
			{
				if (cell.terrainType == TerrainType.GoldDeposit
				|| cell.terrainType == TerrainType.OreDeposit)
				{
					needToRemove = true;
					break;
				}
				
				if ((cell.unmaterialOnCellObject != null 
				&& cell.unmaterialOnCellObject.initer != null
				|| cell.onCellObject != null && !cell.onCellObject.isMovable) 
				&& needFreeSpace)
				{
					needToRemove = true;
					break;
				}
			}
		}
		
		if (!terrainTypeList.Contains(checkedCell.terrainType))
			needToRemove = true;
		
		return needToRemove;
	}
	
	private List<GroundCell> GetRemovedCells(List<GroundCell> cellList, Transformate action)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		for (int i = 0; i < cellList.Count; i++)
		{
			if (CheckCellForRemoving(cellList[i], action) && !removedCells.Contains(cellList[i]))
				removedCells.Add(cellList[i]);
		}
		
		return removedCells;
	}
	
	private bool CheckCellForRemoving(GroundCell checkedCell, Transformate action)
	{
		bool needToRemove = false;
		
		if (checkedCell.onCellObject != null || (checkedCell.unmaterialOnCellObject != null 
		&& checkedCell.unmaterialOnCellObject.initer != null))
			needToRemove = true;
		
		else
			needToRemove = SoftCheckCellForRemoving(checkedCell, action);
		
		return needToRemove;
	}
	
	private bool SoftCheckCellForRemoving(GroundCell checkedCell, Transformate action)
	{
		bool needToRemove = false;
		
		foreach (GroundCell cell in checkedCell.closestCellList)
		{
			if (cell.terrainType == TerrainType.GoldDeposit || cell.terrainType == TerrainType.OreDeposit)
			{
				needToRemove = true;
				break;
			}
				
			if ((cell.unmaterialOnCellObject != null 
			&& cell.unmaterialOnCellObject.initer != null
			|| cell.onCellObject != null && !cell.onCellObject.isMovable))
			{
				needToRemove = true;
				break;
			}
		}
			
		if (checkedCell.terrainType == TerrainType.GoldDeposit || checkedCell.terrainType == TerrainType.OreDeposit)
			needToRemove = true;
		
		return needToRemove;
	}
	
}
