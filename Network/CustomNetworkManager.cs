using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
	[SerializeField] private PlayerObjectController _playerObjectPref;
	
	[SerializeField] private BattleMap _map;
	public int maxPlayerCount => _map.countOfPlayers;

	public BattleMap battleMap { get; private set; }

	private Transform MapContainer => RandomGroundCellGetter.instance.transform;

	public List<PlayerObjectController> gamePlayers { get; } = new List<PlayerObjectController>();
	public PlayerObjectController localPlayer { get; private set; }
	public static int localPlayerId;
	
	public static bool IsOnlineGame
	{
		get
		{
			if (BattleMap.instance == null || BattleMap.instance.networkCellIndexList.Count == 0)
				return false;
			else
				return true;
		}
	}

	public override void OnServerAddPlayer(NetworkConnectionToClient conn)
	{
		PlayerObjectController gamePlayerInstance = Instantiate(_playerObjectPref);
		gamePlayerInstance.connectionID = conn.connectionId;
		gamePlayerInstance.playerIDNumber = gamePlayers.Count + 1;
		gamePlayerInstance.playerSteamID = (ulong) SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, gamePlayers.Count);
			
		NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
	}
	
	public void CreateMap()
	{
		battleMap = Instantiate(_map, MapContainer);
	//	NetworkServer.Spawn(battleMap.gameObject);
	}

	public void SetMapTransform()
	{
		BattleMap.instance.transform.SetParent(MapContainer);
		BattleMap.instance.transform.localScale = new Vector3(1f,1f,1f);
		BattleMap.instance.transform.localPosition = new Vector3(0,0,0);
	}

	public void InitMap()
	{
		LobbyController.instance.InitPlayerList();
		BattleMap.instance.Init(LobbyController.instance.playerList, true);
	}

	public void StartMatch()
	{
		localPlayer = LobbyController.localPlayerController;
		BattleMap.instance.StartMatch();
		LobbyController.instance.gameObject.SetActive(false);
	}

	public void StartHostCustom()
	{
		MainMenu.openSteamLobby.Invoke();
		StartHost();
	}
	
	public void StartClientCustom()
	{
		MainMenu.openSteamLobby.Invoke();
		StartClient();
	}
	
	public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
	//	localPlayer.commandLouncher.DestroyPlayer(GetPlayerId(conn.connectionId));
	}

	public override void OnStopHost()
	{
		Debug.Log("OnStopHost");
	}

	public override void OnStopServer()
	{
		Debug.Log("OnStopServer");
	}

	public override void OnStopClient()
	{
		Debug.Log("OnStopClient");
		
	//	MainMenu.reload.Invoke();
	}
	
	public override void OnClientDisconnect()
    {
		Debug.Log("disconnect");
		
        if (mode == NetworkManagerMode.Offline)
            return;

        StopClient();
    }

	public override void OnClientDisconnect(NetworkConnection conn)
    {
		Debug.Log("onClientDisconnect " + conn.connectionId);
	//	localPlayer.commandLouncher.DestroyPlayer(GetPlayerId(conn.connectionId));

        if (mode == NetworkManagerMode.Offline)
            return;

        StopClient();
    }

	public override void OnClientError(Exception exception)
	{
		Debug.Log("clientError");
	}

	
	private int GetPlayerId(int connectionId)
	{
		int playerId = 0;

		foreach (PlayerObjectController objectController in gamePlayers)
		{
			if (connectionId == objectController.connectionID)
			{
				Debug.Log("new1 " + connectionId);
				Debug.Log("new2 " + objectController.player.id);
				playerId = objectController.player.id;
			}
		}

		Debug.Log("playerId " + playerId);
		return playerId;
	}
	
	// в родительском классе были закоменчены строки 549 и 590
}
