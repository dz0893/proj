using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MapSaveInfo
{
	public string name;
	
	public List<GroundCellSaveInfo> groundCellSaveInfoList;
	public List<UnitSaveInfo> unitSaveInfoList;
	public List<StructureSaveInfo> structureSaveInfoList;
	public List<CapitalSaveInfo> capitalSaveInfoList;
	public List<UnmaterialObjectSaveInfo> unmaterialObjectSaveInfoList;
	public List<EventRowObjectSaveInfo> evenRowObjectList;
	
	public MapSaveInfo(BattleMap map)
	{
		name = map.Name;
		
		groundCellSaveInfoList = new List<GroundCellSaveInfo>();
		unitSaveInfoList = new List<UnitSaveInfo>();
		structureSaveInfoList = new List<StructureSaveInfo>();
		capitalSaveInfoList = new List<CapitalSaveInfo>();
		unmaterialObjectSaveInfoList = new List<UnmaterialObjectSaveInfo>();
		evenRowObjectList = new List<EventRowObjectSaveInfo>();
		
		foreach (GroundCell cell in map.mapCellList)
			groundCellSaveInfoList.Add(new GroundCellSaveInfo(cell));
			
		foreach (NullObject obj in map.objectList)
		{
			if (obj is Unit && obj.player.hero != obj)
				unitSaveInfoList.Add(new UnitSaveInfo(obj as Unit));
			else if (obj is Capital)
				capitalSaveInfoList.Add(new CapitalSaveInfo(obj as Capital));
			else if (obj is Structure)
				structureSaveInfoList.Add(new StructureSaveInfo(obj as Structure));
			else if (obj is UnmaterialObject)
			{
				UnmaterialObjectSaveInfo unmaterialObjectSaveInfo = new UnmaterialObjectSaveInfo(obj as UnmaterialObject);
				if (unmaterialObjectSaveInfo.dontHaveIniter)
					unmaterialObjectSaveInfoList.Add(unmaterialObjectSaveInfo);
			}
		}
		
		if (map.GetComponent<Scenario>() != null)
		{
			foreach (IEventRowObject rowObject in map.GetComponent<Scenario>().eventSystemLoader.eventRowLog)
			{
				evenRowObjectList.Add(new EventRowObjectSaveInfo(rowObject));
			}
		}
	}
}
