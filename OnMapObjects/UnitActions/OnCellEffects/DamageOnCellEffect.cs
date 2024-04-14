using UnityEngine;

public class DamageOnCellEffect : OnCellEffectData
{
	[SerializeField] private int _damage;
	public int damage => _damage;
	
	[SerializeField] private DamageType _damageType;
	public DamageType damageType => _damageType;
	
	[SerializeField] private TerrainType _terrainType;
	public TerrainType terrainType => _terrainType;
	
	[SerializeField] private bool _workOnUndead;
	[SerializeField] private bool _workOnMech;
	
	public override void SetEffectOnObject(OnCellEffect effect, Unit unit)
	{
		if (ChekUnitForActivate(unit))
			unit.GetAttack(_damage, _damageType);
	}
	
	private bool ChekUnitForActivate(Unit unit)
	{
		if (unit.isUndead && !_workOnUndead)
			return false;
		if (unit.isMech && !_workOnMech)
			return false;
		if (unit.terrainKnowingList.Contains(_terrainType))
			return false;
		if (unit.CheckDamageForLetality(_damage, _damageType))
			return false;
		
		return true;
	}
}
