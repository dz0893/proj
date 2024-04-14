using System.Collections.Generic;
using UnityEngine;

public class BuildingListUI : MonoBehaviour
{
	[SerializeField] private Transform _container;
	[SerializeField] private List<BuildingCell> _buildingCellList;
	
	public void Render(Capital capital)
	{
		Clean();
		Init(capital);
	}
	
	private void Clean()
	{
		foreach (BuildingCell cell in _buildingCellList)
			cell.gameObject.SetActive(false);
	}
	
	private void Init(Capital capital)
	{
		for (int i = 0; i < capital.buildingList.Count; i++)
		{
			_buildingCellList[i].gameObject.SetActive(true);
			_buildingCellList[i].Render(capital, capital.buildingList[i]);
		}
	}
}
