using System.Collections.Generic;
using UnityEngine;

public class ChangeActionUpgrade : Upgrade
{
    public List<AbstractAction> oldActionList { get; private set; }
    public List<AbstractAction> newActionList { get; private set; }
    public List<UnitActionType> unitActionTypeList { get; private set; }

    public ChangeActionUpgrade(ChangeActionUpgradeObject upgradeObject)
    {
        this.upgradeObject = upgradeObject;

        oldActionList = upgradeObject.oldActionList;
        newActionList = upgradeObject.newActionList;
        unitActionTypeList = upgradeObject.unitActionTypeList;
    }

    protected override void OnObjectEffect(NullObject obj)
    {
        Unit unit = obj as Unit;
        int actionIndex = GetActionIndex(unit);
        unit.ChangeAction(oldActionList[actionIndex], newActionList[actionIndex], unitActionTypeList[actionIndex]);
    }

    private int GetActionIndex(Unit unit)
    {
        int index = 0;

        for (int i = 0; i < unit.actionList.Count; i++)
        {
            if (oldActionList.Contains(unit.actionList[i]))
            {
                index = oldActionList.IndexOf(unit.actionList[i]);
            }
        }

        return index;
    }
}
