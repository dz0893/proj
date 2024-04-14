using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnitBehavior : AIObjectBehavior
{
	private EnemyDistanceFinder enemyDistanceFinder = new EnemyDistanceFinder();
	private MineDistanceFinder mineDistanceFinder = new MineDistanceFinder();
	private MovingDistanceFinder movingDistanceFinder = new MovingDistanceFinder();
	
	public GroundCell moveTarget { get; private set; }
	public GroundCell actionTarget { get; private set; }
	
	public bool CheckForSecondAction(NullObject obj)
	{
		bool needSecondAction = false;

		if (obj is Unit)
		{
			Unit unit = obj as Unit;
			
			if (unit.choosenAction == null || unit.turnEnded)
			{
				needSecondAction = false;
			}
			
			else if (!unit.turnEnded && !unit.isDead)
			{
				unit.choosenAction = GetUnitAction(unit);
				
				if (unit.choosenAction != null)
				{
					unit.SetRangeOfAction();
					
					if (unit.totalActionTargetList.Count != 0 && unit.choosenAction.GetAITarget(unit) != null)
					{
						needSecondAction = true;
					}
				}
			}
		}

		return needSecondAction;
	}
	
	public override void ActivateAction(NullObject obj)
	{
		Unit unit = obj as Unit;
		
		actionTarget = null;
		
		unit.choosenAction = GetUnitAction(unit);
		
	}
	
	private AbstractAction GetUnitAction(Unit unit)
	{
		AbstractAction action;
		actionTarget = null;
		
		if (unit.aiGoal != null)
			action = GetGoalAction(unit);
		
		else if (unit.unitType == UnitType.Builder)
			action = GetBuilderAction(unit);
		
		else
			action = GetLineUnitAction(unit);
		
		return action;
	}
	
	private AbstractAction GetBuilderAction(Unit unit)
	{
		AbstractAction abstractAction = null;
		
		abstractAction = InitUnitAction(unit.defaultAIAction, unit);
		
		if (unit.currentActionTargetList.Count == 0)
			abstractAction = InitUnitAction(unit.moveAction, unit);
		
		if (actionTarget == null)
			return GetLineUnitAction(unit);
		else
			return abstractAction;
	}
	
	private AbstractAction GetLineUnitAction(Unit unit)
	{
		AbstractAction abstractAction = null;
		AbstractAction lastNotNullAction = null;
		
		foreach (AbstractAction action in unit.actionList)
		{
			if (action.ChekActionForActive(unit) && action != unit.moveAction && action.aiCanUse)
			{
				abstractAction = InitUnitAction(action, unit);

				if (abstractAction.GetAITarget(unit) != null)
					lastNotNullAction = abstractAction;
				
				if (unit.currentActionTargetList.Count != 0 && unit.unitType != UnitType.Mage)
					break;
			}
		}
		
		if (abstractAction != null && unit.currentMovePoints != 0 
		&& (unit.currentActionTargetList.Count == 0 || actionTarget == null))
		{
			abstractAction = InitUnitAction(unit.moveAction, unit);
		}
		
		if (abstractAction == unit.moveAction && lastNotNullAction != null)
		{
			abstractAction = InitUnitAction(lastNotNullAction, unit);
		}
		
		if (abstractAction == null && unit.currentMovePoints > 0 && unit.unitType == UnitType.Archer)
			abstractAction = InitUnitAction(unit.moveAction, unit);

		return abstractAction;
	}
	
	private AbstractAction InitUnitAction(AbstractAction action, Unit unit)
	{
		unit.choosenAction = action;
		unit.SetRangeOfAction();
		actionTarget = unit.choosenAction.GetAITarget(unit);
		
		return action;
	}
	
	private AbstractAction GetGoalAction(Unit unit)
	{
		AbstractAction abstractAction = null;
		
		abstractAction = InitUnitAction(unit.aiGoal.GetAction(unit), unit);
		
		return abstractAction;
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
}
