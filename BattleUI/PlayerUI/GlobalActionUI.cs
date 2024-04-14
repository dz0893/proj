using System.Collections.Generic;
using UnityEngine;

public class GlobalActionUI : MonoBehaviour
{
	[SerializeField] private List<GlobalActionCell> _cellList;
	
	public delegate void Select(GlobalActionCell cell, Player player);
	public delegate void Clean();

	public static Select select;
	public static Clean clean;

	private void Start()
	{
		select = SelectCell;
		clean = CleanSelect;
	}

	public void Render(Player player)
	{
		foreach (GlobalActionCell cell in _cellList)
			cell.gameObject.SetActive(false);
		
		for (int i = 0; i < player.globalActionList.Count; i++)
		{
			_cellList[i].gameObject.SetActive(true);
			_cellList[i].Init(player.globalActionList[i], player);
		}
	}
	
	private void SelectCell(GlobalActionCell cell, Player player)
	{
		CleanSelect();
		
		cell.isSelected = true;

		Render(player);
	}
	
	public void CleanSelect()
	{
		foreach (GlobalActionCell currentCell in _cellList)
			currentCell.isSelected = false;
	}
}
