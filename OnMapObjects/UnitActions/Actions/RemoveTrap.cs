using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveTrap : AbstractAction
{
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	
	public override bool endedTurnAction => true;
	public override bool damageFromUnitStats => false;
	public override bool rangeFromUnitStats => false;
	
	public override ActionType actionType => ActionType.Offensive;
	public override ActionRange range => ActionRange.Melee;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetEnemyTrapsDistance(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		WasteResoursesAndEndTurn(unit);
		
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);

		target.unmaterialOnCellObject.Death();
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
}
