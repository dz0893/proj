using System.Collections.Generic;
using UnityEngine;

public class EnemyDistanceFinder
{
	private AttackDistanceFinder attackDistanceFinder = new AttackDistanceFinder();
	private DamageGetter damageGetter = new DamageGetter();
	
	public GroundCell GetAreaBuffTarget(Unit unit)
	{
		GroundCell bestTarget = null;
		
		IAreaAction areaBuff = unit.choosenAction as IAreaAction;
		List<GroundCell> area = areaBuff.GetAreaDistance(unit.position);
		
		int countOfEnemyInArea = 0;
		int countOfAlliesInArea = 0;
		
		foreach (GroundCell cell in area)
		{
			if (cell.onCellObject != null)
			{
				if (cell.onCellObject.team != unit.team)
					countOfEnemyInArea++;
				else if (cell.onCellObject.player == unit.player && cell.onCellObject is Unit)
					countOfAlliesInArea++;
			}
		}
			
		if (countOfEnemyInArea > 0 && countOfAlliesInArea > 1 && unit.currentMovePoints == unit.currentStats.maxMovePoints)
			bestTarget = unit.currentActionTargetList[0];
		
		return bestTarget;
	}
	
	public GroundCell GetRoundAttackTarget(Unit unit)
	{
		GroundCell target = null;
		
		int countOfTargets = 0;
		
		foreach (GroundCell cell in unit.position.closestCellList)
		{
			if (cell.onCellObject != null)
			{
				if (cell.onCellObject.team != unit.team)
					countOfTargets++;
				else
					countOfTargets -= 2;
			}
		}
		
		if (countOfTargets > 1)
			target = unit.position;
		
		return target;
	}
	
	public GroundCell GetAreaActionTarget(Unit unit)
	{
		int countOfTargets = 0;
		int bestCountOfTarget = 0;
		GroundCell bestTarget = null;
		
		foreach (GroundCell cell in unit.currentActionTargetList)
		{
			countOfTargets = GetCountOfTargetsForAreaActionInCurrentCell(unit, cell);
			
			if (countOfTargets > bestCountOfTarget)
			{
				bestCountOfTarget = countOfTargets;
				bestTarget = cell;
			}
		}
		
		return bestTarget;
	}
	
	public GroundCell GetHealTarget(Unit unit)
	{
		GroundCell bestTarget = null;
		int highestDeltaHealth = 0;
		int deltaHealth = 0;
		
		foreach (GroundCell cell in unit.currentActionTargetList)
		{
			deltaHealth = cell.onCellObject.currentStats.maxHealth - cell.onCellObject.currentHealth;
			
			if (deltaHealth > highestDeltaHealth && deltaHealth > 3)
			{
				highestDeltaHealth = deltaHealth;
				bestTarget = cell;
			}
		}
		
		return bestTarget;
	}
	
	private int GetCountOfTargetsForAreaActionInCurrentCell(Unit unit, GroundCell target)
	{
		int countOfTargets = 0;
		
		countOfTargets = GetAreaActionTargetPrioritet(unit, target);
		
		foreach (GroundCell cell in target.closestCellList)
		{
			countOfTargets += GetAreaActionTargetPrioritet(unit, cell);
		}
		
		return countOfTargets;
	}
	
	private int GetAreaActionTargetPrioritet(Unit unit, GroundCell cell)
	{
		if (cell.onCellObject != null)
		{
			if (cell.onCellObject.team != unit.team && unit.choosenAction.actionType == ActionType.Offensive)
				return 1;
			else if (cell.onCellObject.team == unit.team && unit.choosenAction.actionType == ActionType.Defensive)
				return 1;
			else
				return -2;
		}
		else
			return 0;
	} 
	
	public GroundCell GetRangetUnitMovePosition(Unit unit, Player enemyPlayer)
	{
		foreach (GroundCell cell in unit.moveActionTargetList)
		{
			if (ChekCellForActiveTarget(cell, unit) && !CheckCellForNearEnemy(cell, unit))
				return cell;
		}
		
		List<GroundCell> moveDistance = GetSaveCellsInMoveDistance(unit);
		/*
		if (unit.index == 2106)
		{
			Debug.Log(moveDistance.Count);
		//	Debug.Log(GetSavedClosestMovePosition(unit, moveDistance));
			GetSavedClosestMovePosition(unit, moveDistance).GetComponent<SpriteRenderer>().color = new Vector4(1,0,0,1);
		}
		*/
		if (moveDistance.Count > 0)
			return GetSavedClosestMovePosition(unit, moveDistance);
		
		else
			return null;
	}
	
