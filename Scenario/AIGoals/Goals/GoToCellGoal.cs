using System.Collections.Generic;
using UnityEngine;

public class GoToCellGoal : AIGoal
{
	private int cellIndex;
	
	public GoToCellGoal(GoToCellGoalObject goalObject)
	{
		currentUnitGoal = goalObject.currentUnitGoal;
		currentUnit = goalObject.currentUnit;
		cellIndex = goalObject.cellIndex;
	}
	
	public override List<GroundCell> GetGoalList(Unit unit)
	{
		return BattleMap.instance.GetAllCellsWithIndex(cellIndex);
	}
	
	public override AbstractAction GetAction(Unit unit)
	{
		return unit.moveAction;
	}
}
