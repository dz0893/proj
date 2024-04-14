using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealHealth : AbstractAction, IDamage
{
	public override bool endedTurnAction => true;
	public override bool damageFromUnitStats => true;
	public override bool rangeFromUnitStats => true;
	
	public override ActionType actionType => ActionType.Offensive;
	
	[SerializeField] private ActionRange _range;
	public override ActionRange range => _range;
	
	[SerializeField] private DamageType _damageType;
	public DamageType damageType => _damageType;
	
	public List<DamageType> damageTypeList
	{
		get
		{
			List<DamageType> list = new List<DamageType>();
			list.Add(damageType);
			return list;
		}
	}
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return attackDistanceFinder.GetAttackDistance(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		WasteResoursesAndEndTurn(unit);
		
		Action(unit, target);
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
	
	private void Action(Unit unit, GroundCell target)
	{
	//	SetDamage(unit);
		
		int deltaHealth = target.onCellObject.currentHealth;
		
		int exp = target.onCellObject.GetAttack(unit.GetActionData(this).damage, damageType);
		
		deltaHealth -= target.onCellObject.currentHealth;
		
		if (!target.onCellObject.isMech && !target.onCellObject.isUndead)
			unit.RestoreHealth(deltaHealth);
		
		unit.player.AddExpToUnits(exp);
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
		return actionTextGetter.GetDamageText(cell.onCellObject, unit.GetActionData(this).damage, damageType);
	}
}
