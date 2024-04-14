using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanNegaiveEffects : ActionEffect
{
	public override void LocalActivate(Unit target, CurrentEffect effect)
	{
		for (int i = 0; i < target.activeEffectList.Count; i++) 
		{
			if (target.activeEffectList[i].isNegative)
			{
				target.activeEffectList[i].Clean();
				i--;
			}
		}
	}
}
