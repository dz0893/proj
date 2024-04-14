using System.Collections.Generic;
using UnityEngine;

public class NoCapitalRecruitPoint : RecruitPoint
{
	[SerializeField] private List<UnitDataObject> _unitDataObjectList;
	
	private List<UnitData> _unitDataList;
	public override List<UnitData> unitDataList => _unitDataList;
	
	private void InitUnitDataList()
	{
		_unitDataList = new List<UnitData>();

		foreach (UnitDataObject dataObject in _unitDataObjectList)
			_unitDataList.Add(dataObject.GetUnitData(player));
	}

	protected override void LocalInit(GroundCell positionCell)
	{
		InitUnitDataList();
		
		InitInfo(new UnmaterialObjectInfo());
		turnEnded = true;
	}
}
