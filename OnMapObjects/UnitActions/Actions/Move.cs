using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : AbstractAction
{
	public MovingDistanceFinder movingDistanceFinder { get; private set; } = new MovingDistanceFinder();
	private MineDistanceFinder mineDistanceFinder = new MineDistanceFinder();
	private EnemyDistanceFinder enemyDistanceFinder = new EnemyDistanceFinder();
	
	public int moveCost { get; set; }
	
	public override bool endedTurnAction => false;
	
	public override ActionType actionType => ActionType.Moving;
	public override ActionRange range => ActionRange.Melee;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return movingDistanceFinder.GetMoveDistance(unit);
	}
	
	public override GroundCell GetAITarget(Unit unit)
	{
		if (unit.aiGoal != null)
		{
		//	if (unit.index == 2106)
		//		Debug.Log("1");
			return enemyDistanceFinder.GetClosestToGoalPosition(unit);
		}
		else if (unit.unitType == UnitType.Builder)
		{
		//	if (unit.index == 2106)
		//		Debug.Log("2");
			return mineDistanceFinder.GetClosestToDepositCell(unit);
		}
		else if (unit.unitType == UnitType.Fighter)
		{
		//	if (unit.index == 2106)
		//		Debug.Log("3");
			return enemyDistanceFinder.GetClosestMovePosition(unit, unit.moveActionTargetList);
		}
		else
		{
		//	if (unit.index == 2106)
		//		Debug.Log("4");
			return enemyDistanceFinder.GetRangetUnitMovePosition(unit, GetEnemyPlayer(unit.player));
		}
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		actionTime = unit.road.Count * ActionSettings.MOVETIME + 0.1f;
		
		foreach (GroundCell cell in unit.road)
		{
			ActionController.instance.OneCellMoving(unit, cell, ActionSettings.MOVETIME);
			
			unit.currentMovePoints -= movingDistanceFinder.GetMoveCostToCell(cell, unit);
			
			while (unit.position != cell)
				yield return null;
			
			if (cell.unmaterialOnCellObject != null)
				cell.unmaterialOnCellObject.ContactWithOtherObject(unit);
		}
		
		if (unit.currentMovePoints < 0)
			unit.currentMovePoints = 0;
			
		yield return new WaitForSeconds(0.1f);
		WasteResoursesAndEndTurn(unit);
		unit.inAction = false;
		yield return null;
	}
	
	private Player GetEnemyPlayer(Player player)
	{
		Player enemy = null;
		
		foreach (Player currentPlayer in BattleMap.instance.playerList)
		{
			if (currentPlayer != player && currentPlayer.team != player.team)
				enemy = currentPlayer;
		}
		
		return enemy;
	}
	
	public virtual List<GroundCell> GetRoadToCell(Unit unit, GroundCell goal)
	{
		return movingDistanceFinder.GetRoadTo(unit, goal);
	}
	
	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		return cell.totalMovingCost.ToString();
	}
}
