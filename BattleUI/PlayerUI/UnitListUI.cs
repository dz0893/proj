using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitListUI : MonoBehaviour
{
	[SerializeField] private UnitCellUI _cellPref;
	[SerializeField] private List<UnitCellUI> _cellList;

	public List<UnitCellUI> cellList;
	private UnitCellUI selectedCell;

	private int countOfActiveCell => cellList.Count;
	private int indexOfSelectedUnit;

	public delegate void Select(UnitCellUI cell);
	public static Select select;

	public delegate void SelectUnit(Unit unit);
	public static SelectUnit selectUnit;

	public delegate void ClearCellCash();
	public static ClearCellCash clearCellCash;

	private void Start()
	{
		select = SelectCell;
		clearCellCash = ClearSelectedCellCash;

		selectUnit = RenderUnitSelecting;
	}

	public void NextUnitButton(bool increaseIndex)
	{
		if (countOfActiveCell == 0)
			return;
		
		ScrollCell(increaseIndex);
		
		cellList[indexOfSelectedUnit].Select();
	}

	private void ScrollCell(bool increaseIndex)
	{
		if (increaseIndex)
		{
			indexOfSelectedUnit++;

			if (indexOfSelectedUnit >= countOfActiveCell || selectedCell == null)
				indexOfSelectedUnit = 0;
		}
		else
		{
			indexOfSelectedUnit--;

			if (indexOfSelectedUnit < 0 || selectedCell == null)
				indexOfSelectedUnit = countOfActiveCell - 1;
		}
	}

	private void ClearSelectedCellCash()
	{
		selectedCell = null;
		indexOfSelectedUnit = 0;

		foreach (UnitCellUI cell in _cellList)
			cell.selectingFrame.SetActive(false);
	}

	private void SelectCell(UnitCellUI cell)
	{
		selectedCell = cell;
		indexOfSelectedUnit = cellList.IndexOf(selectedCell);

	//	RenderUnitSelecting(cell.unit);
	}

	public void Render(Player player)
	{
		Clean();
		
		InitUnitList(player.objectList);
	}
	
	private void Clean()
	{
		cellList = new List<UnitCellUI>();

		foreach (UnitCellUI cell in _cellList)
			cell.gameObject.SetActive(false);
	}
	
	private void InitUnitList(List<NullObject> objectList)
	{
		List<NullObject> sortedList = objectList.OrderBy(i => i.index).ToList();

		foreach (NullObject obj in sortedList)
		{
			if (obj is Unit)
			{
				Unit unit = obj as Unit;

				if (unit.actionList.Count != 0 && unit.showingInUnitList && countOfActiveCell < _cellList.Count)
				{
					UnitCellUI cell = _cellList[countOfActiveCell];

					cell.gameObject.SetActive(true);
					cell.Render(unit);

					cellList.Add(cell);
				}
			}
		}
	}

	private void RenderUnitSelecting(Unit unit)
	{
		foreach (UnitCellUI cell in _cellList)
			cell.RenderSelectingFrame(unit);
	}
}
