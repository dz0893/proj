using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonate : AbstractAction, IAreaAction
{
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	
	public override bool endedTurnAction => true;
	public override bool damageFromUnitStats => false;
	public override bool rangeFromUnitStats => false;
	
	public override ActionType actionType => ActionType.Defensive;
	public override ActionRange range => ActionRange.OnCaster;
	
	[SerializeField] private DamageType _damageType;
	public DamageType damageType => _damageType;
	
	public int area => 1;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetCasterPosition(unit);
	}
	
	public List<GroundCell> GetAreaDistance(GroundCell position)
	{
		return position.closestCellList;
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		unit.Death();
		
		foreach (GroundCell cell in GetAreaDistance(target))
			DealDamage(unit, cell);
		
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
	
	private void DealDamage(Unit caster, GroundCell cell)
	{
		if (cell.onCellObject != null)
			cell.onCellObject.GetAttack(caster.GetActionData(this).damage, damageType);
	}
	
	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		return actionTextGetter.GetDamageText(cell.onCellObject, unit.GetActionData(this).damage, damageType);
	}
}
