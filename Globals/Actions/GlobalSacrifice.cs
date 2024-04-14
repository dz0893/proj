using System.Collections.Generic;
using UnityEngine;

public class GlobalSacrifice : GlobalAction
{
	public bool isMaxHealthConverted { get; private set; }
	public float effectivnese { get; private set; }
	
	public GlobalSacrifice(GlobalSacrificeObject actionObject)
	{
		this.actionObject = actionObject;
		
		isMaxHealthConverted = actionObject.isMaxHealthConverted;
		effectivnese = actionObject.effectivnese;
		
		InitDescriptionList();
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		player.hero.RestoreHealth(GetRestoringValue(target.onCellObject));
		target.onCellObject.Death();
	}
	
	private int GetRestoringValue(MaterialObject obj)
	{
		if (isMaxHealthConverted)
			return (int)(effectivnese * obj.currentStats.maxHealth);
		else
			return (int)(effectivnese * obj.currentHealth);
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in areaList)
		{
			if (cell.onCellObject == null)
				removedCells.Add(cell);
			else if (cell.onCellObject.player != player)
				removedCells.Add(cell);
			else if (cell.onCellObject == player.hero)
				removedCells.Add(cell);
			else if (cell.onCellObject is Structure)
				removedCells.Add(cell);
		}
		
		return removedCells;
	}
	
	protected override string GetActionValueString()
	{
		string actionValueString = UISettings.Restore + (int)(effectivnese * 100) + "%";
		
		if (isMaxHealthConverted)
			actionValueString += UISettings.maximal;
		else
			actionValueString += UISettings.current;
		
		actionValueString += UISettings.targetHealth;
		
		return actionValueString;
	}
	
	public override string GetRenderedText(GroundCell position)
	{
		return GetRestoringValue(position.onCellObject).ToString();
	}
}
