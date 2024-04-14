using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : AbstractAction
{
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	private EnemyDistanceFinder enemyDistanceFinder = new EnemyDistanceFinder();
	
	public override bool endedTurnAction => true;

	[SerializeField] private bool _rangeFromUnitStats = true;
	public override bool rangeFromUnitStats => _rangeFromUnitStats;
	
	[SerializeField] private bool _damageFromUnitStats;
	public override bool damageFromUnitStats => _damageFromUnitStats;
	
	[SerializeField] private ActionRange _range;
	public override ActionRange range => _range;
	
	[SerializeField] private bool _isRepairing;
	public bool isRepairing => _isRepairing;
	
	public override ActionType actionType => ActionType.Defensive;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetHealingDistance(unit);
	}
	
	public override GroundCell GetAITarget(Unit unit)
	{
		return enemyDistanceFinder.GetHealTarget(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
		unit.inAction = true;
		WasteResoursesAndEndTurn(unit);
		
		target.onCellObject.RestoreHealth(unit.GetActionData(this).damage);
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
	
	protected override string GetDamageString(ActionData actionData)
	{
		return UISettings.heal + actionData.damage;
	}
	
	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		return actionTextGetter.GetHealingText(cell.onCellObject, unit.GetActionData(this).damage);
	}

	protected override bool CurrentActivateCheck(Unit unit)
	{
		if (range == ActionRange.OnCaster && unit.currentHealth >= unit.currentStats.maxHealth)
			return false;
		else
			return true;
	}
}
