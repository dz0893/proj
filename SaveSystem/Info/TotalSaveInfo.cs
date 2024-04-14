using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TotalSaveInfo
{
	public int turnCounter;
	public int currentPlayerIndex;

	public MapSaveInfo mapSaveInfo;
	
	public List<PlayerSaveInfo> playerSaveInfoList;
	
	public TotalSaveInfo(BattleMap map)
	{
		turnCounter = TurnController.turnCounter;
		currentPlayerIndex = map.turnController.playerIndex;
		mapSaveInfo = new MapSaveInfo(map);
		
		playerSaveInfoList = new List<PlayerSaveInfo>();
		
		foreach (Player player in map.playerList)
		{
			if (player != null)
				playerSaveInfoList.Add(new PlayerSaveInfo(player));
			else
				playerSaveInfoList.Add(new PlayerSaveInfo());
		}
	}
}
