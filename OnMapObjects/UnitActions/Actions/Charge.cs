using System.Collections.Generic;
using UnityEngine;

public class Charge : Attack
{
	private MovingDistanceFinder movingDistanceFinder = new MovingDistanceFinder();
	
	[SerializeField] private bool _fullMovePointsOnly = true;

	public override ActionRange range => ActionRange.Melee;
	
	public override List<GroundCell> GetAreaOfAction(Unit unit)
	{
		List<GroundCell> distance = attackDistanceFinder.GetRangedActionDistanceWithMinRange(unit.position, _minAttackRange, (unit.currentStats.maxMovePoints + 2)/2);
		List<GroundCell> fullDistance = attackDistanceFinder.GetRangedAttackDistance(unit.position, (unit.currentStats.maxMovePoints + 2)/2);

		List<GroundCell> linedCells = GetAllLineCells(unit.position, fullDistance);
		List<GroundCell> areaOfAction = new List<GroundCell>();

		foreach (GroundCell cell in linedCells)
		{
			if (distance.Contains(cell))
			{
				areaOfAction.Add(cell);
			}
		}
		
		return areaOfAction;
	}

	public override List<GroundCell> GetDistance(Unit unit)
	{
		List<GroundCell> distance = attackDistanceFinder.GetAttackDistance(unit);
		
		List<GroundCell> removedCells = attackDistanceFinder.GetRangedAttackDistance(unit.position, _minAttackRange);
		
		foreach (GroundCell cell in distance)
		{
			if (!IsCellOnLine(unit, cell) && !removedCells.Contains(cell))
				removedCells.Add(cell);
		}
		
		foreach (GroundCell cell in removedCells)
		{
			if (distance.Contains(cell))
				distance.Remove(cell);
		}
		
		return distance;
	}
	
	private bool IsCellOnLine(Unit unit, GroundCell goal)
	{
		List<GroundCell> road = movingDistanceFinder.GetRoadTo(unit, goal);
		
		Vector3 pointVector = goal.transform.position - goal.previousCell.transform.position;
		
		bool cellOnLine = true;
		
		for (int i = 0; i < road.Count; i++)
		{
			Vector3 currentPointVector = road[i].transform.position - road[i].previousCell.transform.position;
			
			if (pointVector != currentPointVector)
			{
				cellOnLine = false;
				break;
			}
		}
		
		return cellOnLine;
	}
	
	private List<GroundCell> GetAllLineCells(GroundCell center, List<GroundCell> area)
	{
		List<GroundCell> cellsOnLine = new List<GroundCell>();


		foreach (GroundCell cell in center.closestCellList)
		{
			List<GroundCell> cellLine = GetCellLineInArea(center, cell, area);

			foreach (GroundCell currentCell in cellLine)
			{
				if (!cellsOnLine.Contains(currentCell))
				{
					cellsOnLine.Add(currentCell);
				}
			}
		}

		return cellsOnLine;
	}

	private List<GroundCell> GetCellLineInArea(GroundCell cell1, GroundCell cell2, List<GroundCell> area)
	{
		List<GroundCell> cellLine = new List<GroundCell>();
		cellLine.Add(cell1);
		cellLine.Add(cell2);

		bool areaHaveNextCell = true;

		while (areaHaveNextCell)
		{
			GroundCell nextCell = GetNextCellInLine(cellLine[cellLine.Count - 2], cellLine[cellLine.Count - 1]);
			if (nextCell != null && area.Contains(nextCell))
			{
				cellLine.Add(nextCell);
			}
			else
			{
				areaHaveNextCell = false;
			}
		}

		return cellLine;
	}

	private GroundCell GetNextCellInLine(GroundCell cell1, GroundCell cell2)
	{
		GroundCell nextCell = null;
		Vector3 pointVector = cell1.transform.position - cell2.transform.position;

		foreach (GroundCell cell in cell2.closestCellList)
		{
			Vector3 currentPointVector = cell2.transform.position - cell.transform.position;

			if (pointVector == currentPointVector)
			{
				nextCell = cell;
				break;
			}
		}

		return nextCell;
	}

	protected override string GetRangeString(ActionData actionData)
	{
		return UISettings.chargeRange + (_minAttackRange + 1);
	}

	protected override bool CurrentActivateCheck(Unit unit)
	{
		if (unit.currentMovePoints < unit.currentStats.maxMovePoints && _fullMovePointsOnly)
			return false;
			
		else	
			return true;
	}
}
