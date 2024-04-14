public class Road : IGround
{
	public string Name => GroundSettings.roadName;
	
	public int movingCost => GroundSettings.roadMovingCost;
	
	public MovingType movingType => MovingType.Walk;
	
	public TerrainType terrainType => TerrainType.Road;
	
	public bool isNegativeEffected => false;
	
	public bool canBeTerraformated => false;
}
