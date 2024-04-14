public class HolyGround : IGround
{
	public string Name => GroundSettings.holyGroundName;
	
	public int movingCost => GroundSettings.holyGroundMovingCost;
	
	public MovingType movingType => MovingType.Walk;
	
	public TerrainType terrainType => TerrainType.HolyGround;
	
	public bool isNegativeEffected => false;
	
	public bool canBeTerraformated => true;
}
