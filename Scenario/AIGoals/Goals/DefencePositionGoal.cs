using System.Collections.Generic;
using UnityEngine;

public class DefencePositionGoal : AIGoal
{
	private int defendedCellsIndex;
	private int basicCellIndex;
	
	private List<GroundCell> defencedArea;
	private GroundCell basicCell;
	
	public DefencePositionGoal(DefencePositionGoalObject goalObject)
	{
		currentUnitGoal = goalObject.currentUnitGoal;
		currentUnit = goalObject.currentUnit;
		defendedCellsIndex = goalObject.defendedCellsIndex;
		basicCellIndex = goalObject.basicCellIndex;
		
		
	}
	
	public override List<GroundCell> GetGoalList(Unit unit)
	{
		if (!IsEnemyInDefenceArea(unit, defencedArea))
		{
			defencedArea = new List<GroundCell>();
			defencedArea.Add(basicCell);
		}
		
		return defencedArea;
	}
	
	public override AbstractAction GetAction(Unit unit)
	{
		defencedArea = BattleMap.instance.GetAllCellsWithIndex(defendedCellsIndex);
		basicCell = BattleMap.instance.GetAllCellsWithIndex(basicCellIndex)[0];
		defencedArea.Add(basicCell);
		
		if (!IsEnemyInDefenceArea(unit, defencedArea))
			return unit.moveAction;
		else
			return unit.actionList[0];
	}
	
	private bool IsEnemyInDefenceArea(Unit unit, List<GroundCell> defencedArea)
	{
		foreach (GroundCell cell in defencedArea)
		{
			if (cell.onCellObject != null && cell.onCellObject.team != unit.team)
				return true;
		}
		
		return false;
	}
}
