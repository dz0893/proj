using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSetEffect : GlobalAction
{
	public ActionEffect actionEffect { get; private set; }
	
	public GlobalSetEffect(GlobalSetEffectObject actionObject)
	{
		this.actionObject = actionObject;
		
		actionEffect = actionObject.actionEffect;
		
		InitDescriptionList();
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		actionEffect.Activate(target.onCellObject as Unit);
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in areaList)
		{
			if (cell.onCellObject == null)
				removedCells.Add(cell);
			
			else if (!(cell.onCellObject is Unit) || cell.onCellObject.team == player.team ^ !actionEffect.isNegative)
				removedCells.Add(cell);
		}
		
		return removedCells;
	}
	
	protected override void InitChildrenDescription()
	{
		if (actionEffect.Name != "")
			AddStringToRequiresList(UISettings.Effect + actionEffect.Name);
		
		List<string> effectDescription = actionEffect.GetDescription();
		
		foreach (string descriptionString in effectDescription)
			AddStringToRequiresList(descriptionString);
	}
}
