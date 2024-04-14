using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeVehicle : AbstractAction, IStatChanger
{
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	
	public override bool endedTurnAction => true;
	
	public override ActionType actionType => ActionType.Defensive;
	public override ActionRange range => ActionRange.Melee;
	
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _maxMana;
	[SerializeField] private int _damage;
	[SerializeField] private int _physicalDefence;
	[SerializeField] private int _piercingDefence;
	[SerializeField] private int _magicDefence;
	[SerializeField] private int _siegeDefence;
	[SerializeField] private int _maxMovePoints;
	[SerializeField] private int _expForDestroing;
	[SerializeField] private int _attackRangeSt;
	[SerializeField] private int _minAttackRangeSt;
	[SerializeField] private int _restoreManaPerMeditate;
	[SerializeField] private int _healthRegen;
	[SerializeField] private int _manaRegen;
	
	public UnitStats stats
	{
		get
		{
			return new UnitStats(_maxHealth, _maxMana, _damage, _physicalDefence, _piercingDefence, _magicDefence,
			_siegeDefence, _maxMovePoints, _expForDestroing,_attackRangeSt, _minAttackRangeSt, _restoreManaPerMeditate,
			_healthRegen, _manaRegen);
		}
	}
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetUpgradeMechDistance(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		Upgrade(target.onCellObject);
		
		WasteResoursesAndEndTurn(unit);
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
	
	private void Upgrade(MaterialObject obj)
	{
		obj.ChangeBasicStats(stats);
		
		obj.canBeUpgraded = false;
	}
}
