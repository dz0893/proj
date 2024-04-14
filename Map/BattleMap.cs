using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Mirror;

public class BattleMap : MonoBehaviour
{
	private System.Random random = new System.Random();
	ActionRender actionRender = new ActionRender();
	
	public AIBehaviorSetter aiBehaviorSetter { get; set; }
	
	private bool isCampainMap => GetComponent<CampainMapSettings>();
	
	[SerializeField] private string _name;
	public string Name => _name;
	
	[SerializeField] private int _countOfPlayers;
	public int countOfPlayers => _countOfPlayers;
	
	[SerializeField] private List<StartedPosition> _capitalPositions;
	[SerializeField] private List<StartedPosition> _heroPositions;
	
	public GroundFactory groundFactory { get; private set; } = new GroundFactory();
		
	[SerializeField] private Tilemap _groundMap;
	public Tilemap groundMap => _groundMap;
	
	[SerializeField] private Tilemap _objectMap;
	public Tilemap objectMap => _objectMap;
	
	public TurnController turnController { get; private set; } = new TurnController();
	
	public List<GroundCell> mapCellList { get; private set; } = new List<GroundCell>();
	
	public List<GroundCell> leftDepositCellList { get; private set; } = new List<GroundCell>();
	public List<GroundCell> rightDepositCellList { get; private set; } = new List<GroundCell>();
	
	public List<GroundCell> neDepositCellList { get; private set; } = new List<GroundCell>();
	public List<GroundCell> seDepositCellList { get; private set; } = new List<GroundCell>();
	public List<GroundCell> nwDepositCellList { get; private set; } = new List<GroundCell>();
	public List<GroundCell> swDepositCellList { get; private set; } = new List<GroundCell>();
	
	public List<NullObject> objectList { get; set; } = new List<NullObject>();
	
	public List<Player> playerList { get; private set; }
	
	public delegate void RenderRoad(GroundCell goal);
	public delegate void RendertAreaOfAction(Unit unit, ActionData action);
	public delegate void CleanRoad();
	public delegate void CleanArea();
	public delegate NullObject InitObject(NullObject obj, Player player, GroundCell position);
	
	public static RenderRoad renderRoad;
	public static RendertAreaOfAction rendertAreaOfAction;
	public static CleanRoad cleanRoad;
	public static CleanArea cleanArea;
	public static InitObject initObject;
	
	public static BattleMap instance { get; private set; }
	
	public NullObject selectedObject { get; set; }
	public NullObject lastSelectedObject { get; set; }
	
	public GroundCell topMapBoard { get; private set; }
	public GroundCell bottomMapBoard { get; private set; }
	public GroundCell leftMapBoard { get; private set; }
	public GroundCell rightMapBoard { get; private set; }
	
	public List<TerrainType> networkCellIndexList { get; set; } = new List<TerrainType>();
	
	public Unit SelectedUnit
	{
		get
		{
			if (selectedObject != null && selectedObject is Unit)
				return selectedObject as Unit;
			else
				return null;
		}
	}
	
	public void LoadetInit(List<Player> playerList)
	{
		GameOptions.setDefaultOnLoadScale.Invoke();
		SetDelegates();
		LoadetInitMap(playerList);
		
		CampainLobby.initCurrentMapOnStartingMatch.Invoke(this);
		CameraController.init.Invoke();
	}
	
	public void Init(List<Player> playerList, bool simmetricalMap)
	{
		SetDelegates();
		InitMap(playerList, simmetricalMap);
		MiniMap.init.Invoke(this);
		
		CameraController.init.Invoke();
		GameOptions.setScale.Invoke();
	}

	[Server]
	public void NetworkInit(List<Player> playerList, bool simmetricalMap)
	{
		SetDelegates();
		InitMap(playerList, simmetricalMap);
		MiniMap.init.Invoke(this);
		
		CameraController.init.Invoke();
		GameOptions.setScale.Invoke();
	}
	
	public void SetDelegates()
	{
		cleanRoad = RenderSelectedUnitActionRange;
		renderRoad = RenderSelectedUnitRoad;
		rendertAreaOfAction = RenderSelectedActionArea;
		cleanArea = CleanSelectedActionArea;
		initObject = CreateObject;
	}
	
	private NullObject CreateObject(NullObject obj, Player player, GroundCell position)
	{
		NullObject createdObject = Instantiate(obj, objectMap.transform);
		UnmaterialObject unmaterialOnCellObject = position.unmaterialOnCellObject;
		
		createdObject.Init(position, player);
		
		createdObject.EndTurn();
		
		if (unmaterialOnCellObject != null)
			unmaterialOnCellObject.ContactWithOtherObject(createdObject);
		
		return createdObject;
	}
	
