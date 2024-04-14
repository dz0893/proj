using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CapitalData/Upgrades/ChangeUnitCost")]
public class ChangeUnitCostUpgradeObject : UpgradeObject
{
    [SerializeField] private List<UnitDataObject> _unitDataList;
    public List<UnitDataObject> unitDataList => _unitDataList;
    
    [SerializeField] private int _goldCostDecreasingValue;
    public int goldCostDecreasingValue => _goldCostDecreasingValue;

    [SerializeField] private int _oreCostDecreasingValue;
    public int oreCostDecreasingValue => _oreCostDecreasingValue;

    public override Upgrade GetUpgrade()
    {
        return new ChangeUnitCostUpgrade(this);
    }
}
