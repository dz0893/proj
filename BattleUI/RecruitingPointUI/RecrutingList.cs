using System.Collections.Generic;
using UnityEngine;

public class RecrutingList : MonoBehaviour
{
	[SerializeField] private RecruitUnitCell _cellPref;
	public RecruitPoint recruitPoint { get; set; }
	[SerializeField] private List<RecruitUnitCell> cellList;
	public List<RecruitUnitCell> CellList => cellList; 
	
	public void Render(List<UnitData> unitDataList, Player player)
	{
		Clean();
		InitUnitList(unitDataList, player);
	}
	
	private void Clean()
	{
		foreach(RecruitUnitCell cell in cellList)
			cell.gameObject.SetActive(false);
	}
	
	private void InitUnitList(List<UnitData> unitDataList, Player player)
	{
		for (int i = 0; i < unitDataList.Count; i++)
		{
			cellList[i].gameObject.SetActive(true);
			
			cellList[i].recruitPoint = recruitPoint;
			cellList[i].Render(unitDataList[i], player);
		}
	}
}
