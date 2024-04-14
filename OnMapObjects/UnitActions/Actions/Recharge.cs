using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recharge : AbstractAction
{
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	
	public override bool endedTurnAction => false;
	public override bool damageFromUnitStats => false;
	public override bool rangeFromUnitStats => false;
	
	public override ActionType actionType => ActionType.Defensive;
	public override ActionRange range => ActionRange.OnCaster;
	
	[SerializeField] private int _movePointCost;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetCasterPosition(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		PlaySound(castSound);
		
		unit.inAction = true;
		WasteResoursesAndEndTurn(unit);
		
		unit.attackIsRecharget = true;
		
		unit.currentMovePoints -= _movePointCost;

		IconSetter.setEffects.Invoke(unit);
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
	
	protected override bool CurrentActivateCheck(Unit unit)
	{
		if (unit.attackIsRecharget || unit.currentMovePoints < _movePointCost)
			return false;
			
		else	
			return true;
	}
}