	public bool CheckCellForNearEnemy(GroundCell position, Unit unit)
	{
		foreach (GroundCell cell in position.closestCellList)
		{
			if (cell.onCellObject != null && cell.onCellObject.team != unit.team)
				return true;
		}
		
		return false;
	}
	
	public bool ChekCellForActiveTarget(GroundCell position, Unit unit)
	{
		List<GroundCell> distance = attackDistanceFinder.GetRangedAttackDistance(position, unit.currentStats.attackRange);
		List<GroundCell> minDistance = attackDistanceFinder.GetRangedAttackDistance(position, unit.currentStats.minAttackRange);
		
		List<GroundCell> targetList = new List<GroundCell>();
		
		foreach(GroundCell cell in minDistance)
			distance.Remove(cell);
		
		foreach(GroundCell cell in distance)
		{
			if (attackDistanceFinder.CheckTargetForAddToList(cell, unit))
				targetList.Add(cell);
		}
		
		return targetList.Count > 0;
	}
	
	public GroundCell GetFighterTarget(Unit unit, Attack attack)
	{
		if (unit.currentActionTargetList.Count != 0)
			return GetBestTarget(unit, attack);
		else
			return null;
	}
	
	private List<GroundCell> GetSaveCellsInMoveDistance(Unit unit)
	{
		List<GroundCell> saveCells = new List<GroundCell>();
		bool cellIsSave;
		
		foreach (GroundCell cell in unit.moveActionTargetList)
		{
			cellIsSave = true;
			
			foreach (GroundCell closestCell in cell.closestCellList)
			{
				if (closestCell.onCellObject != null && closestCell.onCellObject.team != unit.team)
				{
					cellIsSave = false;
					break;
				}
			}
			
			if (cellIsSave)
				saveCells.Add(cell);
		}
		
		return saveCells;
	}
	
	public GroundCell GetClosestMovePosition(Unit unit, List<GroundCell> moveDistance)
	{
		GroundCell enemyPos = GetClosestEnemyPosition(unit);
		
		if (enemyPos == null || moveDistance.Count == 0)
			return null;
		
		List<GroundCell> fullDistance = new List<GroundCell>();
		fullDistance.Add(unit.position);
		
		foreach (GroundCell cell in moveDistance)
			fullDistance.Add(cell);
		
		GroundCell closestCell = GetClosestCellFromListToTarget(enemyPos, fullDistance);
		
		if (closestCell == unit.position)
			closestCell = null;
		
		return closestCell;
	}
	
	public GroundCell GetSavedClosestMovePosition(Unit unit, List<GroundCell> moveDistance)
	{
		GroundCell enemyPos = GetClosestEnemyPosition(unit);
		
		if (enemyPos == null || moveDistance.Count == 0)
			return null;
		
		GroundCell closestCell = GetClosestCellFromListToTarget(enemyPos, moveDistance);
		
		if (closestCell == unit.position)
			closestCell = null;
		
		return closestCell;
	}

	public GroundCell GetClosestToGoalPosition(Unit unit)
	{
		GroundCell goalPosition = GetClosestGoalCell(unit, unit.aiGoal.GetGoalList(unit));
		
		return GetClosestCellFromListToTarget(goalPosition, unit.moveActionTargetList);
	}
	
	private GroundCell GetClosestGoalCell(Unit unit, List<GroundCell> goalList)
	{
		List<GroundCell> freeCellsList = GetFreeCellsFromList(goalList);
		
		return GetClosestCellFromListToTarget(unit.position, freeCellsList);
	}
	
	private GroundCell GetClosestCellFromListToTarget(GroundCell target, List<GroundCell> cellList)
	{
		if (cellList.Count == 0 || target == null)
			return null;
		
		GroundCell closestCell = cellList[0];
		
		float distance = GetMagnitude(closestCell, target);
		float currentCellDistance;
		
		foreach (GroundCell cell in cellList)
		{
			if (cell == null)
				continue;
			
			currentCellDistance = GetMagnitude(cell, target);
			
			if (currentCellDistance < distance)
			{
				closestCell = cell;
				distance = currentCellDistance;
			}
			
			
		}
		
		return closestCell;
	}
	
