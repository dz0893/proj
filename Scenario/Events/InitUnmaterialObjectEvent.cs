using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/InitUnmaterialObject")]
public class InitUnmaterialObjectEvent : MapEvent
{
	[SerializeField] private List<int> _playerIndexList;
	[SerializeField] private List<UnmaterialObject> _objectList;
	[SerializeField] private int _index;
	
	public override void CurrentEventActivate()
	{
		List<GroundCell> cellsWithIndex = BattleMap.instance.GetAllCellsWithIndex(_index);
		
		for (int i = 0; i < _objectList.Count; i++)
		{
			if (cellsWithIndex[i].onCellObject == null || cellsWithIndex[i].onCellObject.currentStats.maxMovePoints != 0)
			{
				UnmaterialObject obj = Instantiate(_objectList[i], BattleMap.instance.objectMap.transform);
				obj.Init(cellsWithIndex[i], BattleMap.instance.playerList[_playerIndexList[i]]);
			}
		}
	}
}
