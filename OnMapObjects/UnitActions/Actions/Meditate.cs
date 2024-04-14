using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meditate : AbstractAction
{
	public ActionDistanceFinder actionDistanceFinder { get; private set; } = new ActionDistanceFinder();
	
	public override bool endedTurnAction => true;
	public override bool damageFromUnitStats => true;
	public override bool rangeFromUnitStats => false;
	
	public override ActionType actionType => ActionType.Defensive;
	
	[SerializeField] private ActionRange _range;
	public override ActionRange range => _range;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetCasterPosition(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		AnimationController.play.Invoke(this, unit.position, unit.position, unit.spriteFlipped);
		unit.inAction = true;
		unit.RestoreMana(GetRestoredValue(unit));
		
		WasteResoursesAndEndTurn(unit);
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
	
	private int GetRestoredValue(Unit unit)
	{
		if (unit.currentMovePoints >= unit.currentStats.maxMovePoints)
			return unit.currentStats.restoreManaPerMeditate;
		else
			return unit.currentStats.restoreManaPerMeditate / 2;
	}
	
	protected override bool CurrentActivateCheck(Unit unit)
	{
		if (unit.currentMana < unit.currentStats.maxMana)
			return true;
		else
			return false;
	}
	
	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		return actionTextGetter.GetRestoreManaText(unit, GetRestoredValue(unit));
	}
}
