using UnityEngine;

public class GlobalDamageObject : GlobalActionObject
{
	[SerializeField] private DamageType _damageType;
	public DamageType damageType => _damageType;
	
	[SerializeField] private bool _onMechs;
	public bool onMechs => _onMechs;
	
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalDamage(this);
	}
}
