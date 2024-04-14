using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEffectOnUnit : AbstractAction, IDamage
{
	protected ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	
	[SerializeField] private bool _endedTurnAction = true;
	public override bool endedTurnAction => _endedTurnAction;

	[SerializeField] private bool _rangeFromUnitStats;
	public override bool rangeFromUnitStats => _rangeFromUnitStats;
	
	[SerializeField] private bool _canBeUsedOnThemself = true;
	[SerializeField] private bool _damageFromUnitStats;
	public override bool damageFromUnitStats => _damageFromUnitStats;
	
	[SerializeField] private DamageType _damageType;
	
	[SerializeField] private ActionType _actionType;
	public override ActionType actionType => _actionType;
	
	[SerializeField] private ActionRange _range;
	public override ActionRange range => _range;
	
	public List<DamageType> damageTypeList
	{
		get
		{
			List<DamageType> list = new List<DamageType>();
			list.Add(_damageType);
			return list;
		}
	}
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		List<GroundCell> distance = new List<GroundCell>();
				
		if (range == ActionRange.OnCaster)
			distance = actionDistanceFinder.GetCasterPosition(unit);
		
		else
		{
			distance = actionDistanceFinder.GetOnUnitEffectDistance(unit);
			
			if (!_canBeUsedOnThemself)
			{
				while (distance.Contains(unit.position))
					distance.Remove(unit.position);
			}
		}
		
		return distance;
	}

	public override GroundCell GetAITarget(Unit unit)
	{
		GroundCell target = null;

		foreach (GroundCell cell in unit.currentActionTargetList)
		{
			if (!TargetHaveCurrentEffect(cell))
			{
				target = cell;
				break;
			}
		}

		return target;
	}
	
	protected bool TargetHaveCurrentEffect(GroundCell position)
	{
		if (position.onCellObject != null && position.onCellObject is Unit)
		{
			Unit unit = position.onCellObject as Unit;

			foreach(CurrentEffect effect in unit.activeEffectList)
			{
				if (effect.actionEffect == actionEffect)
					return true;
			}
		}

		return false;
	}

	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
	//	SetDamage(unit);
		
		if (unit.GetActionData(this).damage > 0)
			target.onCellObject.GetAttack(unit.GetActionData(this).damage, _damageType);
		
		SetEffect(target.onCellObject as Unit);
		
		WasteResoursesAndEndTurn(unit);
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
	
	protected virtual void SetEffect(Unit target)
	{
		_actionEffect.Activate(target);
	}
	
	protected override string GetDamageString(ActionData actionData)
	{
		string damageString = "";
		
		if (actionData.damage == 0)
			return damageString;
		
		damageString = UISettings.damage + actionData.damage + " ";
		
		for (int i = 0; i < damageTypeList.Count; i++)
		{
			damageString += UISettings.GetDamageTypeName(damageTypeList[i]);
			
			if (i != damageTypeList.Count - 1)
				damageString += "/";
		}
		
		return damageString;
	}
	
	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		if (unit.GetActionData(this).damage > 0)
			return actionTextGetter.GetDamageText(cell.onCellObject, unit.GetActionData(this).damage, _damageType);
		else
			return "";
	}
}
