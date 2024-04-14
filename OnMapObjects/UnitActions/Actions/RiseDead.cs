using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseDead : AbstractAction//, IAreaAction
{
	public ActionDistanceFinder actionDistanceFinder { get; private set; } = new ActionDistanceFinder();
	
	[SerializeField] private bool _riseAsUndead;
	public bool riseAsUndead => _riseAsUndead;
	
	[SerializeField] private bool _onlyDefaultUndead;

	[SerializeField] private Unit _defaultUndeadUnit;
	[SerializeField] private Unit _warriorUndeadUnit;
	[SerializeField] private Unit _archerUndeadUnit;
	[SerializeField] private Unit _mageUndeadUnit;
	
	public override bool endedTurnAction => true;
	
	public override ActionType actionType => ActionType.OnDeadUnit;
	
	[SerializeField] private ActionRange _range;
	public override ActionRange range => _range;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetRevivingUnitDistance(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
		unit.inAction = true;
		Unit risedUnit = null;
		
		yield return new WaitForSeconds(actionTime);
		
		if (_riseAsUndead)
		{
			risedUnit = GetRisedUndead(target);
			
			if (target.grave.Count > 0)
				target.RemoveUnitFromGrave(target.grave[0]);
			
			BattleMap.initObject.Invoke(risedUnit, unit.player, target);
		}
		else
		{
			risedUnit = GetRevivedUnit(unit, target);
			target.RemoveUnitFromGrave(risedUnit);
			risedUnit.Revive(unit.player, target);
		}
		
		WasteResoursesAndEndTurn(unit);
		unit.inAction = false;
	}
	
	public Unit GetRisedUndead(GroundCell target)
	{
		Unit unit = _defaultUndeadUnit;
		
		if (target.grave.Count > 0 && !_onlyDefaultUndead)
		{
			unit = _warriorUndeadUnit;
			
			if (target.grave.Count > 0)
			{
				if (target.grave[0].unitType == UnitType.Mage)
					unit = _mageUndeadUnit;
				else if (target.grave[0].unitType == UnitType.Archer)
					unit = _archerUndeadUnit;
			}
		}
		
		return unit;
	}
	
	public Unit GetRevivedUnit(Unit caster, GroundCell target)
	{
		Unit revivedUnit = null;
		
		foreach (Unit unit in target.grave)
		{
			if (unit.team == caster.team)
			{
				if (revivedUnit == null)
					revivedUnit = unit;
				else if (revivedUnit.currentStats.maxHealth < unit.currentStats.maxHealth)
					revivedUnit = unit;
			}
		}
		
		return revivedUnit;
	}
	
	protected override bool CurrentActivateCheck(Unit unit)
	{
		if (unit.player.currentUnitLimit >= unit.player.maxUnitLimit)
			return false;
		else	
			return true;
	}
	
	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		if (_riseAsUndead)
			return GetRisedUndead(cell).Name;
		else
			return GetRevivedUnit(unit, cell).Name;
	}
}
