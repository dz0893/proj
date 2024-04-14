using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/InitUnits")]
public class InitUnitsEvent : MapEvent
{
	[SerializeField] private int _unitHostIndex;
	[SerializeField] private Unit _unit;
	[SerializeField] private bool _startTurn;
	[SerializeField] private bool _toGrave;
	[SerializeField] private int _positionIndex;
	
	[SerializeField] private bool _initOnAllCells;
	[SerializeField] private int _countOfInitedUnits;

	private int currentUnitsCount;
	
	public override void CurrentEventActivate()
	{
		List<GroundCell> cellList = BattleMap.instance.GetAllCellsWithIndex(_positionIndex);
		currentUnitsCount = 0;

		foreach (GroundCell cell in cellList)
		{
			if (cell.onCellObject == null)
			{
				Unit unit = Instantiate(_unit, BattleMap.instance.objectMap.transform);
				unit.Init(cell, BattleMap.instance.playerList[_unitHostIndex]);

				currentUnitsCount++;

				if (_startTurn)
					unit.StartTurn();

				if (_toGrave)
					unit.Death();

				if (!_initOnAllCells && currentUnitsCount >= _countOfInitedUnits)
					break;
			}
		}

		PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
	}
}
