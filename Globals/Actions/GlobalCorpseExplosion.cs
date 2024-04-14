using System.Collections.Generic;
using UnityEngine;

public class GlobalCorpseExplosion : GlobalAction, IAreaAction
{
	public DamageType damageType { get; private set; }
	public bool dealDamageToClosestCells { get; private set; }
	public float closestCellsDamageModifier { get; private set; }
	
	public int area => 1;
	
	public override List<GroundCell> GetAreaDistance(GroundCell position)
	{
		List<GroundCell> areaDistanceList = new List<GroundCell>();
		
		areaDistanceList.Add(position);
		
		if (dealDamageToClosestCells)
		{
			foreach (GroundCell cell in position.closestCellList)
				areaDistanceList.Add(cell);
		}
		
		return areaDistanceList;
	}
	
	public GlobalCorpseExplosion(GlobalCorpseExplosionObject actionObject)
	{
		this.actionObject = actionObject;
		
		damageType = actionObject.damageType;
		dealDamageToClosestCells = actionObject.dealDamageToClosestCells;
		closestCellsDamageModifier = actionObject.closestCellsDamageModifier;
		
		InitDescriptionList();
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		int centerDamageValue = actionValue * target.grave.Count;
		int closestCellsDamageValue = (int) (centerDamageValue * closestCellsDamageModifier);
		
		target.CleanGrave();
		
		if (target.onCellObject != null)
			target.onCellObject.GetAttack(centerDamageValue, damageType);
		
		if (dealDamageToClosestCells)
		{
			foreach (GroundCell cell in target.closestCellList)
			{
				if (cell.onCellObject != null)
					cell.onCellObject.GetAttack(closestCellsDamageValue, damageType);
			}
		}
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in areaList)
		{
			if (cell.grave.Count == 0)
				removedCells.Add(cell);
		}
		
		return removedCells;
	}
	
	protected override string GetActionValueString()
	{
		return UISettings.DamagePerBody + actionValue + " " + UISettings.GetDamageTypeName(damageType) + "\n"
		+ UISettings.DamageToClosestCells + (closestCellsDamageModifier * 100) + "%";
	}
	
	public override string GetRenderedText(GroundCell cell)
	{
		int centerDamageValue = actionValue * ActionRender.areaList[0].grave.Count;
		int closestCellsDamageValue = (int) (centerDamageValue * closestCellsDamageModifier);
		
		if (ActionRender.areaList[0] == cell)
			return actionTextGetter.GetDamageText(cell.onCellObject, centerDamageValue, damageType);
		else
			return actionTextGetter.GetDamageText(cell.onCellObject, closestCellsDamageValue, damageType);
	}
}
