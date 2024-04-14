using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CapitalData/Upgrades/ChangeAction")]
public class ChangeActionUpgradeObject : UpgradeObject
{
    [SerializeField] private List<AbstractAction> _oldActionList;
    [SerializeField] private List<AbstractAction> _newActionList;
    [SerializeField] private List<UnitActionType> _unitActionTypeList;

    public List<AbstractAction> oldActionList => _oldActionList;
    public List<AbstractAction> newActionList => _newActionList;
    public List<UnitActionType> unitActionTypeList => _unitActionTypeList;

    public override Upgrade GetUpgrade()
    {
        return new ChangeActionUpgrade(this);
    }
}
