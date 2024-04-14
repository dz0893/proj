using System.Collections.Generic;
using UnityEngine;

public class AIBehaviorSetter : MonoBehaviour
{
	[SerializeField] private List<UnitDataObject> _humanUnitDataList;
	[SerializeField] private List<UnitDataObject> _orcUnitDataList;
	[SerializeField] private List<UnitDataObject> _dwarfUnitDataList;
	[SerializeField] private List<UnitDataObject> _elfUnitDataList;
	[SerializeField] private List<UnitDataObject> _undeadUnitDataList;
	
	[SerializeField] private List<int> _humanCurrentUnitsCountList;
	[SerializeField] private List<int> _orcCurrentUnitsCountList;
	[SerializeField] private List<int> _dwarfCurrentUnitsCountList;
	[SerializeField] private List<int> _elfCurrentUnitsCountList;
	[SerializeField] private List<int> _undeadCurrentUnitsCountList;
	
	[SerializeField] private bool _movingAtFirstTurn;
	public bool movingAtFirstTurn => _movingAtFirstTurn;
	
	public List<UnitDataObject> unitDataList { get; private set; }
	public List<int> currentUnitsCountList { get; private set; }
	
	public void SetUnitDataObjectList(Race race)
	{
		switch (race)
		{
			case Race.Human:
			{
				unitDataList = _humanUnitDataList;
				currentUnitsCountList = _humanCurrentUnitsCountList;
				break;
			}
			case Race.Orc:
			{
				unitDataList = _orcUnitDataList;
				currentUnitsCountList = _orcCurrentUnitsCountList;
				break;
			}
			case Race.Dwarf:
			{
				unitDataList = _dwarfUnitDataList;
				currentUnitsCountList = _dwarfCurrentUnitsCountList;
				break;
			}
			case Race.Elf:
			{
				unitDataList = _elfUnitDataList;
				currentUnitsCountList = _elfCurrentUnitsCountList;
				break;
			}
			case Race.Undead:
			{
				unitDataList = _undeadUnitDataList;
				currentUnitsCountList = _undeadCurrentUnitsCountList;
				break;
			}
		}
	}
}
