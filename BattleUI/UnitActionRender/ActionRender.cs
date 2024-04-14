using System.Collections.Generic;
using UnityEngine;

public class ActionRender
{
	public static List<GroundCell> areaList { get; private set; }
	
	public void Render(IActionDescription action, GroundCell target)
	{
		areaList = action.GetAreaDistance(target);
			
		foreach (GroundCell cell in areaList)
			cell.targetCell.Render(action);
	}
	
	public void Clean()
	{
		foreach (GroundCell cell in BattleMap.instance.mapCellList)
		{
			cell.targetCell.Clean();
		}
	}
}
