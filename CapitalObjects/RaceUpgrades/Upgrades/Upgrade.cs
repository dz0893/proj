using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade
{
    protected UpgradeObject upgradeObject;

    public Sprite icon => upgradeObject.icon;
    public string Name => upgradeObject.Name;
    public string description => upgradeObject.description;
    public Building requiredBuilding => upgradeObject.requiredBuilding;
    public int buildingLevel => upgradeObject.buildingLevel;
    public int goldCost => upgradeObject.goldCost;
    public int oreCost => upgradeObject.oreCost;
    public List<NullObject> targetList => upgradeObject.targetList;

    public bool isResearched { get; private set; }

    public string statusDescription { get; private set; }
	public bool canBeResearched { get; private set; }

    protected virtual void OnObjectEffect(NullObject obj) {}

    public void SetStatus(Player player)
    {
        canBeResearched = false;

        if (isResearched)
        {
            statusDescription = UISettings.UpgradeResearched;
        }
        else if (!CheckRequiredBuildings(player.capital))
        {
            statusDescription = UISettings.cantBuildUnit;
        }
        else if (player.gold < goldCost || player.ore < oreCost)
        {
            statusDescription = UISettings.notEnoughtResources;
        }
        else if (player.capital.turnEnded)
        {
            statusDescription = UISettings.cantResearchToday;
        }
        else if (player != TurnController.currentPlayer)
        {
            statusDescription = UISettings.cantResearchToday;
        }
        else
        {
            statusDescription = UISettings.canBeResearched;
            canBeResearched = true;
        }
    }

    public virtual void MakeUpgradeForPlayer(Player player)
    {
        isResearched = true;

        foreach (NullObject obj in GetObjectsForUpgrade(player))
        {
            SetUpgradeToObject(obj);
        }
    }

    public void SetUpgradeToObject(NullObject obj)
    {
        if (CheckObjectForActivate(obj))
        {
            OnObjectEffect(obj);
            obj.upgradeList.Add(this);
        }
    }

    private bool CheckObjectForActivate(NullObject obj)
    {
        List<string> objectNames = GetObjectNames();

        if (obj.upgradeList.Contains(this) || !objectNames.Contains(obj.Name))
            return false;
        else
            return true;
    }

    protected List<NullObject> GetObjectsForUpgrade(Player player)
    {
        List<string> unitNames = GetObjectNames();
        List<NullObject>  objectList = new List<NullObject>();
        
        foreach (NullObject obj in player.objectList)
        {
            if (unitNames.Contains(obj.Name))
                objectList.Add(obj);
        }

        return objectList;
    }

    private List<string> GetObjectNames()
    {
        List<string> objectNames = new List<string>();

        foreach (NullObject obj in targetList)
            objectNames.Add(obj.Name);

        return objectNames;
    }

    public bool CheckRequiredBuildings(Capital capital)
	{
		if (capital == null || requiredBuilding == null)
			return true;
		
        foreach (BuildingData buildingData in capital.buildingList)
        {
            if (buildingData.building == requiredBuilding && buildingData.currentLevel >= buildingLevel)
				return true;
        }
		
		return false;
	}
}
