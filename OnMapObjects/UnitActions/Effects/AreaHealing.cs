using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaHealing : ActionEffect
{
	private AttackDistanceFinder attackDistanceFinder = new AttackDistanceFinder();
	
	[SerializeField] private int _healingValue;
	
	[SerializeField] private int _range;
	
	private List<GroundCell> distance;
	
	public override void LocalActivate(Unit target, CurrentEffect effect)
	{
		distance = attackDistanceFinder.GetRangedAttackDistance(target.position, _range);
		distance.Add(target.position);
		
		foreach (GroundCell cell in distance)
		{
			MaterialObject obj = cell.onCellObject;
			
			if (obj != null && obj is Unit && !obj.isMech && obj.team == TurnController.currentPlayer.team)
				obj.RestoreHealth(_healingValue);
		}
	}
}
