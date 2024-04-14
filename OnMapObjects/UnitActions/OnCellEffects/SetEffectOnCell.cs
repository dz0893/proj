using UnityEngine;

public class SetEffectOnCell : OnCellEffectData
{
	[SerializeField] private ActionEffect _effect;
	[SerializeField] private TerrainType _terrainType;
	public TerrainType terrainType => _terrainType;
	
	[SerializeField] private bool _workOnUndead;
	[SerializeField] private bool _workOnMech;
	
	public override void SetEffectOnObject(OnCellEffect effect, Unit unit)
	{
		if (ChekUnitForActivate(unit))
			_effect.Activate(unit);
	}
	
	private bool ChekUnitForActivate(Unit unit)
	{
		if (unit.isUndead && !_workOnUndead)
			return false;
		if (unit.isMech && !_workOnMech)
			return false;
		if (unit.terrainKnowingList.Contains(_terrainType))
			return false;
		
		return true;
	}
}
