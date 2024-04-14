using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundAttack : AbstractAction, IDamage, IAreaAction
{
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	private EnemyDistanceFinder enemyDistanceFinder = new EnemyDistanceFinder();
	
	public override bool endedTurnAction => true;
	public override bool damageFromUnitStats => true;
	public override bool rangeFromUnitStats => true;
	
	public override ActionType actionType => ActionType.Offensive;
	public override ActionRange range => ActionRange.OnCaster;
	
	[SerializeField] private DamageType _damageType;
	public DamageType damageType => _damageType;
	
	public int area => 1;
	
	public virtual List<DamageType> damageTypeList
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
		return actionDistanceFinder.GetCasterPosition(unit);
	}
	
	public override GroundCell GetAITarget(Unit unit)
	{
		return enemyDistanceFinder.GetRoundAttackTarget(unit);
	}
	
	public List<GroundCell> GetAreaDistance(GroundCell position)
	{
		return position.closestCellList;
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
		unit.inAction = true;
		WasteResoursesAndEndTurn(unit);
		
		actionTime = ActionSettings.ATTACKTIME;
		
		int totalExp = 0;
		int currentExp = 0;
		
		foreach (GroundCell cell in GetAreaDistance(unit.position))
		{
			if (cell.onCellObject != null)
			{
				MaterialObject targetedObject = cell.onCellObject;
				currentExp = targetedObject.GetAttack(unit.GetActionData(this).damage, damageType);
				
				if (targetedObject.team != unit.team)
					totalExp += currentExp;
			}
		}
		
		unit.player.AddExpToUnits(totalExp);
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
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
