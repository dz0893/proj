using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour
{
	[SerializeField] private MissionCell cellPref;
	[SerializeField] private Transform _container;
	[SerializeField] private Transform _missionListContainer;
	[SerializeField] private Transform _journalSheetListContainer;
	[SerializeField] private Text _missionNameText;
	[SerializeField] private Text _missionDescriptionText;
	[SerializeField] private Image _iconField;
	
	private List<MissionCell> missionCellList;
	private List<MissionCell> journalCellList;
	
	public delegate void SelectMission(MissionCell missionCell);
	public static SelectMission selectMission;
	
	private void Start()
	{
		selectMission = SelectMissionCell;
	}
	
	public void OpenJournalButton()
	{
		if (!MapController.controllerIsBlocked)
			SwitchContainerState();
	}
	
	public void SwitchContainerState()
	{
		_container.gameObject.SetActive(!_container.gameObject.activeSelf);
		
		Render();
	}
	
	private void Render()
	{
		Clean();
		RenderMissionList();
		RenderJournalSheetList();
	}
	
	private void Clean()
	{
		CleanContainer(_missionListContainer);
		CleanContainer(_journalSheetListContainer);
		CleanMissionInfo();
	}
	
	private void CleanContainer(Transform container)
	{
		foreach (Transform child in container)
			Destroy(child.gameObject);
	}
	
	private void CleanMissionInfo()
	{
		_iconField.gameObject.SetActive(false);
		_missionNameText.text = null;
		_missionDescriptionText.text = null;
	}
	
	private void RenderMissionList()
	{
		missionCellList = new List<MissionCell>();
		
		foreach (Mission mission in TurnController.currentPlayer.missionList)
		{
			MissionCell cell = Instantiate(cellPref, _missionListContainer);
			missionCellList.Add(cell);
			cell.Init(mission);
		}
	}
	
	private void RenderJournalSheetList()
	{
		journalCellList = new List<MissionCell>();
		
		foreach (JournalSheet sheet in TurnController.currentPlayer.journalSheetList)
		{
			MissionCell cell = Instantiate(cellPref, _journalSheetListContainer);
			journalCellList.Add(cell);
			cell.Init(sheet);
		}
	}
	
	private void SelectMissionCell(MissionCell missionCell)
	{
		foreach (MissionCell cell in missionCellList)
			cell.Deselect();
		
		foreach (MissionCell cell in journalCellList)
			cell.Deselect();
		
		missionCell.Select();
		RenderMissionInfo(missionCell);
	}
	
	private void RenderMissionInfo(MissionCell missionCell)
	{
		if (missionCell.mission != null)
		{
			_iconField.gameObject.SetActive(false);
			_missionNameText.text = missionCell.mission.Name;
			_missionDescriptionText.text = missionCell.mission.description;
		}
		else
		{
			_iconField.sprite = missionCell.journalSheet.icon;
			_missionNameText.text = missionCell.journalSheet.name;
			_missionDescriptionText.text = missionCell.journalSheet.description;

			if (missionCell.journalSheet.icon != null)
				_iconField.gameObject.SetActive(true);
			else
				_iconField.gameObject.SetActive(false);
		}
	}
}
