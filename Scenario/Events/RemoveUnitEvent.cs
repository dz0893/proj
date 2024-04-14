using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/RemoveUnit")]
public class RemoveUnitEvent : MapEvent
{
	[SerializeField] private Unit _unitPref;
	[SerializeField] private bool _removeAllUnits;
	[SerializeField] private int _indexOfUnitHost;
	
	public override void CurrentEventActivate()
	{
		foreach (NullObject obj in BattleMap.instance.playerList[_indexOfUnitHost].objectList)
		{
			if (obj.index == _unitPref.index)
			{
				Unit unit = obj as Unit;
				
				unit.RemoveFromGame();
				
				if (!_removeAllUnits)
					break;
			}
		}
	}
}
