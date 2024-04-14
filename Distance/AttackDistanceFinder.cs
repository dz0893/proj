using System.Collections.Generic;
using UnityEngine;

public class AttackDistanceFinder
{
	private MovingDistanceFinder movingDistanceFinder = new MovingDistanceFinder();
	
	public List<GroundCell> GetAttackDistance(Unit unit)
	{
		List<GroundCell> distance;
		
		if (unit.choosenAction.range == ActionRange.Melee)
			distance = GetMeleeAttackDistance(unit);
		else
			distance = GetRangedAttackDistanceWithMinRange(unit);
		
		return distance;
	}
	
	public List<GroundCell> GetMeleeAttackDistance(Unit unit)
	{
		List<GroundCell> distance = new List<GroundCell>();
		
		List<GroundCell> movingDistance = unit.moveAction.GetDistance(unit);
		
		movingDistance.Add(unit.position);
		
		foreach (GroundCell movingCell in movingDistance)
		{
			List<GroundCell> cellsWithEnemy = GetClosestCellWithEnemy(movingCell, unit);
			
			foreach (GroundCell cell in cellsWithEnemy)
			{
				if (!distance.Contains(cell))
					distance.Add(cell);
			}
		}
		
		return distance;
	}
	
	private List<GroundCell> GetClosestCellWithEnemy(GroundCell center, Unit unit)
	{
		List<GroundCell> cellsWithEnemy = new List<GroundCell>();
		
		foreach (GroundCell cell in center.closestCellList)
		{
			if (CheckTargetForAddToList(cell, unit))
			{
				if (cell.previousCell == null || cell.previousCell.totalMovingCost >= center.totalMovingCost
				|| unit.moveAction is Fly)
				{
					cell.previousCell = center;
					cellsWithEnemy.Add(cell);
				}
			}
		}
		
		return cellsWithEnemy;
	}
	
	public List<GroundCell> GetRangedAttackDistanceWithMinRange(Unit unit)
	{
		List<GroundCell> distance = GetRangedActionDistanceWithMinRange(unit.position, unit.currentStats.minAttackRange, unit.currentStats.attackRange);
		
		List<GroundCell> targetList = new List<GroundCell>();
		
		foreach(GroundCell cell in distance)
		{
			if (CheckTargetForAddToList(cell, unit))
				targetList.Add(cell);
		}
		
		return targetList;
	}

	public List<GroundCell> GetRangedActionDistanceWithMinRange(GroundCell center, int minRange, int maxRange)
	{
		List<GroundCell> distance = GetRangedAttackDistance(center, maxRange);
		List<GroundCell> minDistance = GetRangedAttackDistance(center, minRange);
		
		foreach(GroundCell cell in minDistance)
			distance.Remove(cell);
		
		return distance;
	}
	
	public bool CheckTargetForAddToList(GroundCell cell, Unit unit)
	{
		bool needAddToList = false;
		
		if (cell.onCellObject != null)
		{
			if (unit.choosenAction.actionType == ActionType.Offensive && cell.onCellObject.team != unit.team)
				needAddToList = true;
			else if (unit.choosenAction.actionType == ActionType.Defensive && cell.onCellObject.team == unit.team)
				needAddToList = true;
		}
		else if (unit.choosenAction.actionType == ActionType.OnDeadUnit && CheckCellForDeadUnits(cell))
			needAddToList = true;
		
		return needAddToList;
	}
	
	private bool CheckCellForDeadUnits(GroundCell cell)
	{
		if (cell.grave.Count != 0 || cell.terrainType == TerrainType.DeadGround && cell.onCellObject == null)
			return true;
		
		else
			return false;
	}
	
	public List<GroundCell> GetRangedAttackTargetCells(Unit unit, int range)
	{
		List<GroundCell> distance = GetRangedAttackDistance(unit.position, range);
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in distance)
		{
			if (!CheckTargetForAddToList(cell, unit))
				removedCells.Add(cell);
		}
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	public List<GroundCell> GetRangedAttackDistance(GroundCell center, int range)
	{
		List<GroundCell> distance = new List<GroundCell>();
		
		List<GroundCell> checkedCells = new List<GroundCell>();
		checkedCells.Add(center);
		
		List<GroundCell> uncheckedCells = new List<GroundCell>();
		
		for (int i = 0; i < range; i++)
		{
			foreach (GroundCell chekedCell in checkedCells)
			{
				foreach (GroundCell cell in chekedCell.closestCellList)
				{
					if (cell != center && !checkedCells.Contains(cell) && !distance.Contains(cell))
						distance.Add(cell);
				}
			}
			
			foreach (GroundCell cell in distance)
			{
				if (!checkedCells.Contains(cell))
					checkedCells.Add(cell);
			}
		}
		
		return distance;
	}
}
