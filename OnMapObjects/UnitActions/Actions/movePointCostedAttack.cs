using UnityEngine;

public class movePointCostedAttack : Attack
{
	public override bool endedTurnAction => false;
	
	[SerializeField] private int _movePointCost;
	
	protected override void EndAction(Unit unit)
	{
		unit.currentMovePoints -= _movePointCost;
		
		if (unit.currentMovePoints <= 0)
		{
			unit.EndTurn();
			unit.currentMovePoints = 0;
		}
		
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
	}
}
