using System.Collections.Generic;
using UnityEngine;

public class MovingDistanceFinder
{
	public List<GroundCell> GetClosestRoadBetweenCells(GroundCell start, GroundCell finish)
	{
		List<GroundCell> closestWay = new List<GroundCell>();

		if (finish == null)
		{
			Debug.Log("no target for moving");
			return closestWay;
		}

		GroundCell checkedCell = start;
		GroundCell bestCell = start;
		float checkedCellRange = (finish.transform.position - checkedCell.transform.position).magnitude;
		float bestRange;

		int j = 0;

		while (!closestWay.Contains(finish) || j > 200)
		{
			bestRange = checkedCellRange;
			bestCell = checkedCell;

			for (int i = 0; i < checkedCell.closestCellList.Count; i++)
			{
				checkedCellRange = (finish.transform.position - checkedCell.closestCellList[i].transform.position).magnitude;

				if (checkedCellRange < bestRange)
				{
					bestRange = checkedCellRange;
					bestCell = checkedCell.closestCellList[i];
				}
			}
			
			checkedCell = bestCell;

			if (checkedCell == null)
			{
				Debug.Log("AlarmAnus");
				return closestWay;
			}

			if (!closestWay.Contains(checkedCell))
				closestWay.Add(checkedCell);
		}

		return closestWay;
	}

	public List<GroundCell> GetRoadTo(Unit unit, GroundCell goal)
	{
		List<GroundCell> road = new List<GroundCell>();
		List<GroundCell> reverseRoad = new List<GroundCell>();
		
		GroundCell currentCell;
		
		if (unit.currentActionTargetList.Contains(goal) && goal != unit.position && !unit.choosenAction.usedOnCasterOnly)
			currentCell = goal.previousCell;
		else
			currentCell = goal;
		
		while (currentCell != unit.position)
		{
			road.Add(currentCell);
			currentCell = currentCell.previousCell;
		}
		
		for (int i = road.Count - 1; i >= 0; i--)
			reverseRoad.Add(road[i]);
		
		return reverseRoad;
	}
	
	public List<GroundCell> GetFlyTargetTo(Unit unit, GroundCell goal)
	{
		List<GroundCell> flyTarget = new List<GroundCell>();
		
		if (goal.onCellObject == null)
		{
			flyTarget.Add(goal);
		}
		
		else
		{
			List<GroundCell> freeClosestCells = new List<GroundCell>();
			
			foreach (GroundCell cell in goal.closestCellList)
			{
				if (cell.onCellObject == null && unit.moveActionTargetList.Contains(cell))
					freeClosestCells.Add(cell);
			}
			
			if (freeClosestCells.Count != 0 && !goal.closestCellList.Contains(unit.position))
				flyTarget.Add(GetClosestCellToTargetFromList(freeClosestCells, unit.position));
		}
		
		return flyTarget;
	}
	
	private GroundCell GetClosestCellToTargetFromList(List<GroundCell> checkedCells, GroundCell target)
	{
		GroundCell closestCell = checkedCells[0];
		
		foreach (GroundCell cell in checkedCells)
		{
			if (GetDistanceBetweenCells(cell, target) < GetDistanceBetweenCells(closestCell, target))
				closestCell = cell;
		}
		
		return closestCell;
	}
	
	private float GetDistanceBetweenCells(GroundCell cell1, GroundCell cell2)
	{
		return Vector3.Magnitude(cell1.transform.position - cell2.transform.position);
	}
	
