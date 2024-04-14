using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
	[SerializeField] private PlayerCell _cellPref;
	
	public List<PlayerCell> playerCellList { get; private set; }
	
	public List<Player> playerList { get; private set; }

	public void Render(int count)
	{
		Clean();
		InitCellList(count);
	}
	
	private void InitCellList(int count)
	{
		playerCellList = new List<PlayerCell>();
		
		for (int i = 0; i < count; i++)
		{
			PlayerCell cell = Instantiate(_cellPref, transform);
			playerCellList.Add(cell);
			
			cell.SetTeamDropdown(count, i);
			cell.RenderPlayerName(i);
			cell.SetRace();
		}
		
		SetAIPlayers();
	}
	
	private void Clean()
	{
		foreach (Transform child in transform)
			Destroy(child.gameObject);
	}
	
	public void InitPlayers()
	{
		playerList = new List<Player>();
		
		for (int i = 0; i < playerCellList.Count; i++)
		{
			playerCellList[i].InitPlayer();
			playerList.Add(playerCellList[i].player);
		}
	}
	
	private void SetAIPlayers()
	{
		for (int i = 1; i < playerCellList.Count; i ++)
			playerCellList[i].playerTypeDropdown.value = 1;
	}
}
