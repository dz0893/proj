using System.Collections.Generic;
using UnityEngine;

public class Capital : UnmaterialObjectsIniter
{
	[SerializeField] private Race _race;
	public Race race => _race;
	
	[SerializeField] private TerrainType _raceTerrainType;
	public TerrainType raceTerrainType => _raceTerrainType;
	
	[SerializeField] private int _basicGold;
	[SerializeField] private int _basicOre;
	public int basicGold => _basicGold;
	public int basicOre => _basicOre;
	
	[SerializeField] private int _goldIncome;
	[SerializeField] private int _oreIncome;
	
	[SerializeField] private List<Building> _basicBuildingList;
	[SerializeField] private List<int> _basicBuildingLevelList;

	[SerializeField] private List<UpgradeObject> _upgradeObjectList;
	public List<Upgrade> upgradeList { get; private set; }
	
	private List<int> buildingLevelList;
	
	public List<BuildingData> buildingList { get; private set; }
	
	[SerializeField] private List<UnitDataObject> _unitDataObjectList;
	public List<UnitDataObject> unitDataObjectList => _unitDataObjectList;
	
	public List<UnitData> unitDataList = new List<UnitData>();

	public List<Unit> unitList { get; set; } = new List<Unit>();
	
	public void Build(BuildingData buildingData)
	{
		buildingData.IncreaseLevel();
		RefreshUnitList();
		
		turnEnded = true;
	}
	
	protected override void CurrentStructureInit(GroundCell positionCell)
	{
		InitRecruitPoints(positionCell);
		InitBuildingLevelList(_basicBuildingLevelList);
		InitBuildingList();
		InitUnitList();
		InitUpgradeList();
		
		SetGround(positionCell);
		
		player.ChangeGoldIncome(_goldIncome);
		player.ChangeOreIncome(_oreIncome);
	}
	
	public void SetBuildingBlockedLevels(List<int> blockingBuildingLevelList)
	{
		for (int i = 0; i < buildingList.Count; i++)
			buildingList[i].SetLevelOfBlocking(blockingBuildingLevelList[i]);
	}
	
	private void InitBuildingList()
	{
		buildingList = new List<BuildingData>();
		
		for (int i = 0; i < _basicBuildingList.Count; i++)
			buildingList.Add(new BuildingData(_basicBuildingList[i], buildingLevelList[i]));
	}
	
	public void InitBuildingLevelList(List<int> levelList)
	{
		buildingLevelList = new List<int>();
		
		foreach (int level in levelList)
			buildingLevelList.Add(level);
	}
	
	private void InitUnitList()
	{
		unitDataList = new List<UnitData>();
		unitList = new List<Unit>();

		foreach (UnitDataObject dataObject in _unitDataObjectList)
			unitDataList.Add(dataObject.GetUnitData(player));
		
		foreach (UnitData unitData in unitDataList)
			unitData.UnlockUnit(this);
	}

	public void RefreshUnitList()
	{
		unitList = new List<Unit>();
		
		foreach (UnitData unitData in unitDataList)
			unitData.UnlockUnit(this);
	}

	private void InitUpgradeList()
	{
		upgradeList = new List<Upgrade>();

		foreach (UpgradeObject upgradeObject in _upgradeObjectList)
			upgradeList.Add(upgradeObject.GetUpgrade());
	}
}
