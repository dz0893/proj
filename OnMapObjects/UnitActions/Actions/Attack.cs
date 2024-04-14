using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : AbstractAction, IDamage
{
	protected EnemyDistanceFinder enemyDistanceFinder = new EnemyDistanceFinder();
	
	public override bool endedTurnAction => true;
	
	public override ActionType actionType => ActionType.Offensive;
	
	[SerializeField] protected ActionRange _range;
	public override ActionRange range => _range; 
	
	[SerializeField] private DamageType _damageType;
	public DamageType damageType => _damageType;
	
	[SerializeField] private bool _rangeFromUnitStats = true;
	public override bool rangeFromUnitStats => _rangeFromUnitStats;

	[SerializeField] private bool _damageFromUnitStats = true;
	public override bool damageFromUnitStats => _damageFromUnitStats;

	[SerializeField] private bool _counterAttackFree;
	public bool counterAttackFree => _counterAttackFree;
	
	[SerializeField] protected bool _onCasterEffect;

	public virtual List<DamageType> damageTypeList
	{
		get
		{
			List<DamageType> list = new List<DamageType>();
			list.Add(damageType);
			return list;
		}
	}
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return attackDistanceFinder.GetAttackDistance(unit);
	}
	
	public override GroundCell GetAITarget(Unit unit)
	{
		return enemyDistanceFinder.GetFighterTarget(unit, this);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		MaterialObject targetedObject = target.onCellObject;

		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
		while (AnimationController.flyAnimationIsActive)
			yield return null;

		MakeAttack(unit, target, false);
		
		if (target.onCellObject is Unit)
		{
			Unit targetUnit = target.onCellObject as Unit;
		
			if (!targetUnit.CheckDamageForLetality(unit.GetActionData(this).damage, damageType) && targetUnit.counterAttack != null 
			&& !_counterAttackFree)
				actionTime = ActionSettings.ATTACKTIME * 2;
			else
				actionTime = ActionSettings.ATTACKTIME;
		}
		else
			actionTime = ActionSettings.ATTACKTIME;
		
		yield return new WaitForSeconds(ActionSettings.ATTACKTIME);
		
		WasteResoursesAndEndTurn(unit);
		
		if (_range == ActionRange.Melee && target.onCellObject is Unit)
		{
			Unit targetUnit = target.onCellObject as Unit;
			
			if (!targetUnit.isDead && targetUnit.counterAttack != null && !_counterAttackFree && targetUnit == targetedObject)
			{
				if (unit.transform.position.x < targetUnit.transform.position.x)
					targetUnit.RotateToLeft();
				else if (unit.transform.position.x > targetUnit.transform.position.x)
					targetUnit.RotateToRight();
				
				Attack counterAttack = targetUnit.counterAttack as Attack;

				AnimationController.play.Invoke(counterAttack, targetUnit.position, unit.position, targetUnit.spriteFlipped);
				targetUnit.PlayActionCastSound(counterAttack);
				counterAttack.MakeAttack(targetUnit, unit.position, true);

				yield return new WaitForSeconds(ActionSettings.ATTACKTIME);
			}
		}
		
		unit.inAction = false;
		yield return null;
	}
	
	public void MakeAttack(Unit unit, GroundCell target, bool isCounterAttack)
	{
		int damage = unit.GetActionData(this).damage;

		if (isCounterAttack)
			damage -= unit.counterAttackModifier;
		
		ChildAction(unit, target);
		AttackAction(unit, target, damage);
	}
	
	protected virtual void AttackAction(Unit unit, GroundCell target, int damage)
	{
		if (_actionEffect != null)
		{
			if (_onCasterEffect)
				OnAttackEffect(unit, unit);
				
			else if (target.onCellObject is Unit)
			{
				Unit targetedUnit = target.onCellObject as Unit;
				
				OnAttackEffect(unit, targetedUnit);
			}
		}
		
		int exp = target.onCellObject.GetAttack(damage, GetBestDamageType(unit, target));
		
		unit.player.AddExpToUnits(exp);
	}
	
	protected virtual void ChildAction(Unit unit, GroundCell target) {}
	
	protected void OnAttackEffect(Unit caster, Unit target)
	{
		_actionEffect.Activate(target);
	}
	
	public virtual DamageType GetBestDamageType(Unit unit, GroundCell target)
	{
		return _damageType;
	}
	
	protected override string GetDamageString(ActionData actionData)
	{
		string damageString;
		
		damageString = UISettings.damage + actionData.damage + " ";
		
		for (int i = 0; i < damageTypeList.Count; i++)
		{
			damageString += UISettings.GetDamageTypeName(damageTypeList[i]);
			
			if (i != damageTypeList.Count - 1)
				damageString += "/";
		}
		
		return damageString;
	}

	protected override void SetCurrentDescription(Unit unit)
	{
		SetDamageModifierDescription(unit);
		SetChildDescription(unit);
	}

	protected void SetDamageModifierDescription(Unit unit)
	{
		if (unit.counterAttackModifier != 0)
		{
			if (range == ActionRange.Melee && this == unit.counterAttack)
				AddStringToRequiresList(UISettings.CounterAttackModifier + unit.counterAttackModifier);
			else if (range == ActionRange.Ranged)
				AddStringToRequiresList(UISettings.RangedAttackOnMovingModifier + unit.counterAttackModifier);
		}
	}

	protected virtual void SetChildDescription(Unit unit) {}
	
	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		return actionTextGetter.GetDamageText(cell.onCellObject, unit.GetActionData(this).damage, GetBestDamageType(unit, cell));
	}
}
