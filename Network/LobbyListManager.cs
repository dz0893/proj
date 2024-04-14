using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class LobbyListManager : MonoBehaviour
{
	[SerializeField] private LobbyDataEntry _lobbyItemPref;
	[SerializeField] private GameObject _lobbieMenu;
	[SerializeField] private Transform _lobbyListContent;
	
	public List<LobbyDataEntry> lobbyList { get; set; } = new List<LobbyDataEntry>();
	
	public void DisplayLobbies(List<CSteamID> lobbyIDList, LobbyDataUpdate_t result)
	{
		for (int i = 0; i < lobbyIDList.Count; i++)
		{
			if (lobbyIDList[i].m_SteamID == result.m_ulSteamIDLobby)
			{
				LobbyDataEntry currentLobby = Instantiate(_lobbyItemPref, _lobbyListContent);
				
				currentLobby.SetLobbyData((CSteamID)lobbyIDList[i].m_SteamID);
				
				lobbyList.Add(currentLobby);
			}
		}
	}
	
	public void CleanLobbyList()
	{
		foreach (LobbyDataEntry lobby in lobbyList)
			Destroy(lobby.gameObject);
		
		lobbyList.Clear();
	}
}
