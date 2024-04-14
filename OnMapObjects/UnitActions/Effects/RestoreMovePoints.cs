using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreMovePoints : ActionEffect
{
	[SerializeField] private int _value;
	[SerializeField] private bool _activateTurnEndedUnit;
	
	public override void LocalActivate(Unit target, CurrentEffect effect)
	{
		if(_activateTurnEndedUnit)
			target.turnEnded = false;
			
		target.currentMovePoints += _value;
		
		if (target.currentMovePoints > target.currentStats.maxMovePoints)
			target.currentMovePoints = target.currentStats.maxMovePoints;
	}
}
