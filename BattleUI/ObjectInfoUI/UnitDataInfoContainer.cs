using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDataInfoContainer : AbstractInfoContainer
{
	[SerializeField] private Transform _requiredBuildingContainer;
	[SerializeField] private RequiredBuildingCell _cellPref;
	
	[SerializeField] private Text _descriptionField;
	[SerializeField] private Text _statusField;
	[SerializeField] private Text _requiredBuildingHeaderField;
	
	
	public override void Render(ObjectInfo info)
	{
		UnitDataInfo unitDataInfo = info as UnitDataInfo;
		
		_descriptionField.text = unitDataInfo.description;
		_statusField.text = unitDataInfo.statusDescription;
		RenderBuildingList(unitDataInfo);
	}
	
	private void RenderBuildingList(UnitDataInfo unitDataInfo)
	{
		CleanRequiredBuildingList();
		RenderRequiringHeader(unitDataInfo);
		InitRequiredBuildingList(unitDataInfo);
	}
	
	private void CleanRequiredBuildingList()
	{
		foreach(Transform child in _requiredBuildingContainer)
			Destroy(child.gameObject);
			
	}
	
	private void InitRequiredBuildingList(UnitDataInfo unitDataInfo)
	{
		for (int i = 0; i < unitDataInfo.requiredBuildingList.Count; i++)
		{
			RequiredBuildingCell cell = Instantiate(_cellPref, _requiredBuildingContainer);
			cell.Render(unitDataInfo, i);
		}
	}

	private void RenderRequiringHeader(UnitDataInfo unitDataInfo)
	{
		if (unitDataInfo.requiredBuildingList.Count > 0)
			_requiredBuildingHeaderField.gameObject.SetActive(true);
		else
			_requiredBuildingHeaderField.gameObject.SetActive(false);
	}
}
