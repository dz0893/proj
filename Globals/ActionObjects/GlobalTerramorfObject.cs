using UnityEngine;

public class GlobalTerramorfObject : GlobalActionObject
{
	[SerializeField] private TerrainType _terrainType;
	public TerrainType terrainType => _terrainType;
	
	[SerializeField] private bool _areaEffect;
	public bool areaEffect => _areaEffect;
	
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalTerramorf(this);
	}
}
