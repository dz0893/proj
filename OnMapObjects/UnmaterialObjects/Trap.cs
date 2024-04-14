using UnityEngine;

public class Trap : UnmaterialObject
{
	[SerializeField] protected GlobalActionObject _actionObject;
	[SerializeField] protected int _damage;
	[SerializeField] protected DamageType _damageType;
	[SerializeField] protected bool _destroyedAfterActivate = true;
	
	protected GlobalAction action;

	protected override void LocalInit(GroundCell positionCell)
	{
		InitInfo(new UnmaterialObjectInfo());
		turnEnded = true;

		if (_actionObject != null)
			action = _actionObject.GetGlobalAction();
	}
	
	public override void ContactWithOtherObject(NullObject obj)
	{
		if (obj is Unit)
		{
			if (obj.team != team)
				Detonate(obj as Unit);
		}
		else
			Death();
	}
	
	private void Detonate(Unit unit)
	{
		if (_damage > 0)
			unit.GetAttack(_damage, _damageType);
		
		if (action != null)
			action.Activate(player, position);
		
		if (_destroyedAfterActivate)
			Death();
	}
}
