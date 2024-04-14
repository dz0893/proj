using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatBody : AbstractAction
{
	public ActionDistanceFinder actionDistanceFinder { get; private set; } = new ActionDistanceFinder();
	
	[SerializeField] private bool _damageFromUnitStats;
	public override bool damageFromUnitStats => _damageFromUnitStats;
	
	public override bool endedTurnAction => true;
	
	public override ActionType actionType => ActionType.OnDeadUnit;
	
	public override ActionRange range => ActionRange.Melee;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetFreeCellsWithGrave(unit);
	}

	public override GroundCell GetAITarget(Unit unit)
	{
		if (unit.currentActionTargetList.Count > 0 && unit.currentHealth < unit.currentStats.maxHealth)
			return unit.currentActionTargetList[0];
		else
			return null;
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		AudioManager.playSound.Invoke(afterCastSoundDelay, castSound);

		unit.inAction = true;
		target.RemoveUnitFromGrave(target.grave[0]);
		
		unit.RestoreHealth(unit.GetActionData(this).damage);
		
		yield return new WaitForSeconds(actionTime);
		
		WasteResoursesAndEndTurn(unit);
		unit.inAction = false;
	}
	
	protected override string GetDamageString(ActionData actionData)
	{
		return UISettings.heal + actionData.damage;
	}
	
	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		return actionTextGetter.GetHealingText(unit, unit.GetActionData(this).damage);
	}
}
