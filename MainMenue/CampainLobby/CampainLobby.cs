using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampainLobby : MonoBehaviour
{
	[SerializeField] private MainMenu _mainMenu;
	[SerializeField] private CampainSaveSystem _campainSaveSystem;
	[SerializeField] private MissionScreen _missionScreen;

	[SerializeField] private Text _mapName;
	[SerializeField] private Image _mapIcon;

	[SerializeField] private List<BattleMap> _humanMapList;
	[SerializeField] private List<BattleMap> _orcMapList;
	[SerializeField] private List<BattleMap> _elfMapList;
	[SerializeField] private List<BattleMap> _undeadMapList;
	[SerializeField] private List<BattleMap> _dwarfMapList;

	public List<BattleMap> totalMapList {get; private set; }

	public List<BattleMap> humanMapList => _humanMapList;
	public List<BattleMap> orcMapList => _orcMapList;
	public List<BattleMap> elfMapList => _elfMapList;
	public List<BattleMap> undeadMapList => _undeadMapList;
	public List<BattleMap> dwarfMapList => _dwarfMapList;
	
	public List<BattleMap> currentMapList { get; private set; }
	
	[SerializeField] private Transform _background;
	[SerializeField] private Transform _mapContainer;
	[SerializeField] private List<Button> _campainButtonList;
	[SerializeField] private List<MissionButton> _missionButtonList;
	
	public BattleMap currentMap {get; private set; }
	
	private List<Player> playerList;

	public delegate void OpenEpilogueScreen();
	public delegate void InitCurrentMapOnStartingMatch(BattleMap map);

	public static OpenEpilogueScreen openEpilogueScreen;
	public static InitCurrentMapOnStartingMatch initCurrentMapOnStartingMatch;

	private void Start()
	{
		openEpilogueScreen = EndMission;
		initCurrentMapOnStartingMatch = RewriteCurrentMapOnStattingGame;
		InitTotalMapList();
	}

	private void InitTotalMapList()
	{
		totalMapList = new List<BattleMap>();
		AttMapListToTotal(_humanMapList);
		AttMapListToTotal(_orcMapList);
		AttMapListToTotal(_elfMapList);
		AttMapListToTotal(_undeadMapList);
		AttMapListToTotal(_dwarfMapList);
	}

	private void RewriteCurrentMapOnStattingGame(BattleMap map)
	{
		foreach (BattleMap battleMap in totalMapList)
		{
			if (battleMap.Name.Equals(map.Name))
			{
				currentMap = battleMap;
			}
		}
	}

	private void AttMapListToTotal(List<BattleMap> mapList)
	{
		foreach (BattleMap map in mapList)
			totalMapList.Add(map);
	}

	public void Open()
	{
		_background.gameObject.SetActive(true);
		_missionScreen.gameObject.SetActive(false);

		SetMapList(_campainSaveSystem.GetLastCampainIndex());
		SelectMission(_missionButtonList[_campainSaveSystem.GetLastMissionIndex()]);
	}

	public void Close()
	{
		_background.gameObject.SetActive(false);
	}

	public void SetMapList(int listIndex)
	{
		if (listIndex == 0)
			currentMapList = _humanMapList;
		else if (listIndex == 1)
			currentMapList = _orcMapList;
		else if (listIndex == 2)
			currentMapList = _elfMapList;
		else if (listIndex == 3)
			currentMapList = _undeadMapList;
		else
			currentMapList = _dwarfMapList;
		
		RenderButtons(listIndex);
	}
	
	private void RenderButtons(int campainIndex)
	{
		RenderCampainButtons();
		RenderMissionButtons(campainIndex);
	}
	private void RenderMissionButtons(int campainIndex)
	{
		foreach (MissionButton button in _missionButtonList)
			button.gameObject.SetActive(false);

		int countOfEnabledButtons = 1;

		foreach (CampainMapSaveInfo saveInfo in _campainSaveSystem.storySaveInfo.campainSaveInfoList[campainIndex].mapInfoList)
		{
			if (saveInfo.isCompleted)
				countOfEnabledButtons++;
			else
				break;
		}

		if (countOfEnabledButtons > currentMapList.Count)
			countOfEnabledButtons = currentMapList.Count;
		
		for (int i = 0; i < countOfEnabledButtons; i++)
		{
			_missionButtonList[i].gameObject.SetActive(true);
			_missionButtonList[i].SetMission(this, i);
		}
	}
	
	private void RenderCampainButtons()
	{
		for (int i = 0; i < _campainButtonList.Count; i++)
		{
			_campainButtonList[i].gameObject.SetActive(false);
		}

		int countOfEnabledButtons = 1;

		foreach (CampainSaveInfo saveInfo in _campainSaveSystem.storySaveInfo.campainSaveInfoList)
		{
			if (saveInfo.campainEnded)
				countOfEnabledButtons++;
			else
				break;
		}

		if (countOfEnabledButtons > _campainButtonList.Count)
			countOfEnabledButtons = _campainButtonList.Count;
		
		for (int i = 0; i < countOfEnabledButtons; i++)
		{
			_campainButtonList[i].gameObject.SetActive(true);
		}
	}

	public void SelectMission(MissionButton button)
	{
		currentMap = button.map;
		
		_mapName.text = button.missionName;
		_mapIcon.sprite = button.missionIcon;
		/*
		_mapDescription.text = button.missionPrologue;
		_mapGoal.text = "";// UISettings.Goal + button.missionGoal;
		*/
	}

	public void StartMission()
	{
		_missionScreen.RenderPrologue(currentMap.GetComponent<Scenario>());
		_missionScreen.gameObject.SetActive(true);
	}

	public void StartNextMission()
	{
		currentMap = totalMapList[totalMapList.IndexOf(currentMap) + 1];
		StartMission();
	}

	private void EndMission()
	{
		_mainMenu.background.SetActive(false);
		_background.gameObject.SetActive(true);
		_missionScreen.RenderEpilogue(currentMap.GetComponent<Scenario>());
		_missionScreen.gameObject.SetActive(true);
	}
	
	public void StartGame()
	{
		BattleMap map = Instantiate(currentMap, _mapContainer);
		InitPlayers(map.GetComponent<CampainMapSettings>());
		map.Init(playerList, false);
		_campainSaveSystem.SetHeroLevel(map.GetComponent<CampainMapSettings>());
		map.StartMatch();
		_background.gameObject.SetActive(false);
	}
	
	private void InitPlayers(CampainMapSettings mapSettings)
	{
		playerList = new List<Player>();
		
		for (int i = 0; i < mapSettings.capitalList.Count; i++)
		{
			Player player = new Player();
			
			player.capital = mapSettings.capitalList[i];
				
			if (player.capital != null)
				player.race = player.capital.race;
			
			player.hero = mapSettings.heroList[i];
			
			player.InitGlobals(mapSettings.GetPlayerGlobals(player));

			if (i == mapSettings.playerIndex)
				player.SetNotComputerPlayer();
			
			else
				player.SetAI(mapSettings.aiStateList[i]);
			
			player.Init(mapSettings.teamList[i]);
			
			playerList.Add(player);
		}
	}
}
