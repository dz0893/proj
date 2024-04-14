using UnityEngine;

public class RechargedAttack : MultipleDamageTypeAttack
{
	[SerializeField] private bool _fullMovePointsOnly;
	[SerializeField] private bool _noNeedToRecharge;
	[SerializeField] private bool _goingOnRechargeAfterAttack = true;
	
	protected override void ChildAction(Unit unit, GroundCell target)
	{
		if (_goingOnRechargeAfterAttack)
		{
			unit.attackIsRecharget = false;
			IconSetter.setEffects.Invoke(unit);
		}
	}
	
	protected override bool CurrentActivateCheck(Unit unit)
	{
		if (!unit.attackIsRecharget && !_noNeedToRecharge)
			return false;
		
		else if (unit.currentMovePoints < unit.currentStats.maxMovePoints && _fullMovePointsOnly)
			return false;
			
		else	
			return true;
	}

	protected override void SetCurrentDescription(Unit unit)
	{
		if (!_fullMovePointsOnly)
			SetDamageModifierDescription(unit);
			
		SetChildDescription(unit);
	}

	protected override void SetChildDescription(Unit unit)
	{
		if (_fullMovePointsOnly)
			AddStringToRequiresList(UISettings.NeedFullMovePoints);

		if (!_noNeedToRecharge)
		{
			if (unit.attackIsRecharget)
				AddStringToRequiresList(UISettings.IsRecharget);
				
			else
				AddStringToRequiresList(UISettings.NeedToReacharge);
		}
	}
}

