using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransiteMana : AbstractAction
{
	public override bool endedTurnAction => true;
	public override bool damageFromUnitStats => false;
	public override bool rangeFromUnitStats => true;
	
	[SerializeField] private int _maxValue;
	public int maxValue => _maxValue;
	
	[SerializeField] private ActionRange _range;
	public override ActionRange range => _range;
	
	[SerializeField] private ActionType _actionType;
	public override ActionType actionType => _actionType;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		List<GroundCell> targetList = attackDistanceFinder.GetAttackDistance(unit);
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in targetList)
		{
			if (cell.onCellObject.currentStats.maxMana == 0)
				removedCells.Add(cell);
		}
		
		foreach (GroundCell cell in removedCells)
			targetList.Remove(cell);
			
		return targetList;
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		WasteResoursesAndEndTurn(unit);
		Action(unit, target);

		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
		yield return new WaitForSeconds(actionTime);
		
		unit.inAction = false;
	}
	
	private void Action(Unit unit, GroundCell target)
	{
		int currentValue = 0;
		
		Unit targetedUnit = target.onCellObject as Unit;
		
		if (actionType == ActionType.Offensive)
		{
			currentValue = GetTransitedValue(targetedUnit, unit);
			targetedUnit.WasteMana(currentValue);
			unit.RestoreMana(currentValue);
		}
		
		else
		{
			currentValue = GetTransitedValue(unit, targetedUnit);
			unit.WasteMana(currentValue);
			targetedUnit.RestoreMana(currentValue);
		}
	}
	
	private int GetTransitedValue(Unit anod, Unit catod)
	{
		int currentValue = _maxValue;
		
		if (anod.currentMana < currentValue)
			currentValue = anod.currentMana;
		
		if (catod.currentStats.maxMana - catod.currentMana < currentValue)
			currentValue = catod.currentStats.maxMana - catod.currentMana;
			
		return currentValue;
	}
	
	protected override string GetDamageString(ActionData actionData)
	{
		return UISettings.Give + maxValue + UISettings.mana;
	}
	
	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		if (actionType == ActionType.Offensive)
			return GetTransitedValue(cell.onCellObject as Unit, unit).ToString();
		else
			return GetTransitedValue(unit, cell.onCellObject as Unit).ToString();
	}
}
