using UnityEngine;
using System.Collections.Generic;

public class TurnController
{
	public List<Player> playerList { get; set; }
	public List<int> teamList { get; set; }
	
	public static Player currentPlayer { get; private set; }
	public static Player lastNotComputerPlayer { get; private set; }
	
	public int playerIndex { get; private set; }
	
	public static int turnCounter { get; private set; }
	
	public static bool currentPlayerNotLocal => currentPlayer.isAIPlayer || CustomNetworkManager.localPlayerId != currentPlayer.id;
	public static bool currentPlayerIsAi => currentPlayer.isAIPlayer;
	
	public delegate void KillPlayer(int playerId);
	public static KillPlayer killPlayer;

	public TurnController()
	{
		killPlayer = DestroyPlayerWithId;
	}

	public void CleanCash()
	{
		playerList = new List<Player>();
		teamList = new List<int>();

		NullObject.ObjectDied.RemoveListener(ActivateEndGameChecking);
		currentPlayer = null;

		lastNotComputerPlayer = null;

		playerIndex = 0;
		turnCounter = 0;
	}

	public bool IsLocalPlayer(Player player)
	{
		if (!player.isAIPlayer && CustomNetworkManager.localPlayerId == player.id)
			return true;
		else
			return false;
	}

	public void InitPlayerList()
	{
		lastNotComputerPlayer = null;
		
		foreach (Player player in playerList)
		{
			if (IsLocalPlayer(player))
			{
				lastNotComputerPlayer = player;
				break;
			}
		}
	}
	
	private void InitTeamList()
	{
		teamList = new List<int>();
		
		foreach (Player player in playerList)
		{
			if (!teamList.Contains(player.team))
				teamList.Add(player.team);
		}
		
		if (BattleMap.instance.GetComponent<Scenario>() == null)
			NullObject.ObjectDied.AddListener(ActivateEndGameChecking);
	}
	
	public void InitMatch(Player player)
	{
		InitTeamList();
		currentPlayer = player;
		playerIndex = playerList.IndexOf(player);
		turnCounter = 0;
			
		currentPlayer.StartTurn();
	}
	
	public void InitLoadetMatch(Player player, int startedTurnCounter)
	{
		InitTeamList();
		currentPlayer = player;
		playerIndex = playerList.IndexOf(player);
		turnCounter = startedTurnCounter;
		
		if (IsLocalPlayer(player))
			lastNotComputerPlayer = player;
	}
	
	public void EndTurn()
	{
		playerIndex++;
		
		if (playerList.Count == playerIndex)
		{
			playerIndex = 0;
			turnCounter++;
		}
		
		currentPlayer.EndTurn();
		currentPlayer = playerList[playerIndex];
		
		if (IsLocalPlayer(currentPlayer))
			lastNotComputerPlayer = currentPlayer;
		
		currentPlayer.StartTurn();
	}
	
	private void ActivateEndGameChecking(NullObject obj)
	{
		if (obj.player == null)
			return;
		
		if (obj.player.capital == obj)
		{
			DestroyPlayer(obj.player);
			
			if (CheckForEndGame())
			{
				if (currentPlayer.team == lastNotComputerPlayer.team)
					PlayerUI.openGameResult(true);
				else
					PlayerUI.openGameResult(false);
			}
		}
	}
	
	private bool CheckForEndGame()
	{
		int countOfLivingTeam = 0;
		
		foreach (int team in teamList)
		{
			if (!CheckTeamForLosing(team))
				countOfLivingTeam++;
		}
		
		if (countOfLivingTeam > 1)
			return false;
		else
			return true;
	}
	
	private bool CheckTeamForLosing(int team)
	{
		foreach (Player player in playerList)
		{
			if (!player.capital.isDead && player.team == team)
				return false;
		}
		
		return true;
	}
	
	private void DestroyPlayer(Player player)
	{
		player.LoseGame();
		playerList.Remove(player);
		TeamsRendererUI.renderTeams.Invoke();
	}

	private void DestroyPlayerWithId(int playerId)
	{
		Debug.Log("destroy " + playerId);
		foreach (Player player in playerList)
		{
			if (player.id == playerId)
			{
				Debug.Log("aas");
				DestroyPlayer(player);
			}
		}
	}
}
