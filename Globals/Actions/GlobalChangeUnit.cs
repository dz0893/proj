using System.Collections.Generic;
using UnityEngine;

public class GlobalChangeUnit : GlobalAction
{
	public List<Unit> targetUnitList { get; private set; }
	public List<Unit> changetUnitList { get; private set; }
	
	public GlobalChangeUnit(GlobalChangeUnitObject actionObject)
	{
		this.actionObject = actionObject;
		
		targetUnitList = actionObject.targetUnitList;
		changetUnitList = actionObject.changetUnitList;
		
		InitDescriptionList();
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		Unit unit = target.onCellObject as Unit;
		int index = GetUnitIndex(unit);
		
		unit.RemoveFromGame();
		BattleMap.initObject.Invoke(changetUnitList[index], player, target);
	}
	
	private int GetUnitIndex(Unit unit)
	{
		int index = 0;
		
		for (int i = 0; i < targetUnitList.Count; i++)
		{
			if (targetUnitList[i].Name.Equals(unit.Name))
				index = i;
		}
		
		return index;
	}
	
	private bool CheckListForContainingObject(NullObject obj)
	{
		for (int i = 0; i < targetUnitList.Count; i++)
		{
			if (targetUnitList[i].Name.Equals(obj.Name))
				return true;
		}
		
		return false;
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in areaList)
		{
			if (cell.onCellObject == null)
				removedCells.Add(cell);
			else if (!CheckListForContainingObject(cell.onCellObject))
				removedCells.Add(cell);
		}
		
		return removedCells;
	}
	
	protected override bool CheckCurrentActivate(Player player)
	{
		if (player.currentUnitLimit >= player.maxUnitLimit)
			return false;
		else
			return true;		
	}
}