	public List<GroundCell> GetAllCellsWithIndex(int index)
	{
		List<GroundCell> cellList = new List<GroundCell>();
		
		foreach (GroundCell cell in mapCellList)
		{
			if (cell.index == index)
				cellList.Add(cell);
		}
		
		return cellList;
	}
	
	public GroundCell GetCellWithIndex(int index)
	{
		foreach (GroundCell cell in mapCellList)
		{
			if (cell.index == index)
				return cell;
		}
		
		return null;
	}
	
	public Player GetPlayerWithID(int id)
	{
		foreach (Player player in playerList)
		{
			if (player.id == id)
				return player;
		}

		Debug.Log("ALARM NO PLAYER WITH ID " + id);
		return null;
	}
	
	private void RenderSelectedUnitRoad(GroundCell goal)
	{
		if (SelectedUnit != null && !SelectedUnit.turnEnded)
		{
			SelectedUnit.RenderUnitRoad(goal);
			UnitActionRender(goal);
		}
		else if (MapController.selectedGlobalAction != null)
		{
			GlobalActionRender(goal);
		}
	}
	
	private void UnitActionRender(GroundCell target)
	{
		if (SelectedUnit.currentActionTargetList.Contains(target))
			actionRender.Render(SelectedUnit.GetActionData(SelectedUnit.choosenAction), target);
			
		else if (SelectedUnit.moveActionTargetList.Contains(target))
			actionRender.Render(SelectedUnit.GetActionData(SelectedUnit.moveAction), target);
		else
			actionRender.Clean();
	}
	
	private void GlobalActionRender(GroundCell target)
	{
		if (MapController.selectedGlobalAction.areaList.Contains(target))
			actionRender.Render(MapController.selectedGlobalAction, target);
		else
			actionRender.Clean();
	}
	
	private void RenderSelectedUnitActionRange()
	{
		foreach (GroundCell cell in mapCellList)
			cell.targetCell.Clean();
			
		if (SelectedUnit != null && !SelectedUnit.turnEnded)
			SelectedUnit.RenderActionDistance();
	}

	private void RenderSelectedActionArea(Unit unit, ActionData actionData)
	{
		List<GroundCell> cellList = actionData.abstractAction.GetAreaOfAction(unit);

		foreach (GroundCell cell in cellList)
		{
			cell.OnPossibleTargetSelecter();
		}
	}

	private void CleanSelectedActionArea()
	{
		foreach (GroundCell cell in mapCellList)
		{
			cell.OffPossibleTargetSelecter();
		}
	}
	
	public void EndGame()
	{
		if (CustomNetworkManager.IsOnlineGame)
			CustomNetworkManager.singleton.StopHost();

		if (GetComponent<Scenario>() != null)
			DialogeUI.close.Invoke();
			
		turnController.CleanCash();
		CameraController.offCameraController.Invoke();
		PlayerUI.unlockInput.Invoke();
		MatchMenue.close.Invoke();
		AudioManager.setMainMenuMusic.Invoke();
		CustomNetworkManager.localPlayerId = 0;

		GameOptions.setDefaultMapScale.Invoke();
		MiniMap.cleanMiniMap.Invoke();

		Destroy(gameObject);
	}
	
	public void StartMatch()
	{
		turnController.InitMatch(turnController.playerList[0]);
		PlayerUI.initMatch.Invoke();
		
		AudioManager.setMusic.Invoke();
	}
	
	public void StartLoadetMatch(TotalSaveInfo totalSaveInfo)
	{
		turnController.InitLoadetMatch(turnController.playerList[totalSaveInfo.currentPlayerIndex], totalSaveInfo.turnCounter);
		PlayerUI.initMatch.Invoke();
		
		AudioManager.setMusic.Invoke();
	}
	
	public void LoadetInitMap(List<Player> playerList)
	{
		instance = this;
		
		MapController.setInstance.Invoke();
		
		InitPlayers(playerList);
		InitGroundMap();
		InitScenario(true);
	}
	
	private void Awake()
	{
		instance = this;
	}

	public void InitMap(List<Player> playerList, bool simmetricalMap)
	{
		MapController.setInstance.Invoke();
		
		InitPlayers(playerList);
		InitGroundMap();
		
		if (countOfPlayers == 2)
		{
			InitDeposits(rightDepositCellList);
			InitDeposits(leftDepositCellList);
		}
		else
		{
			InitDeposits(neDepositCellList);
			InitDeposits(nwDepositCellList);
			InitDeposits(seDepositCellList);
			InitDeposits(swDepositCellList);
		}
		
		if (simmetricalMap)
			SimmetrizateGroundMap();
		
		InitObjectMap();
		InitScenario(false);
	}
	
