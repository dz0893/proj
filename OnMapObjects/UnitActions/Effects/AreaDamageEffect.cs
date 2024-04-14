using System.Collections.Generic;
using UnityEngine;

public class AreaDamageEffect : ActionEffect
{
	[SerializeField] private int _damage;
	public int damage => _damage;
	
	[SerializeField] private DamageType _damageType;
	public DamageType damageType => _damageType;
	
	public override void LocalActivate(Unit target, CurrentEffect effect)
	{
		foreach (GroundCell cell in target.position.closestCellList)
			DealDamageToObjectOnCell(cell);
	}
	
	private void DealDamageToObjectOnCell(GroundCell cell)
	{
		if (cell.onCellObject != null)
			cell.onCellObject.GetAttack(_damage, _damageType);
	}
}
