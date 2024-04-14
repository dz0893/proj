using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{
	[SerializeField] private LobbyListManager _lobbyListManager;

	public static SteamLobby instance;
	
	protected Callback<LobbyCreated_t> LobbyCreated;
	protected Callback<GameLobbyJoinRequested_t> JoinRequest;
	protected Callback<LobbyEnter_t> LobbyEntered;
	
	protected Callback<LobbyChatUpdate_t> LobbyChatUpdate;
	
	protected Callback<LobbyMatchList_t> LobbyList;
	protected Callback<LobbyDataUpdate_t> LobbyDataUpdated;
	
	public List<CSteamID> steamIDList = new List<CSteamID>();
	
	public ulong currentLobbyID;
	private const string hostAdressKey = "HostAdress";
	
	[SerializeField] private CustomNetworkManager _manager;
	public CustomNetworkManager manager => _manager;
	
	private void Start()
	{
		if (!SteamManager.Initialized)
			return;
		
		if (instance == null)
			instance = this;
		
		LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
		JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
		LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
		
		
		LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
		
		LobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
		LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
	}
	
	public void CreateLobby()
	{
		SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, manager.maxConnections);
	}
	
	private void OnLobbyChatUpdate(LobbyChatUpdate_t callback)
	{
		LobbyController.instance.UpdatePlayerList();
	}
	
	private void OnLobbyCreated(LobbyCreated_t callback)
	{
		if (callback.m_eResult != EResult.k_EResultOK)
			return;
		
		manager.StartHost();
		
		SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAdressKey, SteamUser.GetSteamID().ToString());
		SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString());
	}
	
	private void OnJoinRequest(GameLobbyJoinRequested_t callback)
	{
		SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
	}
	
	private void OnLobbyEntered(LobbyEnter_t callback)
	{
		currentLobbyID = callback.m_ulSteamIDLobby;
		

		MainMenu.openSteamLobby.Invoke();

		if (NetworkServer.active)
			return;
		
		manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), hostAdressKey);
		manager.StartClient();
	}
	
	public void OnGetLobbyList(LobbyMatchList_t callback)
	{
		if (steamIDList.Count > 0)
			_lobbyListManager.CleanLobbyList();
		
		for (int i = 0; i < callback.m_nLobbiesMatching; i++)
		{
			CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
			
			steamIDList.Add(lobbyID);
			SteamMatchmaking.RequestLobbyData(lobbyID);
		}
	}
	
	public void OnGetLobbyData(LobbyDataUpdate_t callback)
	{
		_lobbyListManager.DisplayLobbies(steamIDList, callback);
	}
	
	public void GetLobbyList()
	{
		steamIDList.Clear();
		SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
		SteamMatchmaking.RequestLobbyList();
	}
	
	public void JoinLobby(CSteamID lobbyID)
	{
		SteamMatchmaking.JoinLobby(lobbyID);
	}
}
