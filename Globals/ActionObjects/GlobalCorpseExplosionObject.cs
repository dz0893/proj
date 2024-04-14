using UnityEngine;

public class GlobalCorpseExplosionObject : GlobalActionObject
{
	[SerializeField] private DamageType _damageType;
	public DamageType damageType => _damageType;
	
	[SerializeField] private bool _dealDamageToClosestCells;
	public bool dealDamageToClosestCells => _dealDamageToClosestCells;
	
	[SerializeField] private float _closestCellsDamageModifier;
	public float closestCellsDamageModifier => _closestCellsDamageModifier;
	
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalCorpseExplosion(this);
	}
}
