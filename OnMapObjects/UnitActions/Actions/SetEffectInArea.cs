using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEffectInArea : SetEffectOnUnit, IAreaAction
{
	private AttackDistanceFinder attackDistanceFinder = new AttackDistanceFinder();
	private MovingDistanceFinder movingDistanceFinder = new MovingDistanceFinder();
	private EnemyDistanceFinder enemyDistanceFinder = new EnemyDistanceFinder();
	
	[SerializeField] private int _area;
	public int area => _area;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		if (range == ActionRange.OnCaster)
			return actionDistanceFinder.GetCasterPosition(unit);
		
		else
			return attackDistanceFinder.GetRangedAttackDistance(unit.position, unit.GetActionData(this).attackRange);
	}
	
	public override GroundCell GetAITarget(Unit unit)
	{
		return enemyDistanceFinder.GetAreaActionTarget(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
		WasteResoursesAndEndTurn(unit);
		
		SetAreaEffect(unit, target);
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
	
	public List<GroundCell> GetAreaDistance(GroundCell position)
	{
		List<GroundCell> distance = movingDistanceFinder.GetCurrentCellList(position.closestCellList);
		distance.Add(position);
		
		List<GroundCell> checkedCells = movingDistanceFinder.GetCurrentCellList(position.closestCellList);
		
		for (int i = 0; i < _area; i++)
		{
			foreach (GroundCell cell in checkedCells)
			{
				if (!distance.Contains(cell))
					distance.Add(cell);
			}
			
			foreach (GroundCell checkedCell in distance)
			{
				foreach (GroundCell cell in checkedCell.closestCellList)
				{
					if (!distance.Contains(cell))
						checkedCells.Add(cell);
				}
			}
		}
		
		return distance;
		
	}
	
	private void SetAreaEffect(Unit caster, GroundCell center)
	{
		List<GroundCell> areaOfAction = GetAreaDistance(center);
		
		foreach (GroundCell cell in areaOfAction)
		{
			if (cell.onCellObject != null && cell.onCellObject is Unit && CheckEffectToActivateOnCurrentObject(caster, cell.onCellObject))
				SetEffect(cell.onCellObject as Unit);
		}
	}
	
	private bool CheckEffectToActivateOnCurrentObject(Unit caster, MaterialObject obj)
	{
		if (_actionEffect.isNegative ^ (caster.team == obj.team))
			return true;
		else
			return false;
	}
}
