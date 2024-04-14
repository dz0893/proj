using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class LobbyDataEntry : MonoBehaviour
{
	[SerializeField] private Text _lobbyNameField;
	
	public CSteamID lobbyID;
	public string lobbyName;
	
	public void SetLobbyData(CSteamID steamID)
	{
		lobbyID = steamID;
		lobbyName = SteamMatchmaking.GetLobbyData(steamID, "name");
		
		if (lobbyName != "")
			_lobbyNameField.text = lobbyName;
		else
			_lobbyNameField.text = "empty";
	}
	
	public void JoinLobby()
	{
		SteamLobby.instance.JoinLobby(lobbyID);
	}
}
