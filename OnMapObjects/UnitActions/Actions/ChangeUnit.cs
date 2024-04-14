using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUnit : AbstractAction
{
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	
	[SerializeField] private Unit _changetUnit;
	public Unit changetUnit => _changetUnit;
	
	[SerializeField] private Unit _newUnit;
	public Unit newUnit => _newUnit;
	
	public override bool endedTurnAction => true;
	public override bool damageFromUnitStats => false;
	public override bool rangeFromUnitStats => false;
	
	public override ActionType actionType => ActionType.Defensive;
	public override ActionRange range => ActionRange.Melee;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetChangeUnitDistance(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		WasteResoursesAndEndTurn(unit);
		
		target.onCellObject.Death();
		
		Unit createdUnit = Instantiate(_newUnit, unit.map.objectMap.transform);
		createdUnit.Init(target, unit.player);
		
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
		
		createdUnit.EndTurn();
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
}
