using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDataInfoContainer : AbstractInfoContainer
{
	[SerializeField] private Text _descriptionField;
	[SerializeField] private Text _currentLevelField;
	[SerializeField] private Text _maxLevelField;
	[SerializeField] private Text _statusField;
	
	[SerializeField] private Text _unitNamePref;
	[SerializeField] private Transform _unitNamesContainer;
	
	public override void Render(ObjectInfo info)
	{
		BuildingDataInfo buildingDataInfo = info as BuildingDataInfo;
		
		_statusField.text = buildingDataInfo.statusDescription;
		RenderLevel(buildingDataInfo);
		RenderUnits(buildingDataInfo.unitList);
	}
	
	private void RenderLevel(BuildingDataInfo buildingDataInfo)
	{
		_currentLevelField.text = UISettings.Lvl + buildingDataInfo.currentLevel;
		_maxLevelField.text = UISettings.maxLevel + buildingDataInfo.maxLevel;
	}
	
	private void RenderUnits(List<Unit> unitList)
	{
		CleanContainer(_unitNamesContainer);
		InitUnits(unitList);
		
	}
	
	private void CleanContainer(Transform container)
	{
		foreach (Transform child in container)
			Destroy(child.gameObject);
	}
	
	private void InitUnits(List<Unit> unitList)
	{
		if (unitList.Count == 0)
		{
			_descriptionField.text = UISettings.unlockGlobals;
			return;
		}
		
		foreach (Unit unit in unitList)
		{
			_descriptionField.text = UISettings.unlockNextUnits;
			Text cell = Instantiate(_unitNamePref, _unitNamesContainer);
			cell.text = unit.Name;
		}
	}
}
