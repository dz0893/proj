using System.Collections.Generic;
using UnityEngine;

public class RecruitPoint : UnmaterialObject
{
	[SerializeField] private int _healingValue;
	
	public virtual List<Unit> unitList => player.capital.unitList;
	
	public virtual List<UnitData> unitDataList => player.capital.unitDataList;
	
	private MaterialObject obj => position.onCellObject;

	public bool IsFree
	{
		get
		{
			foreach (NullObject obj in map.objectList)
			{
				if (obj.position == position && obj != this)
					return false;
			}
		
			return true;
		}
	}
	
	public override void StartTurn()
	{
		if (obj != null && obj.team == team)
		{
			if (!obj.isMech)
				obj.RestoreHealth(_healingValue);
		}
	}
}
