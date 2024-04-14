using System.Collections.Generic;

public class GlobalRecharge : GlobalAction
{
	public GlobalRecharge(GlobalRechargeObject actionObject)
	{
		this.actionObject = actionObject;
		
		InitDescriptionList();
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		Unit unit = target.onCellObject as Unit;
		unit.attackIsRecharget = true;
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in areaList)
		{
			if (cell.onCellObject == null)
				removedCells.Add(cell);
			else if (!(cell.onCellObject is Unit))
				removedCells.Add(cell);
			else
			{
				Unit unit = cell.onCellObject as Unit;
				
				if (unit.attackIsRecharget)
					removedCells.Add(cell);
			}
		}
		
		return removedCells;
	}
}
