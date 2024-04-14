using System.Collections.Generic;

public class GlobalHealing : GlobalAction
{
	public bool onMechs { get; private set; }
	
	public GlobalHealing(GlobalHealingObject actionObject)
	{
		this.actionObject = actionObject;
		
		onMechs = actionObject.onMechs;
		
		InitDescriptionList();
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		target.onCellObject.RestoreHealth(actionValue);
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in areaList)
		{
			if (cell.onCellObject == null)
				removedCells.Add(cell);
				
			else if ((cell.onCellObject.isMech ^ onMechs)
			|| (cell.onCellObject.currentStats.maxHealth <= cell.onCellObject.currentHealth))
				removedCells.Add(cell);
		}
		
		return removedCells;
	}
	
	protected override string GetActionValueString()
	{
		return UISettings.heal + actionValue;
	}
	
	public override string GetRenderedText(GroundCell cell)
	{
		return actionTextGetter.GetHealingText(cell.onCellObject, actionValue);
	}
}
