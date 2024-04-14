using System.Collections;
using System.Collections.Generic;

public class BuildingDataInfo : ObjectInfo
{
	private BuildingData buildingData;
	
	public List<Unit> unitList { get; private set; }
	
	public int currentLevel => buildingData.currentLevel;
	public int maxLevel => buildingData.building.maxLevel;
	public string statusDescription => buildingData.statusDescription;
	
	public override void Init(object obj)
	{
		buildingData = obj as BuildingData;
		
		objectName = buildingData.building.Name;
		
		SetUnitList();
	}
	
	private void SetUnitList()
	{
		unitList = new List<Unit>();
		
		foreach (UnitData unitData in TurnController.currentPlayer.capital.unitDataList)
		{
			if (unitData.requiredBuildingList.Contains(buildingData.building))
				unitList.Add(unitData.unit);
		}
	}
}
