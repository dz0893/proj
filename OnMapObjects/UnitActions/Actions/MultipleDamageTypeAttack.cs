using System.Collections.Generic;
using UnityEngine;

public class MultipleDamageTypeAttack : Attack
{
	private DamageGetter damageGetter = new DamageGetter();
	
	[SerializeField] private List<DamageType> _damageTypeList;
	public override List<DamageType> damageTypeList => _damageTypeList;
	
	public override DamageType GetBestDamageType(Unit unit, GroundCell target)
	{
		DamageType bestDamageType = _damageTypeList[0];
		
		int bestDamage = damageGetter.GetDamage(target.onCellObject, unit.GetActionData(this).damage, _damageTypeList[0]);
		
		foreach (DamageType damageType in _damageTypeList)
		{
			int currentDamage = damageGetter.GetDamage(target.onCellObject, unit.GetActionData(this).damage, damageType);
			
			if (currentDamage > bestDamage)
			{
				bestDamage = currentDamage;
				bestDamageType = damageType;
			}
		}
		
		return bestDamageType;
	}
}
