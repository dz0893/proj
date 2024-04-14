using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;

public class PlayerObjectController : NetworkBehaviour
{
	[SerializeField] private NetworkMapComandLouncher _commandLouncher;
	public NetworkMapComandLouncher commandLouncher => _commandLouncher;

	[SyncVar] public int connectionID;
	[SyncVar] public int playerIDNumber;
	[SyncVar] public ulong playerSteamID;
	[SyncVar(hook = nameof(PlayerNameUpdate))] public string playerName;
	[SyncVar(hook = nameof(PlayerReadyUpdate))] public bool isReady;

	[SyncVar(hook = nameof(RaceIndexUpdate))] public int raceIndex;
	[SyncVar(hook = nameof(HeroIndexUpdate))] public int heroIndex;
	[SyncVar(hook = nameof(TeamUpdate))] public int team;
	[SyncVar(hook = nameof(PlayerStateUpdate))] public int playerState;

	public bool inAction;

	[SyncVar] public int currentRaceIndex;
	[SyncVar] public int currentHeroIndex;

	private int raceCount = Enum.GetValues(typeof(Race)).Length;
	private int heroCount = 3;
	private System.Random random = new System.Random();
	
	public PlayerListItem playerListItem { get; private set; }
	
	public CustomNetworkManager manager { get; private set; }
	
	public Player player { get; private set; } = new Player();
	
	public List<TerrainType> mapCellIndexList = new List<TerrainType>();

	private CustomNetworkManager Manager
	{
		get
		{
			if (manager != null)
				return manager;
			
			return manager = CustomNetworkManager.singleton as CustomNetworkManager;
		}
	}

	[ClientRpc]
	private void RpcAddIndexToList(TerrainType terrainType)
	{
		mapCellIndexList.Add(terrainType);
	}

	public void SetMapGroundTypeIndexList(BattleMap map)
	{
		mapCellIndexList = new List<TerrainType>();

		foreach (Transform cellObject in map.groundMap.transform)
		{
			GroundCell groundCell = cellObject.GetComponent<GroundCell>();

			TerrainType terrainType;
			if (groundCell.isRandomCell)
			{
				terrainType = RandomGroundCellGetter.instance.GetRandomTerrainType(groundCell.terrainTypeList);;
			}
			else
			{
				terrainType = groundCell.defaultTerrainType;
			}

			RpcAddIndexToList(terrainType);
		}
	}

	public void InitPlayer()
	{
		player = new Player();

		player.race = playerListItem.currentRace;
		player.hero = playerListItem.currentHero;
		player.capital = playerListItem.currentCapital;
	}

	public void SetPlayerListItem(PlayerListItem playerListItem)
	{
		this.playerListItem = playerListItem;
		playerListItem.controller = this;
		playerListItem.RenderHeroList();

		playerListItem.raceDropdown.onValueChanged.AddListener(delegate {RaceDropdownChanged(playerListItem.raceDropdown);});

		playerListItem.heroDropdown.onValueChanged.AddListener(delegate {HeroDropdownChanged(playerListItem.heroDropdown);});

		playerListItem.teamDropdown.onValueChanged.AddListener(delegate {TeamDropdownChanged(playerListItem.teamDropdown);});

		playerListItem.playerStateDropdown.onValueChanged.AddListener(delegate {PlayerStateDropdownChanged(playerListItem.playerStateDropdown);});
	}

	private void RaceDropdownChanged(Dropdown dropdown)
	{
		if (hasAuthority)
		{
			CmdSetRaceIndex(raceIndex, dropdown.value);
			
			if (playerListItem.isRandomRace)
				CmdSetCurrentHeroIndex();
			else
				heroIndex = 0;

			CmdSetCurrentRaceIndex();
		}
	}

	private void RaceIndexUpdate(int oldValue, int newValue)
	{
		if (isServer)
			this.raceIndex = newValue;
		
		if (isClient)
			LobbyController.instance.UpdatePlayerList();
	}

	[Command]
	private void CmdSetRaceIndex(int oldValue, int newValue)
	{
		this.RaceIndexUpdate(oldValue, newValue);
	}

	private void HeroDropdownChanged(Dropdown dropdown)
	{
		if (hasAuthority)
		{
			CmdSetHeroIndex(heroIndex, dropdown.value);
			CmdSetCurrentHeroIndex();
		}
	}

	private void HeroIndexUpdate (int oldValue, int newValue)
	{
		if (isServer)
			this.heroIndex = newValue;
		
		if (isClient)
			LobbyController.instance.UpdatePlayerList();
	}

	[Command]
	private void CmdSetHeroIndex(int oldValue, int newValue)
	{
		this.HeroIndexUpdate(oldValue, newValue);
	}

