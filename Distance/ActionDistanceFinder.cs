using System.Collections.Generic;
using UnityEngine;

public class ActionDistanceFinder
{
	private MovingDistanceFinder movingDistanceFinder = new MovingDistanceFinder();
	private AttackDistanceFinder attackDistanceFinder = new AttackDistanceFinder();
	
	public List<GroundCell> GetCasterPosition(Unit unit)
	{
		List<GroundCell> distance = new List<GroundCell>();
		
		distance.Add(unit.position);
		
		return distance;
	}
	
	public List<GroundCell> GetUpgradeMechDistance(Unit unit)
	{
		List<GroundCell> distance = GetMechDistance(unit);
		
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in distance)
		{
			if (!cell.onCellObject.canBeUpgraded || cell.onCellObject is Structure)
				removedCells.Add(cell);
		}
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	public List<GroundCell> GetMechDistance(Unit unit)
	{
		List<GroundCell> distance = attackDistanceFinder.GetAttackDistance(unit);
		
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in distance)
		{
			if (!cell.onCellObject.isMech)
				removedCells.Add(cell);
		}
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	public List<GroundCell> GetBuryDistance(Unit unit, Bury action)
	{
		List<GroundCell> distance = new List<GroundCell>();

		if (action.needFullMovePoints)
		{
			foreach (GroundCell cell in unit.position.closestCellList)
			{
				if (cell.movingType == MovingType.Walk && cell.onCellObject == null && cell.unmaterialOnCellObject == null)
				{
					distance.Add(cell);
				}
			}
		}
		else
		{
			distance = movingDistanceFinder.GetMoveDistance(unit);
		}

		return distance;
	}

	public List<GroundCell> GetEnemyTrapsDistance(Unit unit)
	{
		List<GroundCell> moveDistance = movingDistanceFinder.GetMoveDistance(unit);
		
		moveDistance.Add(unit.position);
		
		List<GroundCell> distance = new List<GroundCell>();
		
		foreach (GroundCell movingCell in moveDistance)
		{
			foreach (GroundCell cell in movingCell.closestCellList)
			{
				if (cell.unmaterialOnCellObject != null && cell.unmaterialOnCellObject.initer == null
				&& cell.unmaterialOnCellObject.team != unit.team && cell.onCellObject == null)
					distance.Add(cell);
			}
		}
		
		return distance;
	}
	
	public List<GroundCell> GetOnUnitEffectDistance(Unit unit)
	{
		List<GroundCell> distance = new List<GroundCell>();
		List<GroundCell> removedCells = new List<GroundCell>();
		
		SetEffectOnUnit action = unit.choosenAction as SetEffectOnUnit;
		
		if (action.range == ActionRange.Melee)
			distance = attackDistanceFinder.GetAttackDistance(unit);
		else
			distance = attackDistanceFinder.GetRangedAttackTargetCells(unit, unit.GetActionData(action).attackRange);
		
		if (action.actionType == ActionType.Defensive)
			distance.Add(unit.position);
		
		foreach (GroundCell cell in distance)
		{
			//if (cell.onCellObject != null && cell.onCellObject is Structure)
			if (action.actionEffect.IsWrongTarget(cell.onCellObject))
				removedCells.Add(cell);
		}
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	public List<GroundCell> GetChangeUnitDistance(Unit unit)
	{
		List<GroundCell> distance = attackDistanceFinder.GetAttackDistance(unit);
		
		ChangeUnit action = unit.choosenAction as ChangeUnit;
		
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in distance)
		{
			if (cell.onCellObject != null && !cell.onCellObject.Name.Equals(action.changetUnit.Name))
				removedCells.Add(cell);
		}
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	public List<GroundCell> GetHealingDistance(Unit unit)
	{
		Heal heal = unit.choosenAction as Heal;
		
		List<GroundCell> distance = new List<GroundCell>();
		List<GroundCell> removedCells = new List<GroundCell>();
		
		if (heal.range != ActionRange.OnCaster)
			distance = attackDistanceFinder.GetAttackDistance(unit);
		
		if (!distance.Contains(unit.position))	
			distance.Add(unit.position);
		
		foreach (GroundCell cell in distance)
		{
			if ((cell.onCellObject.isMech ^ heal.isRepairing)
			|| cell.onCellObject.currentHealth >= cell.onCellObject.currentStats.maxHealth)
				removedCells.Add(cell);
		}
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	public List<GroundCell> GetRevivingUnitDistance(Unit unit)
	{
		RiseDead action = unit.choosenAction as RiseDead;
		
		List<GroundCell> distance = attackDistanceFinder.GetAttackDistance(unit);

		List<GroundCell> revivingCells = GetRevivingUnitCells(unit, distance, action);
		return revivingCells;
	}
	
	public List<GroundCell> GetFreeCellsWithGrave(Unit unit)
	{
		List<GroundCell> distance = attackDistanceFinder.GetAttackDistance(unit);
		
		List<GroundCell> removedCells = new List<GroundCell>();
		
		foreach (GroundCell cell in distance)
		{
			if (cell.grave.Count == 0)
				removedCells.Add(cell);
		}
		
		foreach (GroundCell cell in removedCells)
			distance.Remove(cell);
		
		return distance;
	}
	
	private List<GroundCell> GetRevivingUnitCells(Unit caster, List<GroundCell> distance, RiseDead action)
	{
		List<GroundCell> revivingCells = new List<GroundCell>();
		
		foreach (GroundCell cell in distance)
		{
			if (action.riseAsUndead)
			{
				if (CheckCellForRiseUndead(cell))
					revivingCells.Add(cell);
			}
			else
			{
				if (CheckCellForReviving(cell, caster))
					revivingCells.Add(cell);
			}
		}
		
		return revivingCells;
	}
	
	private bool CheckCellForReviving(GroundCell cell, Unit caster)
	{
		bool cellHaveRequiredBody = false;
		
		if (cell.grave.Count > 0)
		{
			foreach (Unit unit in cell.grave)
			{
				if (unit.team == caster.team && cell.onCellObject == null)
				{
					cellHaveRequiredBody = true;
					break;
				}
			}
		}
		
		return cellHaveRequiredBody;
	}
	
	private bool CheckCellForRiseUndead(GroundCell cell)
	{
		if (cell.grave.Count != 0 || cell.terrainType == TerrainType.DeadGround && cell.onCellObject == null)
			return true;
		
		else
			return false;
	}
}
