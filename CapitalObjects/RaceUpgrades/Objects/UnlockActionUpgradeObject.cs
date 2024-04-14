using UnityEngine;

[CreateAssetMenu(menuName = "CapitalData/Upgrades/UnlockAction")]
public class UnlockActionUpgradeObject : UpgradeObject
{
    public override Upgrade GetUpgrade()
    {
        return new UnlockActionUpgrade(this);
    }
}