	public List<GroundCell> GetFlyingDistance(Unit unit, int flyDistance)
	{
		List<GroundCell> distance = new List<GroundCell>();
		List<GroundCell> removedCells = new List<GroundCell>();
		List<GroundCell> checkedCells = new List<GroundCell>();
		
		GroundCell center = unit.position;
		checkedCells.Add(center);
		distance.Add(center);
		
		int movePoints = flyDistance; 
		
		if (movePoints == 0)
			movePoints = unit.currentMovePoints / GroundSettings.defaulTerrainMovingCost;
		
		for (int i = 1; i < movePoints + 1; i++)
		{
			foreach (GroundCell checkedCell in checkedCells)
			{
				foreach (GroundCell cell in checkedCell.closestCellList)
				{
					if (!distance.Contains(cell))
					{
						distance.Add(cell);
						cell.totalMovingCost = i * GroundSettings.defaulTerrainMovingCost;
						cell.previousCell = checkedCell;
					}
				}
			}
			
			foreach (GroundCell cell in distance)
			{
				if (!checkedCells.Contains(cell))
					checkedCells.Add(cell);
			}
		}
		
		foreach (GroundCell cell in distance)
		{
			if (cell.onCellObject != null || cell.movingType > MovingType.Walk)
				removedCells.Add(cell);
		}
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	private void AddClosestCellsToDistanceForFlyingAndSetMovingCost(GroundCell center, List<GroundCell> distance, int movingCost)
	{
		foreach (GroundCell cell in center.closestCellList)
		{
			if (cell.onCellObject != null && !distance.Contains(cell))
			{
				distance.Add(cell);
				cell.totalMovingCost = movingCost;
			}
		}
	}
	
	public List<GroundCell> GetMoveDistance(Unit unit)
	{
		GroundCell center = unit.position;
		int movePoints = unit.currentMovePoints;
		
		List<GroundCell> waitedCells = new List<GroundCell>();
		List<GroundCell> checkedCells = new List<GroundCell>();
		List<GroundCell> maxDistance = GetRadiusDistance(center, movePoints, unit);
		
		for (int i = 0; i < movePoints + 1; i++)
		{
			waitedCells = GetRadiusDistance(center, i, unit);
			
			foreach (GroundCell cell in checkedCells)
				waitedCells.Remove(cell);
			
			foreach (GroundCell cell in waitedCells)
			{
				if (cell.previousCell != null || cell == center)
					SetMovingCostToClosestCells(cell, unit);
			}
				
			checkedCells = GetCurrentCellList(waitedCells);
		}
		
		List<GroundCell> distance = GetCurrentCellList(maxDistance);
		
		center.totalMovingCost = 0;
		
		foreach (GroundCell cell in maxDistance)
		{
			if (cell.totalMovingCost > movePoints || cell.totalMovingCost == 0 || cell == center)
				distance.Remove(cell);
		}
		
		return distance;
	}
	
	public List<GroundCell> GetCurrentCellList(List<GroundCell> cellList)
	{
		List<GroundCell> currentCellList = new List<GroundCell>();
		
		foreach(GroundCell cell in cellList)
			currentCellList.Add(cell);
		
		return currentCellList;
	}
	
	public List<GroundCell> GetRadiusDistance(GroundCell center, int radius, Unit currentObject)
	{
		List<GroundCell> distance = new List<GroundCell>();
		
		List<GroundCell> checkedCells = new List<GroundCell>();
		
		checkedCells.Add(center);
		
		List<GroundCell> uncheckedCells = new List<GroundCell>();
		
		distance.Add(center);
		
		for (int i = 0; i < radius; i++)
		{
			foreach (GroundCell chekedCell in checkedCells)
			{
				foreach (GroundCell cell in chekedCell.closestCellList)
				{
					if (!distance.Contains(cell))
						distance.Add(cell);
				}
			}
			
			foreach (GroundCell cell in distance)
			{
				if (!checkedCells.Contains(cell))
					checkedCells.Add(cell);
			}
		}
		
		for (int i = 0; i < distance.Count; i++)
		{
			if(distance[i].movingType > currentObject.movingType)
			{
				distance.Remove(distance[i]);
				i--;
			}
		}
		
		RemoveCellsWithObjectsFromList(distance, currentObject);
		
		return distance;
	}
	
	private void RemoveCellsWithObjectsFromList(List<GroundCell> list, Unit currentObject)
	{
		List<GroundCell> copyList = GetCurrentCellList(list);
		
		foreach (GroundCell cell in copyList)
		{
			if (cell.onCellObject != null && cell.onCellObject != currentObject)
				list.Remove(cell);
		}
	}	
	
	private void SetMovingCostToClosestCells(GroundCell center, Unit unit)
	{
		foreach (GroundCell cell in center.closestCellList)
		{
			if(cell.closestCellList.Contains(center))
			{
				int wastedMovePoints = GetMoveCostToCell(cell, unit);
				wastedMovePoints += center.totalMovingCost;
				
				if (cell.totalMovingCost == 0 || cell.totalMovingCost > wastedMovePoints 
				|| (cell.totalMovingCost == wastedMovePoints && cell.previousCell.isNegativeEffected) 
				&& !unit.terrainKnowingList.Contains(cell.previousCell.terrainType))
				{
					cell.totalMovingCost = wastedMovePoints;
					cell.SetPreviousCell(center);
				}
			}
		}
	}
	
	public int GetMoveCostToCell(GroundCell cell, Unit unit)
	{
		if (unit.movingType == MovingType.Fly || unit.terrainKnowingList.Contains(cell.terrainType))
			return GroundSettings.roadMovingCost;
		else
			return cell.movingCost;
	}
}

