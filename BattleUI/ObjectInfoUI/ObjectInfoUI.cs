using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectInfoUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Text _nameField;
	[SerializeField] private Transform _fullInfoContainer;
	[SerializeField] private Transform _background;
	
	[SerializeField] private GroundInfoContainer _groundInfo;
	[SerializeField] private BuildingDataInfoContainer _buildingDataInfo;
	[SerializeField] private UnitDataInfoContainer _unitDataInfo;
	[SerializeField] private UnitInfoContainer _unitInfo;
	[SerializeField] private StructureInfoContainer _structureInfo;
	[SerializeField] private UnmaterialObjectInfoContainer _unmaterialObjectInfo;
	[SerializeField] private ActionInfoContainer _actionInfo;
	[SerializeField] private GlobalActionInfoContainer _globalActionInfo;
	[SerializeField] private UpgradeInfoContainer _upgradeInfo;
	
	private AbstractInfoContainer currentInfoContainer;
	
	public delegate void WriteInfo(ObjectInfo info);
	public delegate void CleanInfo();
	
	public static WriteInfo writeInfo;
	public static CleanInfo cleanInfo;
	
	public static bool CoursorOnObjectInfo;
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		CoursorOnObjectInfo = true;
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		CoursorOnObjectInfo = false;
		
		CloseObjectInfo();
	}
	
	private void Start()
	{
		writeInfo = OpenObjectInfo;
		cleanInfo = CloseObjectInfo;
	}
	
	private void OpenObjectInfo(ObjectInfo info)
	{
		_background.gameObject.SetActive(true);
		
		WriteObjectInfo(info);
	}
	
	private void CloseObjectInfo()
	{
		Clean();
		_background.gameObject.SetActive(false);
	}
	
	private void WriteObjectInfo(ObjectInfo info)
	{
		Clean();
		
		WriteFullObjectInfo(info);
	}

	private void WriteFullObjectInfo(ObjectInfo info)
	{
		_background.gameObject.SetActive(true);
		_nameField.text = info.objectName;
		
		SetInfoContainer(info);
		currentInfoContainer.gameObject.SetActive(true);
		currentInfoContainer.Render(info);
	}
	
	private void SetInfoContainer(ObjectInfo info)
	{
		if (info is GroundInfo)
			currentInfoContainer = _groundInfo;
		
		if (info is UnitDataInfo)
			currentInfoContainer = _unitDataInfo;
		
		if (info is BuildingDataInfo)
			currentInfoContainer = _buildingDataInfo;
		
		if (info is UnitInfo)
			currentInfoContainer = _unitInfo;
		
		if (info is StructureInfo)
			currentInfoContainer = _structureInfo;
		
		if (info is UnmaterialObjectInfo)
			currentInfoContainer = _unmaterialObjectInfo;
			
		if (info is ActionInfo)
			currentInfoContainer = _actionInfo;
		
		if (info is GlobalActionInfo)
			currentInfoContainer = _globalActionInfo;
		
		if (info is UpgradeInfo)
			currentInfoContainer = _upgradeInfo;
	}
	
	public void Clean()
	{
		_nameField.text = null;
		
		foreach (Transform child in _fullInfoContainer)
			child.gameObject.SetActive(false);
	}
}
