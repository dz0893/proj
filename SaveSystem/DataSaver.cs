using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DataSaver : MonoBehaviour
{
	[SerializeField] private DataReader _dataReader;
	[SerializeField] private SaveSlot _slotPrefab;
	[SerializeField] private Transform _slotContainer;
	[SerializeField] private GameObject _background;
	[SerializeField] private GameObject _createNewSlotButton;
	[SerializeField] private GameObject _deleteSlotView;
	[SerializeField] private NewSaveSlotView _newSaveSlotView;
	[SerializeField] private Button _saveButton;
	[SerializeField] private Button _loadButton;
	[SerializeField] private Button _deleteButton;
	[SerializeField] private Text _deleteViewTextField;
	
	private bool isLoading;
	
	private DirectoryInfo dir;
	private SaveSlot selectedSlot;
	
	public string selectedSlotName => selectedSlot.Name;
	private static string path;
	public static bool GameInLoad;
		
	public delegate void SaveGame(string name);
	public delegate void LoadGame(string name);
	public delegate void SetSelectedSlot(SaveSlot slot);
	public delegate void DeselectSlots();
	public delegate void CloseMenu();
	
	public static SaveGame save;
	public static LoadGame load;
	public static SetSelectedSlot setSelectedSlot;
	public static CloseMenu close;
	
	private void Start()
	{
		path = Directory.GetCurrentDirectory() + "/Saves/";
		dir = new DirectoryInfo(@path);
		save = Save;
		load = Load;
		setSelectedSlot = SelectSlot;
		close = Close;

		if (!dir.Exists)
			dir.Create();
	}
	
	public void OpenNewSaveSlotView()
	{
		_deleteSlotView.SetActive(false);
		_newSaveSlotView.gameObject.SetActive(true);
	}
	
	public void OpenDeleteSlotView()
	{
		_deleteSlotView.SetActive(true);
		_newSaveSlotView.gameObject.SetActive(false);
	}
	
	public void CloseViews()
	{
		_newSaveSlotView.gameObject.SetActive(false);
		_deleteSlotView.SetActive(false);
	}
	
	public void OpenSaveMenu()
	{
		_background.SetActive(true);
		_createNewSlotButton.SetActive(true);
		
		RenderMenu(false);
	}
	
	public void OpenLoadMenu()
	{
		_background.SetActive(true);
		_createNewSlotButton.SetActive(false);
		
		RenderMenu(true);
	}
	
	public void Close()
	{
		_background.SetActive(false);
	}
	
	private void CloseAll()
	{
		CloseViews();
		MainMenu.close.Invoke();
		MatchMenue.close.Invoke();
	}
	
	private void RenderMenu(bool isLoadingMenu)
	{
		isLoading = isLoadingMenu;
		ClearContainer();
		InitContainer(isLoadingMenu);
		SetButtonState(selectedSlot != null);
		
		_saveButton.gameObject.SetActive(!isLoadingMenu);
		_loadButton.gameObject.SetActive(isLoadingMenu);
	}
	
	private void SetButtonState(bool state)
	{
		_saveButton.interactable = state;
		_loadButton.interactable = state;
		_deleteButton.interactable = state;
	}
	
	private void InitContainer(bool isLoadingMenu)
	{
		int i = 1;
		
		foreach (FileInfo file in dir.GetFiles())
		{
			SaveSlot saveSlot = Instantiate(_slotPrefab, _slotContainer);
			
			saveSlot.SetLoadingState(isLoadingMenu);
			saveSlot.Render(i, file.Name);
			i++;
		}
	}
	
	private void ClearContainer()
	{
		foreach(Transform child in _slotContainer)
			Destroy(child.gameObject);
	}
	
	private void SelectSlot(SaveSlot saveSlot)
	{
		foreach(Transform slot in _slotContainer)
			slot.GetComponent<SaveSlot>().UnselectSlot();
		
		selectedSlot = saveSlot;
		SetButtonState(true);
		
		_deleteViewTextField.text = UISettings.wantToDeleteSlot + selectedSlotName;
	}
	
	public void DeleteSlot()
	{
		if (selectedSlot != null)
		{
			File.Delete(path + selectedSlot.Name);
			RenderMenu(isLoading);
			CloseViews();
		}
	}
	
	public void Save(string fileName)
	{
		Debug.Log("Save");
		CloseAll();
		TotalSaveInfo totalSaveInfo = new TotalSaveInfo(BattleMap.instance);
		
		File.WriteAllText(path + fileName, JsonUtility.ToJson(totalSaveInfo));
	}
	
	public void Load(string fileName)
	{
		Debug.Log("Load");
		GameInLoad = true;
		CloseAll();
		LoadMap(fileName);
		GameInLoad = false;
	}
	
	private void LoadMap(string fileName)
	{
		Debug.Log("LoadMap");
		if (BattleMap.instance != null)
			BattleMap.instance.EndGame();
		
		TotalSaveInfo totalSaveInfo = JsonUtility.FromJson<TotalSaveInfo>(File.ReadAllText(path + fileName));
		
		BattleMap map = DataBase.instance.GetMap(totalSaveInfo.mapSaveInfo.name);
		_dataReader.Read(totalSaveInfo, map);
	}
	
	public void PushSaveButton()
	{
		Save(selectedSlot.Name);
	}
	
	public void PushLoadButton()
	{
		Load(selectedSlot.Name);
	}
}
