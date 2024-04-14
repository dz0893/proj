using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDistanceFinder
{
	private MovingDistanceFinder movingDistanceFinder = new MovingDistanceFinder();
	private EnemyDistanceFinder enemyDistanceFinder = new EnemyDistanceFinder();
	
	public GroundCell GetDepositCell(Unit unit)
	{
		if (unit.currentActionTargetList.Count != 0)
			return unit.currentActionTargetList[0];
		
		GroundCell depositCell = GetClosestFreeDepositCellToUnit(unit);
		
		if (unit.totalActionTargetList.Contains(depositCell))
		{
			unit.player.aiPlayer.targetedDepositCells.Add(depositCell);
			return depositCell;
		}
		else
			return null;
	}
	
	public GroundCell GetClosestToDepositCell(Unit unit)
	{
		List<GroundCell> movingCells = movingDistanceFinder.GetMoveDistance(unit);
		movingCells.Add(unit.position);
		GroundCell depositCell = GetClosestFreeDepositCellToUnit(unit);
		
		if (depositCell == null)
			return enemyDistanceFinder.GetClosestMovePosition(unit, unit.moveActionTargetList);
		else if (movingCells.Count == 0)
			return null;
		
		unit.player.aiPlayer.targetedDepositCells.Add(depositCell);
		GroundCell closestCell = movingCells[0];
		
		foreach (GroundCell cell in movingCells)
		{
			if ((cell.transform.position - depositCell.transform.position).magnitude == 0)
			{
				closestCell = cell.previousCell;
				break;
			}
			if ((cell.transform.position - depositCell.transform.position).magnitude 
			< (closestCell.transform.position - depositCell.transform.position).magnitude)
			{
				closestCell = cell;
			}
		}
		
		if (closestCell == unit.position)
			closestCell = null;
		
		return closestCell;
	}	
	
	public List<GroundCell> GetAllFreeDepositCells()
	{
		List<GroundCell> freeDepositList = new List<GroundCell>();
		
		foreach (GroundCell cell in BattleMap.instance.mapCellList)
		{
			if (cell.terrainType == TerrainType.GoldDeposit || cell.terrainType == TerrainType.OreDeposit)
			{
				if (cell.onCellObject == null)
					freeDepositList.Add(cell);
			}
		}
		
		return freeDepositList;
	}
	
	private GroundCell GetClosestFreeDepositCellToUnit(Unit unit)
	{
		List<GroundCell> freeDepositList = GetAllFreeDepositCells();
		GroundCell groundCell = null;
			
		if (freeDepositList.Count == 0)
			return null;
		
		Vector3 distance = freeDepositList[0].transform.position - unit.transform.position;
		groundCell = freeDepositList[0];
		
		foreach (GroundCell cell in freeDepositList)
		{
			if ((cell.transform.position - unit.transform.position).magnitude < distance.magnitude 
			&& !unit.player.aiPlayer.targetedDepositCells.Contains(cell))
			{
				distance = cell.transform.position - unit.transform.position;
				groundCell = cell;
			}
		}
		
		return groundCell;
	}
}
