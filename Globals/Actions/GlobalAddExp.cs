using System.Collections.Generic;

public class GlobalAddExp : GlobalAction
{
	public GlobalAddExp(GlobalAddExpObject actionObject)
	{
		this.actionObject = actionObject;
		
		InitDescriptionList();
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		Unit unit = target.onCellObject as Unit;
		
		unit.experience.AddExp(actionValue);
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in areaList)
		{
			if (cell.onCellObject != null && cell.onCellObject is Unit)
			{
				Unit unit = cell.onCellObject as Unit;
				
				if (unit.experience.maxLevel == 0)
					removedCells.Add(cell);
			}
			else
				removedCells.Add(cell);
		}
		
		return removedCells;
	}
	
	protected override string GetActionValueString()
	{
		return UISettings.addExperience + actionValue;
	}
	
	public override string GetRenderedText(GroundCell cell)
	{
		return actionTextGetter.GetExpAddingText(cell.onCellObject, actionValue);
	}
}
