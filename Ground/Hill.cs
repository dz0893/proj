public class Hill : IGround
{
	public string Name => GroundSettings.hillName;
	
	public int movingCost => GroundSettings.hillMovingCost;
	
	public MovingType movingType => MovingType.Walk;
	
	public TerrainType terrainType => TerrainType.Hill;
	
	public bool isNegativeEffected => false;
	
	public bool canBeTerraformated => true;
}
