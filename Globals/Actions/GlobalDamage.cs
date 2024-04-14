using System.Collections.Generic;

public class GlobalDamage : GlobalAction
{
	public DamageType damageType { get; private set; }
	public bool onMechs { get; private set; }
	
	public GlobalDamage(GlobalDamageObject actionObject)
	{
		this.actionObject = actionObject;
		
		damageType = actionObject.damageType;
		onMechs = actionObject.onMechs;
		
		InitDescriptionList();
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		target.onCellObject.GetAttack(actionValue, damageType);
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in areaList)
		{
			if (cell.onCellObject == null)
				removedCells.Add(cell);
			else if (cell.onCellObject.team == player.team || onMechs && !cell.onCellObject.isMech)
				removedCells.Add(cell);
		}
		
		return removedCells;
	}
	
	protected override string GetActionValueString()
	{
		return UISettings.damage + actionValue + " " + UISettings.GetDamageTypeName(damageType);
	}
	
	public override string GetRenderedText(GroundCell cell)
	{
		return actionTextGetter.GetDamageText(cell.onCellObject, actionValue, damageType);
	}
}
