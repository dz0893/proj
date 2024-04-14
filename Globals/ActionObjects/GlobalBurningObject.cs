using UnityEngine;

public class GlobalBurningObject : GlobalActionObject
{
	[SerializeField] private TerrainType _newTerrainType;
	public TerrainType newTerrainType => _newTerrainType;
	
	[SerializeField] private DamageType _damageType;
	public DamageType damageType => _damageType;
	
	[SerializeField] private bool _areaEffect;
	public bool areaEffect => _areaEffect;

	[SerializeField] private bool _terrainChanger;
	public bool terrainChanger => _terrainChanger;
	
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalBurning(this);
	}
}
