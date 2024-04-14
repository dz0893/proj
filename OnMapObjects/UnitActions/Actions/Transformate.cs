using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformate : AbstractAction
{
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	private BuildingDistanceFinder buildingDistanceFinder = new BuildingDistanceFinder();
	
	public override bool usedOnCasterOnly => true;
	public override bool endedTurnAction => true;
	public override bool damageFromUnitStats => false;
	public override bool rangeFromUnitStats => false;
	
	public override ActionType actionType => ActionType.Defensive;
	
	[SerializeField] private ActionRange _range;
	public override ActionRange range => _range;
	
	[SerializeField] private Unit _transformatedUnit;
	[SerializeField] private Structure _transformatedStruct;
	[SerializeField] private bool _createUnmaterialObect;
	[SerializeField] private bool _destroyUnmaterialObect;
	[SerializeField] private bool _ToStruct;
	[SerializeField] private UnmaterialObject _obj;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		if (range == ActionRange.Melee)
			return buildingDistanceFinder.GetTransformToBuildingDistance(unit);
		else
			return actionDistanceFinder.GetCasterPosition(unit);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		WasteResoursesAndEndTurn(unit);
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
		if (_ToStruct)
			TransformToStruct(unit, target);
		else
			TransformToUnit(unit, target);
		
		yield return new WaitForSeconds(actionTime);
		unit.inAction = false;
	}
	
	private void TransformToUnit(Unit unit, GroundCell target)
	{
		int currentHealth = unit.currentHealth;
		int currentMana = unit.currentMana;
		int exp = unit.experience.totalExp;
		Player player = unit.player;
		
		unit.RemoveFromGame();
		
		BattleMap.initObject.Invoke(_transformatedUnit, player, target);
		
		Unit transformedUnit = target.onCellObject as Unit;
		
		transformedUnit.experience.AddExp(exp);
		transformedUnit.currentHealth = currentHealth;
		transformedUnit.currentMana = currentMana;
		transformedUnit.RefreshHealthBar();
		
		if (_createUnmaterialObect)
			InitUnmaterialObjects(target, transformedUnit);
		
		else if (_destroyUnmaterialObect)
			DestroyUnmaterialObjects(target);
	}
	
	private void TransformToStruct(Unit unit, GroundCell target)
	{
		Player player = unit.player;
		unit.RemoveFromGame();
		BattleMap.initObject.Invoke(_transformatedStruct, player, target);
	}
	
	private void InitUnmaterialObjects(GroundCell center, Unit unit)
	{
		foreach (GroundCell cell in center.closestCellList)
		{
			if ((cell.unmaterialOnCellObject == null || !cell.unmaterialOnCellObject.Name.Equals(_obj.Name))
			&& cell.canBeTerraformated)
			{
				BattleMap.initObject.Invoke(_obj, unit.player, cell);
			}
		}
	}
	
	private void DestroyUnmaterialObjects(GroundCell center)
	{
		foreach (GroundCell cell in center.closestCellList)
		{
			if (cell.unmaterialOnCellObject != null && cell.unmaterialOnCellObject.Name.Equals(_obj.Name))
				cell.unmaterialOnCellObject.Death();
		}
	}
}
