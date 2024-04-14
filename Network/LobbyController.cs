using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
	public static LobbyController instance;
	
	[SerializeField] private Text _lobbyNameText;
	[SerializeField] private Text _readyButtonText;
	[SerializeField] private Button _startButton;
	[SerializeField] private Button _readyButton;
//	public Button readyButton => _readyButton;
	
	[SerializeField] private Transform _playerListViewContent;
	[SerializeField] private PlayerListItem _playerListItemPref;

	public GameObject localPlayerObject { get; private set; }
	
	public ulong currentLobbyID;
	public bool playerItemCreated;
	
	public List<PlayerListItem> playerListItems = new List<PlayerListItem>();
	
	public static PlayerObjectController localPlayerController { get; private set; }

	public List<Player> playerList { get; private set; } = new List<Player>();

	public CustomNetworkManager manager { get; private set; }
	
	private CustomNetworkManager Manager
	{
		get
		{
			if (manager != null)
				return manager;
			
			return manager = CustomNetworkManager.singleton as CustomNetworkManager;
		}
	}
	
	public void InitPlayerList()
	{
		playerList = new List<Player>();

		foreach (PlayerListItem playerListItem in playerListItems)
		{
			if (playerListItem.playerStateDropdown.value == 0)
			{
				playerListItem.SetPlayerContent();
				playerList.Add(GetCurrentPlayer(playerListItem));
			}
		}
		
		CustomNetworkManager.localPlayerId = localPlayerController.playerIDNumber;
	}

	private Player GetCurrentPlayer(PlayerListItem playerListItem)
	{
		Player currentPlayer = new Player();

		currentPlayer.race = playerListItem.player.race;
		currentPlayer.hero = playerListItem.player.hero;
		currentPlayer.capital = playerListItem.player.capital;
		currentPlayer.InitGlobals(currentPlayer.hero.heroGlobalActionList);
		currentPlayer.id = playerListItem.controller.playerIDNumber;
		currentPlayer.nickname = playerListItem.playerName;

		currentPlayer.Init(playerListItem.controller.team);

		return currentPlayer;
	}

	private void Awake()
	{
		if (instance == null)
			instance = this;
	}
	
	public void PressStartButton()
	{
		/*
		localPlayerController.CreateMap();
		localPlayerController.SetMapGroundTypeIndexList(BattleMap.instance);
		BattleMap.instance.RpcSetNetworkIndexList(localPlayerController);
		localPlayerController.RpcSetMapParent();
		localPlayerController.RpcInitMap();
		localPlayerController.RpcStartMatch();
		*/

		StartCoroutine(CreatingMapCoroutine());
	}

	public IEnumerator CreatingMapCoroutine()
	{
		localPlayerController.RpcCreateMap();
		
		while (BattleMap.instance == null)
			yield return null;

		localPlayerController.SetMapGroundTypeIndexList(BattleMap.instance);
		localPlayerController.RpcSetNetworkIndexList();
		localPlayerController.RpcSetMapParent();
		localPlayerController.RpcInitMap();
		localPlayerController.RpcStartMatch();
		
		yield return null;
	}

	public void PlayerReady()
	{
		localPlayerController.ChangeReady();
	}
	
	public void UpdateButton()
	{
		if (localPlayerController.isReady)
			_readyButtonText.text = "Unready";
		else
			_readyButtonText.text = "Ready";
	}
	
	public void ChekIfAllReady()
	{
		bool allReady = false;
		int countOfPlayers = 0;

		foreach (PlayerObjectController player in Manager.gamePlayers)
		{
			if(player.isReady)
			{
				allReady = true;
				countOfPlayers++;
			}
			else if (player.playerState == 1)
			{
				allReady = true;
			}
			else
			{
				allReady = false;
				break;
			}
		}
		
		if (allReady && countOfPlayers > 0 && countOfPlayers <= manager.maxPlayerCount)
		{
			if (localPlayerController.playerIDNumber == 1)
				_startButton.interactable = true;
			else
				_startButton.interactable = false;
		}
		else
		{
			_startButton.interactable = false;
		}
	}
	
	public void UpdateLobbyName()
	{
		currentLobbyID = Manager.GetComponent<SteamLobby>().currentLobbyID;
		_lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyID), "name");
	}
	
	public void UpdatePlayerList()
	{
		if (!playerItemCreated)
			CreateHostPlayerItem();
		
		if (playerListItems.Count < Manager.gamePlayers.Count)
			CreateClientPlayerItem();
		
		if (playerListItems.Count > Manager.gamePlayers.Count)
			RemovePlayerItem();
		
		if (playerListItems.Count == Manager.gamePlayers.Count)
			UpdatePlayerItem();
		
		SetInterractableState();
	}
	
	public void FindLocalPlayer()
	{
		localPlayerObject = GameObject.Find("LocalGamePlayer");
		localPlayerController = localPlayerObject.GetComponent<PlayerObjectController>();
	}
	
	public void CreateHostPlayerItem()
	{
		foreach (PlayerObjectController player in Manager.gamePlayers)
			CreateCurrentPlayerItem(player);
		
		playerItemCreated = true;
	}
	
	public void CreateClientPlayerItem()
	{
		foreach (PlayerObjectController player in Manager.gamePlayers)
		{
			if (!playerListItems.Any(b => b.connectionID == player.connectionID))
				CreateCurrentPlayerItem(player);
		}
	}
	
	private void CreateCurrentPlayerItem(PlayerObjectController playerController)
	{
		PlayerListItem newPlayerItem = Instantiate(_playerListItemPref, _playerListViewContent);
		
		playerController.SetPlayerListItem(newPlayerItem);
		newPlayerItem.playerName = playerController.playerName;
		newPlayerItem.connectionID = playerController.connectionID;
		newPlayerItem.playerSteamID = playerController.playerSteamID;
		
		newPlayerItem.InitTeamDropDown();
		newPlayerItem.Render();
		playerListItems.Add(newPlayerItem);
	}

	private void SetInterractableState()
	{
		foreach (PlayerObjectController player in Manager.gamePlayers)
		{
			foreach (PlayerListItem playerListItem in playerListItems)
			{
				if (playerListItem.controller.hasAuthority)
				{
					playerListItem.raceDropdown.interactable = true;
					playerListItem.heroDropdown.interactable = true;
					playerListItem.teamDropdown.interactable = true;
					playerListItem.playerStateDropdown.interactable = true;
				}
				else
				{
					playerListItem.raceDropdown.interactable = false;
					playerListItem.heroDropdown.interactable = false;
					playerListItem.teamDropdown.interactable = false;
					playerListItem.playerStateDropdown.interactable = false;
				}
			}
		}
	}
	
	public void UpdatePlayerItem()
	{
		foreach (PlayerListItem playerListItem in playerListItems)
		{
			playerListItem.playerName = playerListItem.controller.playerName;
			playerListItem.Render();
				
			if (playerListItem.controller == localPlayerController)
				UpdateButton();
		}

		ChekIfAllReady();
	}
	
	public void RemovePlayerItem()
	{
		List<PlayerListItem> listItemsToRemove = new List<PlayerListItem>();
		
		foreach (PlayerListItem playerListItem in playerListItems)
		{
			if (!Manager.gamePlayers.Any(b => b.connectionID == playerListItem.connectionID))
				listItemsToRemove.Add(playerListItem);
		}
		if (listItemsToRemove.Count > 0)
		{
			foreach (PlayerListItem playerListItem in listItemsToRemove)
			{
				GameObject obj = playerListItem.gameObject;
				playerListItems.Remove(playerListItem);
			
				Destroy(obj);
				obj = null;
			}
		}
	}
	
	public void LeaveLobby()
	{
		SteamMatchmaking.LeaveLobby((CSteamID)currentLobbyID);
		manager.StopHost();
		MainMenu.reload.Invoke();
	}

	public void SetActionControllerStateToCurrentClient(bool state, int playerId)
	{
		foreach (PlayerListItem playerListItem in playerListItems)
		{
			if (playerListItem.controller.playerIDNumber == playerId)
			{
				playerListItem.controller.inAction = state;
			}
		}
	}
}
