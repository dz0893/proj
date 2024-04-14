using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveTime : ActionEffect
{
	public override EffectType effectType => EffectType.LiveTime;

	[SerializeField] private int _onHeroDamage;
	
	public override void LocalClean(Unit target, CurrentEffect effect)
	{
		if (target.player.hero != target)
			target.Death();
		else
		{
			target.EndTurn();
			target.GetAttack(_onHeroDamage, DamageType.True);
		}
	}
}