	public void ClearMapCash()
	{
		foreach (GroundCell cell in mapCellList)
			cell.CLearCellCash();
		
		actionRender.Clean();
	}
	
	public void InitGroundMap()
	{
		mapCellList = new List<GroundCell>();
		leftDepositCellList = new List<GroundCell>();
		rightDepositCellList = new List<GroundCell>();
		
		if (CustomNetworkManager.IsOnlineGame)
			SetGroundTypeInOnLine();
		else
			SetGroundTypeInOffLine();
		
		SetClosestCells();
	}

	private void SetGroundTypeInOffLine()
	{
		foreach (Transform child in groundMap.transform)
		{
			GroundCell groundCell = child.GetComponent<GroundCell>();
			groundCell.Init();
			
			TerrainType terrainType;
			
			if (groundCell.isRandomCell)
				terrainType = RandomGroundCellGetter.instance.GetRandomTerrainType(groundCell.terrainTypeList);
			else
				terrainType = groundCell.defaultTerrainType;
			
			groundCell.SetTerrainType(groundFactory.GetTerrain(terrainType));
			
			if (groundCell.isDepositCell)
				AddDepositToList(groundCell);
			
			mapCellList.Add(groundCell);
			SetBoardedCell(groundCell);
		}
	}
	
	private void SetGroundTypeInOnLine()
	{
		for (int i = 0; i < networkCellIndexList.Count; i++)
		{
			GroundCell groundCell = groundMap.transform.GetChild(i).GetComponent<GroundCell>();
			groundCell.Init();
			
			groundCell.SetTerrainType(groundFactory.GetTerrain(networkCellIndexList[i]));
			
			if (groundCell.isDepositCell)
				AddDepositToList(groundCell);
			
			mapCellList.Add(groundCell);
			SetBoardedCell(groundCell);
		}
	}

	private void AddDepositToList(GroundCell cell)
	{
		if (cell.transform.position.x > 0)
		{
			rightDepositCellList.Add(cell);
			
			if (cell.transform.position.y > 0)
				neDepositCellList.Add(cell);
			else
				seDepositCellList.Add(cell);
		}
		else if (cell.transform.position.x < 0)
		{
			leftDepositCellList.Add(cell);
			
			if (cell.transform.position.y > 0)
				nwDepositCellList.Add(cell);
			else
				swDepositCellList.Add(cell);
		}
	}
	
	private void InitDeposits(List<GroundCell> depositList)
	{
		if (leftDepositCellList.Count < (MapSettings.goldDepositsPerPlayer + MapSettings.oreDepositsPerPlayer))
		{
			Debug.Log("ALARM!!!! not enought deposit points");
			return;
		}
		
		InitCurrentDepostList(depositList, MapSettings.goldDepositsPerPlayer, TerrainType.GoldDeposit);
		InitCurrentDepostList(depositList, MapSettings.oreDepositsPerPlayer, TerrainType.OreDeposit);
		CleanGroundList(depositList);
	}
	
	private void InitCurrentDepostList(List<GroundCell> depositList, int count, TerrainType terrainType)
	{
		for (int i = 0; i < count; i++)
		{
			GroundCell cell = depositList[random.Next(depositList.Count)];
			
			cell.SetTerrainType(groundFactory.GetTerrain(terrainType));
			
			depositList.Remove(cell);
		}
	}
	
	private void CleanGroundList(List<GroundCell> groundList)
	{
		foreach (GroundCell cell in groundList)
			cell.SetTerrainType(groundFactory.GetTerrain(TerrainType.Grass));
	}
	
	private void SetBoardedCell(GroundCell cell)
	{
		if (topMapBoard == null || topMapBoard.transform.position.y < cell.transform.position.y)
			topMapBoard = cell;
		if (bottomMapBoard == null || bottomMapBoard.transform.position.y > cell.transform.position.y)
			bottomMapBoard = cell;
		if (leftMapBoard == null || leftMapBoard.transform.position.x > cell.transform.position.x)
			leftMapBoard = cell;
		if (rightMapBoard == null || rightMapBoard.transform.position.x < cell.transform.position.x)
			rightMapBoard = cell;
	}
	
	private void SetClosestCells()
	{
		for (int i = 0; i < mapCellList.Count; i++)
		{
			for (int j = 0; j < mapCellList.Count; j++)
			{
				if (CheckCellsForClosestPositions(mapCellList[i], mapCellList[j]))
				{
					mapCellList[i].AddCellToClosestCellList(mapCellList[j]);
				}
			}
		}
	}
	
