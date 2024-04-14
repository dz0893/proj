using System.Collections.Generic;

public class ChangeUnitCostUpgrade : Upgrade
{
    public List<UnitDataObject> unitDataList { get; private set; }
    public int goldCostDecreasingValue { get; private set; }
    public int oreCostDecreasingValue { get; private set; }

    public ChangeUnitCostUpgrade(ChangeUnitCostUpgradeObject upgradeObject)
    {
        this.upgradeObject = upgradeObject;

        unitDataList = upgradeObject.unitDataList;
        goldCostDecreasingValue = upgradeObject.goldCostDecreasingValue;
        oreCostDecreasingValue = upgradeObject.oreCostDecreasingValue;
    }

    protected override void OnObjectEffect(NullObject obj)
    {
        RecruitPoint recruitPoint = obj as RecruitPoint;
        
        foreach (UnitData unitData in recruitPoint.unitDataList)
        {
            if (NeedToChangeCost(unitData))
                unitData.ChangeCost(-goldCostDecreasingValue, -oreCostDecreasingValue);
        }
    }

    private bool NeedToChangeCost(UnitData unitData)
    {
        foreach (UnitDataObject dataObject in unitDataList)
        {
            if (dataObject.unit == unitData.unit)
                return true;
        }

        return false;
    }
}
