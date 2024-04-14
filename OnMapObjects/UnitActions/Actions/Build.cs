using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : AbstractAction
{
	public BuildingDistanceFinder buildingDistanceFinder { get; private set; } = new BuildingDistanceFinder();
	
	[SerializeField] private List<int> _cellIndexList;
	public List<int> cellIndexList => _cellIndexList;

	[SerializeField] private List<TerrainType> _terrainTypeList;
	public List<TerrainType> terrainTypeList => _terrainTypeList;
	
	[SerializeField] private bool _needFreeSpace;
	public bool needFreeSpace => _needFreeSpace;

	[SerializeField] private bool _isMoveCostedAction;
	public bool isMoveCostedAction => _isMoveCostedAction;
	
	[SerializeField] private bool _endedTurnAction = true;
	public override bool endedTurnAction => _endedTurnAction;
	
	public override ActionType actionType => ActionType.Create;
	public override ActionRange range => ActionRange.Melee;
	
	[SerializeField] private NullObject _building;
	public virtual NullObject building => _building;
	
	[SerializeField] private bool _destroyUnmaterialOnCellObject = true; 
	
	protected TerrainType currentTerrainType;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return buildingDistanceFinder.GetBuildObjectDistance(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		currentTerrainType = target.terrainType;
		
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
		yield return new WaitForSeconds(actionTime);
		
		if (target.unmaterialOnCellObject != null && _destroyUnmaterialOnCellObject)
			target.unmaterialOnCellObject.Death();
		
		BattleMap.initObject.Invoke(building, unit.player, target);
		
		WasteResoursesAndEndTurn(unit);
		unit.inAction = false;
	}
	
	protected override bool CurrentActivateCheck(Unit unit)
	{
		if (building.leadershipCost > unit.player.maxUnitLimit - unit.player.currentUnitLimit && building.leadershipCost != 0)
			return false;
		
		else	
			return true;
	}

	public override string GetRenderedText(Unit unit, GroundCell cell)
	{
		return building.Name;
	}

	protected override void SetCurrentDescription(Unit unit)
	{
		string currentDescription = UISettings.CanBeBuildedOn + '\n';
		
		for (int i = 0; i < _terrainTypeList.Count; i++)
		{
			currentDescription += GroundSettings.GetTerrainName(_terrainTypeList[i]);

			if (i < _terrainTypeList.Count - 1)
				currentDescription += ", ";
			else
				currentDescription += " ";
		}
		AddStringToRequiresList(currentDescription);
	}
}
