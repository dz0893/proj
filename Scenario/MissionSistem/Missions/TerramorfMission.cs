using System.Collections.Generic;
using UnityEngine;

public class TerramorfMission : Mission
{
	private TerrainType requiredTerrainType;
	private int cellIndex;
	
	private List<GroundCell> cellList = new List<GroundCell>();
	
	public TerramorfMission(TerramorfMissionObject missionObject, Player player)
	{
		this.missionObject = missionObject;
		requiredTerrainType = missionObject.requiredTerrainType;
		cellIndex = missionObject.cellIndex;
		this.player = player;
		
		InitTargets();
		
		GroundCell.TerrainTypeWasChanged.AddListener(ActivateChecking);
	}
	
	private void ActivateChecking(TerrainType oldTerrainType, TerrainType newTerrainType)
	{
		TryToEndMission();
	}
	
	protected override bool CheckForEnded()
	{
		cellList = BattleMap.instance.GetAllCellsWithIndex(cellIndex);
		
		foreach (GroundCell cell in cellList)
		{
			if (cell.terrainType != requiredTerrainType)
				return false;
		}
		
		return true;
		
	}
	
	public override void RemoveListener()
	{
		GroundCell.TerrainTypeWasChanged.RemoveListener(ActivateChecking);
	}
}
