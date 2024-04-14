using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInfoContainer : AbstractInfoContainer
{
    [SerializeField] private Text _descriptionText;
    [SerializeField] private Text _statusText;
    [SerializeField] private Text _requiredBuildingText;

    public override void Render(ObjectInfo info)
    {
        UpgradeInfo upgradeInfo = info as UpgradeInfo;

        _descriptionText.text = upgradeInfo.description;
        _statusText.text = upgradeInfo.statusDescription;
        _requiredBuildingText.text = UISettings.mustBuild + upgradeInfo.requiredBuildingName + UISettings.lv + upgradeInfo.buildingLevel;
    }
}
