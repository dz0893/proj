using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartedTurnOrcishTerramorfer : AbstractAction, IAreaAction
{
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	
	public override bool endedTurnAction => false;
	public override bool damageFromUnitStats => false;
	public override bool rangeFromUnitStats => false;
	
	public override ActionType actionType => ActionType.Defensive;
	
	public override ActionRange range => ActionRange.OnCaster;
	
	[SerializeField] private TerrainType _terrainType;
	[SerializeField] private bool _terramorfClosestCells;
	
	public int area
	{
		get
		{
			if (_terramorfClosestCells)
				return 1;
			else
				return 0;
		}
	}
	
	public List<GroundCell> GetAreaDistance(GroundCell position)
	{
		List<GroundCell> areaList = new List<GroundCell>();
		
		areaList.Add(position);
		
		if (_terramorfClosestCells)
		{
			foreach (GroundCell cell in position.closestCellList)
				areaList.Add(cell);
		}
		
		return areaList;
	}
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetCasterPosition(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		WasteResoursesAndEndTurn(unit);
		
		TerramorfUnitPosition(unit);
		
		if (_terramorfClosestCells)
			TerramorfClosestCells(unit);
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
	
	private void TerramorfUnitPosition(Unit unit)
	{
		if (unit.position.canBeTerraformated && unit.position.terrainType != _terrainType)
			unit.position.SetTerrainType(unit.map.groundFactory.GetTerrain(_terrainType));
	}
	
	private void TerramorfClosestCells(Unit unit)
	{
		foreach (GroundCell cell in unit.position.closestCellList)
		{
			if (cell.canBeTerraformated)
				cell.SetTerrainType(unit.map.groundFactory.GetTerrain(_terrainType));
		}
	}
	
	protected override void SetCurrentDescription(Unit unit)
	{
		AddStringToRequiresList(UISettings.TerraformateAtStartOfTurn);
		AddStringToRequiresList(UISettings.NewTerrainType + GroundSettings.GetTerrainName(_terrainType));
		
		if (_terramorfClosestCells)
			AddStringToRequiresList(UISettings.TerrainArea + UISettings.closestCells);
		else
			AddStringToRequiresList(UISettings.TerrainArea + UISettings.currentPosition);
	}
}
