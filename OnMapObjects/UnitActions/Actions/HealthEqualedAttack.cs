using UnityEngine;

public class HealthEqualedAttack : Attack
{
    [SerializeField] private float _damagePerHealthPercent;

	public override int GetCurrentActionDamageModifire(Unit unit)
	{
		return (int)((unit.currentStats.maxHealth - unit.currentHealth) * _damagePerHealthPercent);
	}
}