	private List<GroundCell> GetFreeCellsFromList(List<GroundCell> cellList)
	{
		List<GroundCell> freeCellsList = new List<GroundCell>(); 
		
		foreach (GroundCell cell in cellList)
		{
			if (cell.onCellObject == null)
				freeCellsList.Add(cell);
		}
		
		return freeCellsList;
	}
	
	private float GetMagnitude(GroundCell pos1, GroundCell pos2)
	{
		float magnitude = (pos1.transform.position - pos2.transform.position).magnitude;
		
		return magnitude;
	}
	
	private GroundCell GetClosestEnemyPosition(Unit unit)
	{
		List<NullObject> enemyObjects = GetEnemyObjects(unit.team);
		
		if (enemyObjects.Count == 0)
			return null;
		
		GroundCell closestPosition = enemyObjects[0].position;
		Vector3 distance = closestPosition.transform.position - unit.position.transform.position;
		Vector3 objPos;
		
		foreach (NullObject obj in enemyObjects)
		{
			if (obj is Unit || obj is Structure)
			{
				objPos = obj.position.transform.position - unit.position.transform.position;
				
				if (objPos.magnitude < distance.magnitude)
				{
					closestPosition = obj.position;
					distance = objPos;
				}
			}
		}
		
		return closestPosition;
	}
	
	private List<NullObject> GetEnemyObjects(int team)
	{
		List<NullObject> objectList = new List<NullObject>();
		
		foreach (NullObject obj in BattleMap.instance.objectList)
		{
			if (obj.team != team && obj is MaterialObject)
				objectList.Add(obj);
		}
		
		return objectList;
	}
	
	private GroundCell GetBestTarget(Unit unit, Attack attack)
	{
		GroundCell target = unit.currentActionTargetList[0];
		List<GroundCell> killedUnitPositions = new List<GroundCell>();
		
		int damage = damageGetter.GetDamage(target.onCellObject, unit.GetActionData(attack).damage, attack.GetBestDamageType(unit, target));
		int bestDamage = damage;
		
		foreach (GroundCell cell in unit.currentActionTargetList)
		{
			damage = damageGetter.GetDamage(cell.onCellObject, unit.GetActionData(attack).damage, attack.GetBestDamageType(unit, cell));
			
			if (bestDamage < damage)
			{
				bestDamage = damage;
				target = cell;
			}
			
			else if (bestDamage == damage
			&& cell.onCellObject.currentStats.maxHealth > target.onCellObject.currentStats.maxHealth)
			{
					target = cell;
			}
			
			if (cell.onCellObject.CheckDamageForLetality(damage, attack.GetBestDamageType(unit, cell)))
				killedUnitPositions.Add(cell);
		}
		
		if (killedUnitPositions.Count > 0)
			target = killedUnitPositions[0];
		
		foreach (GroundCell cell in killedUnitPositions)
		{
			if (cell.onCellObject.currentStats.maxHealth > target.onCellObject.currentStats.maxHealth)
				target = cell;
		}
		
		return target;
	}
	
	public GroundCell GetCreatedObjectAttackTarget(Unit unit, LivestealedAttack attack)
	{
		GroundCell target = null;
		List<GroundCell> killedUnitPositions = new List<GroundCell>();
		int damage = 0;
		
		foreach (GroundCell cell in unit.currentActionTargetList)
		{
			damage = damageGetter.GetDamage(cell.onCellObject, unit.GetActionData(attack).damage, attack.GetBestDamageType(unit, target));
			
			if (cell.onCellObject.CheckDamageForLetality(unit.GetActionData(attack).damage, attack.GetBestDamageType(unit, cell))
			&& attack.CheckObjectForCreating(cell.onCellObject))
			{
				killedUnitPositions.Add(cell);
			}
		}
		
		if (killedUnitPositions.Count > 0)
			target = killedUnitPositions[0];
		
		foreach (GroundCell cell in killedUnitPositions)
		{
			if (cell.onCellObject.currentStats.maxHealth > target.onCellObject.currentStats.maxHealth)
			{
				target = cell;
			}
		}
		
		return target;
	}
}
