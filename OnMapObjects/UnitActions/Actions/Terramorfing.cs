using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terramorfing : AbstractAction
{
	public TerramorfingDistanceFinder terramorfingDistanceFinder { get; private set; } = new TerramorfingDistanceFinder();
	
	[SerializeField] private TerrainType _terrainType;
	public TerrainType terrainType => _terrainType;
	
	public override bool endedTurnAction => true;
	public override bool damageFromUnitStats => false;
	public override bool rangeFromUnitStats => false;
	
	public override ActionType actionType => ActionType.Create;
	public override ActionRange range => ActionRange.Melee;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return terramorfingDistanceFinder.GetTerramorfingDistance(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
		unit.inAction = true;
		yield return new WaitForSeconds(actionTime);
		
		target.SetTerrainType(unit.map.groundFactory.GetTerrain(_terrainType));
		
		WasteResoursesAndEndTurn(unit);
		unit.inAction = false;
		yield return null;
	}
	
	protected override bool CurrentActivateCheck(Unit unit)
	{
		if (unit.currentMovePoints == 0)
			return false;
		
		else	
			return true;
	}
}
