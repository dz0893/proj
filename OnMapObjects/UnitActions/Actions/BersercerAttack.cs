using System.Collections;
using UnityEngine;

public class BersercerAttack : Attack
{
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		MaterialObject targetedObject = target.onCellObject;
		
		while (AnimationController.flyAnimationIsActive)
			yield return null;

		MakeAttack(unit, target, false);
		
		if (target.onCellObject is Unit)
		{
			Unit targetUnit = target.onCellObject as Unit;
		
			if (!targetUnit.CheckDamageForLetality(unit.GetActionData(this).damage, damageType) && targetUnit.counterAttack != null 
			&& !counterAttackFree)
				actionTime = ActionSettings.ATTACKTIME * 2;
			else
				actionTime = ActionSettings.ATTACKTIME;
		}
		else
			actionTime = ActionSettings.ATTACKTIME;
		
		yield return new WaitForSeconds(ActionSettings.ATTACKTIME);
		
		WasteResoursesAndEndTurn(unit);
		
		if (range == ActionRange.Melee && target.onCellObject is Unit)
		{
			Unit targetUnit = target.onCellObject as Unit;
			
			if (!targetUnit.isDead && targetUnit.counterAttack != null && !counterAttackFree && targetUnit == targetedObject)
			{
				if (unit.transform.position.x < targetUnit.transform.position.x)
					targetUnit.RotateToLeft();
				else if (unit.transform.position.x > targetUnit.transform.position.x)
					targetUnit.RotateToRight();
				
				Attack counterAttack = targetUnit.counterAttack as Attack;

				AnimationController.play.Invoke(counterAttack, targetUnit.position, unit.position, targetUnit.spriteFlipped);
				targetUnit.PlayActionCastSound(counterAttack);
				counterAttack.MakeAttack(targetUnit, unit.position, true);
				
				yield return new WaitForSeconds(ActionSettings.ATTACKTIME);

				if (!unit.isDead)
					unit.turnEnded = false;
			}
		}
		
		unit.inAction = false;
		yield return null;
	}
	
	protected override void SetCurrentDescription(Unit unit)
	{
		AddStringToRequiresList(UISettings.BersercerAttackRestoringDescription);
	}
}
