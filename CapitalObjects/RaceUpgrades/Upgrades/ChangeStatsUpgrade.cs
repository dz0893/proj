using System.Collections.Generic;
using UnityEngine;

public class ChangeStatsUpgrade : Upgrade
{
    public UnitStats unitStats { get; private set; }

    public ChangeStatsUpgrade(ChangeStatsUpgradeObject upgradeObject)
    {
        this.upgradeObject = upgradeObject;

        unitStats = upgradeObject.unitStats;
    }

    protected override void OnObjectEffect(NullObject obj)
    {
        MaterialObject materialObject = obj as MaterialObject;
        materialObject.ChangeBasicStats(unitStats);
        
        if (unitStats.maxHealth > 0)
            materialObject.currentHealth += unitStats.maxHealth;
        
        if (unitStats.maxMana > 0)
            materialObject.currentMana += unitStats.maxMana;
        
        materialObject.RefreshHealthBar();
    }
}
