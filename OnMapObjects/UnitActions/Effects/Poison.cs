using System.Collections.Generic;
using UnityEngine;

public class Poison : ActionEffect
{
	public override EffectType effectType => EffectType.Poison;

	[SerializeField] private int _damage;
	public int damage => _damage;
	
	[SerializeField] private DamageType _damageType;
	public DamageType damageType => _damageType;
	
	[SerializeField] private bool _canKillEnemy;
	
	public override void Dot(Unit target, CurrentEffect effect)
	{
		if (!target.CheckDamageForLetality(_damage, _damageType) || _canKillEnemy)
			target.GetAttack(_damage, _damageType);
		else if (target.currentHealth != 1)
			target.GetAttack((target.currentHealth - 1), DamageType.True);
	}
	
	public override List<string> GetDescription()
	{
		List<string> description = new List<string>();
		
		description.Add(UISettings.damage + damage + " " + UISettings.GetDamageTypeName(damageType));
		
		if (liveTime != 0)
			description.Add(UISettings.LiveTime + liveTime + UISettings.turns);
		
		return description;
	}
}
