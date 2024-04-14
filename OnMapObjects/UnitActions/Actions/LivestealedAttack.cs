using UnityEngine;

public class LivestealedAttack : Attack
{
	private DamageGetter damageGetter = new DamageGetter();
	
	[SerializeField] private AudioClip _transformateSound;

	[SerializeField] private float _healthStealingModifier;
	[SerializeField] private NullObject _createdObject;
	
	[SerializeField] private bool _isManaRestoredAttack;
	[SerializeField] private int _defaultRestoringManaValue;
	[SerializeField] private int _onKillRestoringManaValue;
	
	protected override void ChildAction(Unit unit, GroundCell target)
	{
		if (target.onCellObject.isMech)
			return;
		
		int damageValue = damageGetter.GetDamage(target.onCellObject, unit.GetActionData(this).damage, GetBestDamageType(unit, target));
		
		unit.RestoreHealth((int)(damageValue * _healthStealingModifier));
		
		if (_isManaRestoredAttack)
		{
			if (damageValue >= target.onCellObject.currentHealth)
				unit.RestoreMana(_onKillRestoringManaValue);
			else
				unit.RestoreMana(_defaultRestoringManaValue);
		}
	}
	
	public override GroundCell GetAITarget(Unit unit)
	{
		if (_createdObject != null)
			return enemyDistanceFinder.GetCreatedObjectAttackTarget(unit, this);
		else
			return enemyDistanceFinder.GetFighterTarget(unit, this);
	}
	
	protected override void SetCurrentDescription(Unit unit)
	{
		if (_healthStealingModifier > 0)
			AddStringToRequiresList(UISettings.Livesteal + (_healthStealingModifier * 100) + "%");
		if (_isManaRestoredAttack)
		{
			AddStringToRequiresList(UISettings.Restore + _defaultRestoringManaValue + UISettings.mana + UISettings.perHit);
			AddStringToRequiresList(UISettings.Or + _onKillRestoringManaValue + UISettings.mana + UISettings.perKill);
		}
	}
	
	protected override void AttackAction(Unit unit, GroundCell target, int damage)
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
		
		IActionEffectTarget targetedObject = target.onCellObject;
		
		int exp = target.onCellObject.GetAttack(damage, GetBestDamageType(unit, target));
		
		unit.player.AddExpToUnits(exp);
		
		if (targetedObject != null && CheckObjectForCreating(targetedObject) && targetedObject.isDead)
			CreateObject(unit, target, targetedObject as Unit);
	}
	
	private void CreateObject(Unit caster, GroundCell target, Unit targetedObject)
	{
		if (target.grave.Contains(targetedObject))
		{
			AudioManager.playSound.Invoke(afterCastSoundDelay, _transformateSound);

			target.RemoveUnitFromGrave(targetedObject);
			BattleMap.initObject.Invoke(_createdObject, caster.player, target);
		}
	}
	
	public bool CheckObjectForCreating(IActionEffectTarget targetedObject)
	{
		if (targetedObject is Unit && targetedObject.goingToGraveAtDeath && !targetedObject.isMech && _createdObject != null)
			return true;
		else
			return false;
	}
}
