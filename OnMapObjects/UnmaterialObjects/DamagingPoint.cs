using UnityEngine;

public class DamagingPoint : Trap
{
	[SerializeField] private bool _effectedAtMechs;
	
	public override void ContactWithOtherObject(NullObject obj)
	{
		DealDamage(obj);
	}
	
	public override void StartTurn()
	{
		if (position.onCellObject != null)
			DealDamage(position.onCellObject);
	}
	
	private void DealDamage(NullObject obj)
	{
		if (obj is Unit && obj.team != team)
		{
			Unit unit = obj as Unit;
			
			if (!unit.isMech || _effectedAtMechs)
				unit.GetAttack(_damage, _damageType);
		}
	}
}
