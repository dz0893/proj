using UnityEngine;

public class RechargeUnitAttackPoint : UnmaterialObject
{
	private MaterialObject obj => position.onCellObject;
	
	public override void StartTurn()
	{
		if (obj != null && obj is Unit && obj.player == player)
		{
			Unit unit = obj as Unit;
			unit.attackIsRecharget = true;
		}
	}
}
