using System.Collections.Generic;

public class UnitDataInfo : ObjectInfo
{
	private UnitData unitData;
	
	public List<Building> _requiredBuildingList;
	
	public List<Building> requiredBuildingList => unitData.requiredBuildingList;
	public List<int> buildingLevelList => unitData.buildingLevelList;
	
	public List<bool> buildingStateList { get; private set; }
	
	public string description => unitData.unit.description;
	
	public string statusDescription => unitData.statusDescription;
	public bool canBeRecruited => unitData.canBeRecruited;

	public override void Init(object obj)
	{
		unitData = obj as UnitData;
		
		objectName = unitData.unit.Name;
		
		CheckBuildingList();
	}
	
	private void CheckBuildingList()
	{
		buildingStateList = new List<bool>();
		
		foreach (BuildingData buildingData in unitData.player.capital.buildingList)
		{
			if (requiredBuildingList.Contains(buildingData.building))
				buildingStateList.Add(unitData.CheckCurrentBuilding(buildingData));
		}
	}
}