	private void TeamDropdownChanged(Dropdown dropdown)
	{
		if (hasAuthority)
			CmdSetTeam(team, dropdown.value);
	}

	private void TeamUpdate (int oldValue, int newValue)
	{
		if (isServer)
			this.team = newValue;
		
		if (isClient)
			LobbyController.instance.UpdatePlayerList();
	}

	[Command]
	private void CmdSetTeam(int oldValue, int newValue)
	{
		this.TeamUpdate(oldValue, newValue);
	}

	private void PlayerStateDropdownChanged(Dropdown dropdown)
	{
		if (hasAuthority)
			CmdSetPlayerState(playerState, dropdown.value);
	}

	private void PlayerStateUpdate(int oldValue, int newValue)
	{
		if (isServer)
			this.playerState = newValue;
		
		if (isClient)
			LobbyController.instance.UpdatePlayerList();
	}

	[Command]
	private void CmdSetPlayerState(int oldValue, int newValue)
	{
		this.PlayerStateUpdate(oldValue, newValue);
	}

	private void PlayerReadyUpdate(bool oldValue, bool newValue)
	{
		if (isServer)
			this.isReady = newValue;
		
		if (isClient)
			LobbyController.instance.UpdatePlayerList();
	}
	
	[Command]
	private void CmdSetPlayerReady()
	{
		this.PlayerReadyUpdate(this.isReady, !this.isReady);
	}
	
	public void ChangeReady()
	{
		if (hasAuthority)
			CmdSetPlayerReady();
	}
	
	public override void OnStartAuthority()
	{
		CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
		gameObject.name = "LocalGamePlayer";
		LobbyController.instance.FindLocalPlayer();
		LobbyController.instance.UpdateLobbyName();
	}
	
	public override void OnStartClient()
	{
		Manager.gamePlayers.Add(this);
		LobbyController.instance.UpdateLobbyName();
		LobbyController.instance.UpdatePlayerList();

		if (manager.gamePlayers.Count <= manager.maxPlayerCount)
			CmdSetTeam(team, manager.gamePlayers.IndexOf(this));
		else
			CmdSetPlayerState(playerState, 1);
	}
	
	public override void OnStopClient()
	{
		Manager.gamePlayers.Remove(this);
		LobbyController.instance.UpdatePlayerList();
	}
	
	[Command]
	private void CmdSetPlayerName(string playerName)
	{
		this.PlayerNameUpdate(this.playerName, playerName);
	}
	
	public void PlayerNameUpdate(string oldValue, string newValue)
	{
		if (isServer)
		{
			this.playerName = newValue;
		}
		if (isClient)
		{
			LobbyController.instance.UpdatePlayerList();
		}
	}

	/*[Server]
	public void CreateMap()
	{
		manager.CreateMap();
	}*/

	[ClientRpc]
	public void RpcCreateMap()
	{
		manager.CreateMap();
	}

	[ClientRpc]
	public void RpcSetMapParent()
	{
		manager.SetMapTransform();
	}

	[ClientRpc]
	public void RpcInitMap()
	{
		manager.InitMap();
	}

	[ClientRpc]
	public void RpcStartMatch()
	{
		manager.StartMatch();
	}

	[ClientRpc]
	public void RpcSetNetworkIndexList()
	{
		BattleMap.instance.networkCellIndexList = new List<TerrainType>(mapCellIndexList);
	}

	[Command]
	private void CmdSetCurrentRaceIndex()
	{
		if (playerListItem.isRandomRace)
		{
			currentRaceIndex = random.Next(raceCount);
		}
		else
		{
			currentRaceIndex = raceIndex;
		}
	}

	[Command]
	private void CmdSetCurrentHeroIndex()
	{
		if (playerListItem.isRandomHero)
		{
			currentHeroIndex = random.Next(heroCount);
		}
		else
		{
			currentHeroIndex = heroIndex;
		}
	}

	public void ToMainMenue()
	{
		if (isServer)
			RpcToMainMenue();
		else
			CmdToMainMenue();
	}

	[ClientRpc]
	private void RpcToMainMenue()
	{
		MainMenu.reload.Invoke();
	}

	[Command]
	private void CmdToMainMenue()
	{
		RpcToMainMenue();
	}
	
	public void SetActionState(bool state)
	{
		if (isServer)
			RpcSetActionState(state);
		else
			CmdSetActionState(state);
	}

	[ClientRpc]
	public void RpcSetActionState(bool state)
	{
		LobbyController.instance.SetActionControllerStateToCurrentClient(state, this.playerIDNumber);
	}

	[Command]
	public void CmdSetActionState(bool state)
	{
		RpcSetActionState(state);
	}
}
