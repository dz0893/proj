using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalChangeMana : GlobalAction
{
	public bool isRestoring { get; private set; }
	
	public GlobalChangeMana(GlobalChangeManaObject actionObject)
	{
		this.actionObject = actionObject;
		
		isRestoring = actionObject.isRestoring;
		InitDescriptionList();
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		if (isRestoring)
		{
			target.onCellObject.RestoreMana(actionValue);
		}
		else
		{
			target.onCellObject.WasteMana(actionValue);
			AnimationController.write(target.transform.position, "-" + actionValue, Color.blue);
		}
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in areaList)
		{
			if (cell.onCellObject == null)
				removedCells.Add(cell);
			
			else
			{
				if (cell.onCellObject.currentStats.maxMana == 0)
					removedCells.Add(cell);
			
				else if (isRestoring)
				{
					if (cell.onCellObject.player != player)
						removedCells.Add(cell);
					else if (cell.onCellObject.currentMana >= cell.onCellObject.currentStats.maxMana)
						removedCells.Add(cell);
				}
				else
				{
					if (cell.onCellObject.team == player.team)
						removedCells.Add(cell);
				}
			}
		}
		
		return removedCells;
	}
	
	protected override string GetActionValueString()
	{
		if (isRestoring)
			return UISettings.Restore + actionValue + UISettings.mana;
		
		else
			return UISettings.Burn + actionValue + UISettings.mana;
	}
	
	public override string GetRenderedText(GroundCell cell)
	{
		if (isRestoring)
			return actionTextGetter.GetRestoreManaText(cell.onCellObject, actionValue);
		
		else
			return actionTextGetter.GetWastedMana(cell.onCellObject, actionValue);
	}
}
