using System.Collections.Generic;

public class ManaCostedAttack : Attack
{
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return attackDistanceFinder.GetRangedAttackTargetCells(unit, unit.GetActionData(this).attackRange);
	}
}
