using System.Collections.Generic;
using UnityEngine;

public class ChangeUnitStats : ActionEffect, IStatChanger
{
	public override string Name => UISettings.changeStats;
	
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _maxMana;
	[SerializeField] private int _damage;
	[SerializeField] private int _physicalDefence;
	[SerializeField] private int _piercingDefence;
	[SerializeField] private int _magicDefence;
	[SerializeField] private int _sturmDefence;
	[SerializeField] private int _maxMovePoints;
	[SerializeField] private int _expForDestroing;
	[SerializeField] private int _attackRange;
	[SerializeField] private int _minAttackRange;
	[SerializeField] private int _restoreManaPerMeditate;
	[SerializeField] private int _healthRegen;
	[SerializeField] private int _manaRegen;
	
	public override EffectType effectType
	{
		get
		{
			if (isNegative)
				return EffectType.DecreaseStats;
			else
				return EffectType.IncreaseStats;
		}
	}

	public UnitStats stats
	{
		get
		{
			return new UnitStats(_maxHealth, _maxMana, _damage, _physicalDefence, _piercingDefence, _magicDefence,
			_sturmDefence, _maxMovePoints, _expForDestroing, _attackRange, _minAttackRange, _restoreManaPerMeditate,
			_healthRegen, _manaRegen);
		}
	}
	
	public override void LocalClean(Unit target, CurrentEffect effect)
	{
		target.RemoveStatChanger(this);
	}
	
	public override void LocalActivate(Unit target, CurrentEffect effect)
	{
		target.AddStatChanger(this);
	}
	
	public override List<string> GetDescription()
	{
		List<string> description = new List<string>();
		
		if (_maxHealth != 0)
			description.Add(UISettings.health + UISettings.GetSignedValue(_maxHealth));
		if (_maxMana != 0)
			description.Add(UISettings.Mana + UISettings.GetSignedValue(_maxMana));
		if (_damage != 0)
			description.Add(UISettings.damage + UISettings.GetSignedValue(_damage));
		if (_physicalDefence != 0)
			description.Add(UISettings.PhysicalDefence + UISettings.GetSignedValue(_physicalDefence));
		if (_piercingDefence != 0)
			description.Add(UISettings.PiercingDefence + UISettings.GetSignedValue(_piercingDefence));
		if (_magicDefence != 0)
			description.Add(UISettings.MagicDefence + UISettings.GetSignedValue(_magicDefence));
		if (_sturmDefence != 0)
			description.Add(UISettings.SiegeDefence + UISettings.GetSignedValue(_sturmDefence));
		if (_maxMovePoints != 0)
			description.Add(UISettings.movePoints + UISettings.GetSignedValue(_maxMovePoints));
		if (_attackRange != 0)
			description.Add(UISettings.rangeOfAttack + UISettings.GetSignedValue(_attackRange));
		if (_healthRegen != 0)
			description.Add(UISettings.healthRegen + UISettings.GetSignedValue(_healthRegen));
		if (_manaRegen != 0)
			description.Add(UISettings.manaRegen + UISettings.GetSignedValue(_manaRegen));
		
		if (liveTime != 0)
			description.Add(UISettings.LiveTime + liveTime + UISettings.turns);
		
		return description;		
	}
}
