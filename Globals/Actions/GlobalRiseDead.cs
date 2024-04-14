using System.Collections.Generic;
using UnityEngine;

public class GlobalRiseDead : GlobalAction
{
	public Unit warriorUnit { get; private set; }
	public Unit archerUnit { get; private set; }
	public Unit mageUnit { get; private set; }
	
	public GlobalRiseDead(GlobalRiseDeadObject actionObject)
	{
		this.actionObject = actionObject;
		
		warriorUnit = actionObject.warriorUnit;
		archerUnit = actionObject.archerUnit;
		mageUnit = actionObject.mageUnit;
		
		InitDescriptionList();
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		Unit risedUnit = GetRisedUnit(target);
		
		BattleMap.initObject.Invoke(risedUnit, player, target);
		
		if (target.grave.Count != 0)
			target.RemoveUnitFromGrave(target.grave[0]);
	}
	
	private Unit GetRisedUnit(GroundCell position)
	{
		if (position.grave.Count == 0)
			return warriorUnit;
		else if (position.grave[0].unitType == UnitType.Mage)
			return mageUnit;
		else if (position.grave[0].unitType == UnitType.Archer)
			return archerUnit;
		else
			return warriorUnit;
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in areaList)
		{
			if (cell.onCellObject != null)
			{
				removedCells.Add(cell);
			}
			else if (cell.grave.Count == 0 && cell.terrainType != TerrainType.DeadGround)
			{
				removedCells.Add(cell);
			}
		}
		
		return removedCells;
	}
	
	protected override bool CheckCurrentActivate(Player player)
	{
		currentActivatingWarningText = "";
		
		if (warriorUnit.leadershipCost > player.maxUnitLimit - player.currentUnitLimit)
		{
			currentActivatingWarningText = UISettings.notEnoughtLimit;
			return false;
		}
		
		return true;
	}
	
	public override string GetRenderedText(GroundCell position)
	{
		return GetRisedUnit(position).Name;
	}
}
