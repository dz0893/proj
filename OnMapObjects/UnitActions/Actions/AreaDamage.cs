using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : AbstractAction, IAreaAction, IDamage
{
	private DamageGetter damageGetter = new DamageGetter();
	
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	private MovingDistanceFinder movingDistanceFinder = new MovingDistanceFinder();
	private EnemyDistanceFinder enemyDistanceFinder = new EnemyDistanceFinder();
	
	public override bool endedTurnAction => true;
	
	[SerializeField] private bool _damageFromUnitStats;
	public override bool damageFromUnitStats => _damageFromUnitStats;
	
	[SerializeField] private bool _rangeFromUnitStats;
	public override bool rangeFromUnitStats => _rangeFromUnitStats;
	
	public override ActionType actionType => ActionType.Offensive;
	
	[SerializeField] private ActionRange _range;
	public override ActionRange range => _range;
	
	[SerializeField] private DamageType _damageType;
	public DamageType damageType => _damageType;
	
	[SerializeField] private List<DamageType> _damageTypeList;
	public List<DamageType> damageTypeList => _damageTypeList;
	
	[SerializeField] private bool _canBeUsedAfterMoving = true;
	public bool canBeUsedAfterMoving => _canBeUsedAfterMoving = true;
	
	[SerializeField] private int _area;
	public int area => _area;
	
	private int totalExp;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		int minAttackRange = unit.GetActionData(this).minAttackRange;
		int attackRange = unit.GetActionData(this).attackRange;

		if (range == ActionRange.OnCaster)
			return actionDistanceFinder.GetCasterPosition(unit);
		
		else
		{
			if (rangeFromUnitStats)
				return attackDistanceFinder.GetRangedAttackDistanceWithMinRange(unit);
			else
				return attackDistanceFinder.GetRangedActionDistanceWithMinRange(unit.position, minAttackRange, attackRange);
		}
	}
	
	public override GroundCell GetAITarget(Unit unit)
	{
		return enemyDistanceFinder.GetAreaActionTarget(unit);
	}

	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
		List<GroundCell> areaOfAction = GetAreaDistance(target);
		
		while (AnimationController.flyAnimationIsActive)
			yield return null;
		
		totalExp = 0;

		foreach (GroundCell cell in areaOfAction)
			totalExp += DealDamage(unit, cell);
		
		unit.player.AddExpToUnits(totalExp);
		WasteResoursesAndEndTurn(unit);
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
	
	private int DealDamage(Unit caster, GroundCell cell)
	{
		int expForAttack = 0;
		
		MaterialObject target = cell.onCellObject;
		
		if (target != null)
		{
			expForAttack = target.GetAttack(caster.GetActionData(this).damage, GetBestDamageType(caster, cell));
			
			if (target.team == caster.team)
				expForAttack = 0;
		}
		
		return expForAttack;
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
	
	protected override bool CurrentActivateCheck(Unit unit)
	{
		if (!_canBeUsedAfterMoving && unit.currentMovePoints < unit.currentStats.maxMovePoints)
			return false;
		
		return true;
	}
	
	public DamageType GetBestDamageType(Unit caster, GroundCell target)
	{
		if (target.onCellObject == null)
			return damageType;
		
		DamageType bestDamageType = _damageTypeList[0];
		
		int bestDamage = damageGetter.GetDamage(target.onCellObject, caster.GetActionData(this).damage, _damageTypeList[0]);
		
		foreach (DamageType damageType in _damageTypeList)
		{
			int currentDamage = damageGetter.GetDamage(target.onCellObject, caster.GetActionData(this).damage, damageType);
			
			if (currentDamage > bestDamage)
			{
				bestDamage = currentDamage;
				bestDamageType = damageType;
			}
		}
		
		return bestDamageType;
	}
	
	protected override string GetDamageString(ActionData actionData)
	{
		string damageString;
		
		damageString = UISettings.damage + actionData.damage + " ";
		
		for (int i = 0; i < damageTypeList.Count; i++)
		{
			damageString += UISettings.GetDamageTypeName(damageTypeList[i]);
			
			if (i != damageTypeList.Count - 1)
				damageString += "/";
		}
		
		return damageString;
	}
	
	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		return actionTextGetter.GetDamageText(cell.onCellObject, unit.GetActionData(this).damage, GetBestDamageType(unit, cell));
	}
}
