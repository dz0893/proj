using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInitedObjects : ActionEffect
{
	public override void LocalActivate(Unit target, CurrentEffect effect)
	{
		foreach (GroundCell cell in target.position.closestCellList)
		{
			if (cell.unmaterialOnCellObject != null && cell.unmaterialOnCellObject.initer == target)
				cell.unmaterialOnCellObject.Death();
		}
	}
}
