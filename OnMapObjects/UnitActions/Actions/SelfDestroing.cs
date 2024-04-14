using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroing : AbstractAction, IAreaAction
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
	
	[SerializeField] private bool _isRemovingWithoutDeath;
	[SerializeField] private bool _dealDamageInArea;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetCasterPosition(unit);
	}
	
	public List<GroundCell> GetAreaDistance(GroundCell position)
	{
		List<GroundCell> areaList = new List<GroundCell>();
		
		if (_dealDamageInArea)
			areaList = position.closestCellList;
		else
			areaList.Add(position);
		
		return areaList;
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		PlaySound(castSound);

		WasteResoursesAndEndTurn(unit);

		if (_isRemovingWithoutDeath)
			unit.RemoveFromGame();
		else
			unit.Death();
		
		if (_dealDamageInArea)
		{
			foreach (GroundCell cell in GetAreaDistance(target))
				DealDamage(unit, cell);
		}
		
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
