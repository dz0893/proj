using System.Collections.Generic;
using UnityEngine;

public class ActionData : IActionDescription
{
    private Unit unit;
    public AbstractAction abstractAction { get; private set; }

    public string Name => abstractAction.Name;
    public List<string> descriptionList => abstractAction.GetDescription(unit, this);
    public Sprite icon => abstractAction.icon;
    public bool isAreaAction => abstractAction is IAreaAction;
//    public bool isRecharget;

//    private List<string> descriptionList = new List<string>();

    private UnitStats unitStats
    {
        get
        {
            if (unit.player != null)
                return unit.currentStats;
            else
                return unit.GetBasicStats();
        }
    }

    public int damage
    {
        get
        {
            int totalDamage = 0;

            if (abstractAction.damageFromUnitStats)
			    totalDamage = unitStats.damage + abstractAction.damageIntegerModifire;
		    else
			    totalDamage = abstractAction.constDamage;
            
            if (abstractAction.modifiredAfterMove && unit.currentMovePoints < unitStats.maxMovePoints)
			    totalDamage -= unit.counterAttackModifier;

            totalDamage += abstractAction.GetCurrentActionDamageModifire(unit);

		    return totalDamage;
        }
    }

	public int attackRange
    {
        get
        {
            if (abstractAction.rangeFromUnitStats)
			    return unitStats.attackRange;
            else
                return abstractAction.AttackRange;
        }
    }

	public int minAttackRange
    {
        get
        {
            if (abstractAction.rangeFromUnitStats)
			    return unitStats.minAttackRange;
            else
                return abstractAction.MinAttackRange;
        }
    }

    public ActionData(Unit unit, AbstractAction abstractAction)
    {
        this.unit = unit;
        this.abstractAction = abstractAction;
    }

    public string GetRenderedText(GroundCell cell)
    {
        return abstractAction.GetRenderedText(unit, cell);
    }

    public List<GroundCell> GetAreaDistance(GroundCell target)
    {
        List<GroundCell> area = new List<GroundCell>();

        if (isAreaAction)
        {
            IAreaAction areaAction = abstractAction as IAreaAction;

            area = areaAction.GetAreaDistance(target);
        }
        else
        {
            area.Add(target);
        }
        
        return area;
    }
}
