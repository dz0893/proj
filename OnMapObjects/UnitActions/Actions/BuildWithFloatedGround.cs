using System.Collections.Generic;
using UnityEngine;

public class BuildWithFloatedGround : Build
{
	[SerializeField] private List<NullObject> _buildedObjectList;
	
	public override NullObject building
	{
		get
		{
			if (terrainTypeList.Contains(currentTerrainType))
				return _buildedObjectList[terrainTypeList.IndexOf(currentTerrainType)];
			else
				return _buildedObjectList[0];
		}
	}

	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		if (terrainTypeList.Contains(cell.terrainType))
			return _buildedObjectList[terrainTypeList.IndexOf(cell.terrainType)].Name;
		else
			return _buildedObjectList[0].Name;
	}
}