	private bool CheckCellsForClosestPositions(GroundCell cell1, GroundCell cell2)
	{
		float dx = Math.Abs(cell1.transform.localPosition.x - cell2.transform.localPosition.x);
		float dy = Math.Abs(cell1.transform.localPosition.y - cell2.transform.localPosition.y);
		
		dx = (float)Math.Round(dx, 3, MidpointRounding.AwayFromZero);
		dy = (float)Math.Round(dy, 3, MidpointRounding.AwayFromZero);
		
		if (dx == GroundSettings.HORIZONTALDX && dy == GroundSettings.HORIZONTALDY)
			return true;
		else if (dx == GroundSettings.DIAGONALDX && dy == GroundSettings.DIAGONALDY)
			return true;
		else
			return false;
	}
	
	private void InitObjectMap()
	{
		if (isCampainMap)
			InitCampainObjectMap();
		
		InitSkirmishObjectMap();
	}
	
	public void CleanObjectMap()
	{
		foreach (Transform child in objectMap.transform)
		{
			Destroy(child.gameObject);
		}
	}
	
	private void InitCampainObjectMap()
	{
		foreach (Transform child in objectMap.transform)
		{
			float objX = (float)Math.Round(child.transform.position.x, 3, MidpointRounding.AwayFromZero);
			float objY = (float)Math.Round(child.transform.position.y, 3, MidpointRounding.AwayFromZero);
			
			foreach (GroundCell cell in mapCellList)
			{
				float cellX = (float)Math.Round(cell.transform.position.x, 3, MidpointRounding.AwayFromZero);
				float cellY = (float)Math.Round(cell.transform.position.y, 3, MidpointRounding.AwayFromZero);
				
				if (objX == cellX && objY == cellY)
				{
					NullObject obj = child.GetComponent<NullObject>();

					bool rotatedToRight = obj.spriteFlipped;
					Debug.Log(obj.Name);
					obj.Init(cell, playerList[obj.playerIndex]);

					if (rotatedToRight)
						obj.RotateToRight();
					else
						obj.RotateToLeft();
				}
			}
		}
	}
	
	private void InitSkirmishObjectMap()
	{
		foreach (Player player in playerList)
		{
			foreach (StartedPosition pos in _capitalPositions)
			{	
				if (player == null || player.capital == null)
					continue;
					
				if (pos.playerIndex == playerList.IndexOf(player) && playerList[pos.playerIndex] != null)
				{
					pos.Init(player.capital);
					player.capital = pos.obj as Capital;
				}
			}
			
			foreach (StartedPosition pos in _heroPositions)
			{	
				if (player == null || player.hero == null)
					continue;
					
				if (pos.playerIndex == playerList.IndexOf(player) && playerList[pos.playerIndex] != null)
				{
					pos.Init(player.hero);
					player.hero = pos.obj as Unit;
				}
			}
		}
	}
	
	private void InitScenario(bool isLoading)
	{
		GetComponent<Scenario>()?.Init(playerList, isLoading);
	}
	
	private void InitPlayers(List<Player> playerList)
	{
		this.playerList = new List<Player>();
		turnController.playerList = new List<Player>();
		
		foreach (Player player in playerList)
			AddNewPlayer(player);
		
		turnController.InitPlayerList();
	}
	
	public void AddNewPlayer(Player player)
	{
		playerList.Add(player);
		
		if (player == null || player.isDefeated)
			return;
		
		turnController.playerList.Add(player);
		SetPlyerColor();

		if (player.isAIPlayer)
			player.aiPlayer.Init();
		
		player.SetPlayerName();
	}
	
	public void SetPlyerColor()
	{
		playerList[playerList.Count - 1].SetColorWithIndex(playerList.Count - 1);
	}
	
	private void SimmetrizateGroundMap()
	{
		foreach (GroundCell cell in mapCellList)
		{
			float cellX = (float)Math.Round(cell.transform.position.x, 3, MidpointRounding.AwayFromZero);
			float cellY = (float)Math.Round(cell.transform.position.y, 3, MidpointRounding.AwayFromZero);

			if (cellX > 0)
			{
				foreach (GroundCell mirroredCell in mapCellList)
				{
					float mirroredCellX = (float)Math.Round(mirroredCell.transform.position.x, 3, MidpointRounding.AwayFromZero);
					float mirroredCellY = (float)Math.Round(mirroredCell.transform.position.y, 3, MidpointRounding.AwayFromZero);

					if (mirroredCellX == -cellX && mirroredCellY == cellY)
					{
						mirroredCell.SetTerrainType(groundFactory.GetTerrain(cell.terrainType));
					}
				}
			}
		}
	}

	public void RenderObjectsState()
	{
		foreach (NullObject obj in objectList)
		{
			obj.RenderTurnState();

			if (selectedObject == obj)
				obj.objectRenderer.OnSelecter();
			else
				obj.objectRenderer.OffSelecter();
		}
	}
}