public class DeadGround : IGround
{
	public string Name => GroundSettings.deadGroundName;
	
	public int movingCost => GroundSettings.deadGroundMovingCost;
	
	public MovingType movingType => MovingType.Walk;
	
	public TerrainType terrainType => TerrainType.DeadGround;
	
	public bool isNegativeEffected => false;
	
	public bool canBeTerraformated => true;
}
