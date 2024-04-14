using System.Collections.Generic;
using UnityEngine;

public class UpgradeInfo : ObjectInfo
{
    public Upgrade upgrade { get; private set; }

    public string description => upgrade.description;

    public string requiredBuildingName => upgrade.requiredBuilding.Name;
    public string buildingLevel => upgrade.buildingLevel.ToString();
    public string statusDescription => upgrade.statusDescription;

    public override void Init(object obj)
	{
		upgrade = obj as Upgrade;
		
		objectName = upgrade.Name;
	}
}
