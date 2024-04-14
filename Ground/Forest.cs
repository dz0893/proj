public class Forest : IGround
{
	public string Name => GroundSettings.forestName;
	
	public int movingCost => GroundSettings.forestMovingCost;
	
	public MovingType movingType => MovingType.Walk;
	
	public TerrainType terrainType => TerrainType.Forest;
	
	public bool isNegativeEffected => false;
	
	public bool canBeTerraformated => true;
}
