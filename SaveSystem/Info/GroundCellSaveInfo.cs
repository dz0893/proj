using System.Collections.Generic;

[System.Serializable]
public struct GroundCellSaveInfo
{
	public int terrainType;
	
	public int currentOreValue;

	public List<UnitSaveInfo> grave;
	
	public GroundCellSaveInfo(GroundCell cell)
	{
		terrainType = (int)cell.terrainType;
		currentOreValue = cell.currentOreValue;

		grave = new List<UnitSaveInfo>();
		
		foreach (Unit unit in cell.grave)
			grave.Add(new UnitSaveInfo(unit));
	}
}
