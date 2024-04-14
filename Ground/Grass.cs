public class Grass : IGround
{
	public string Name => GroundSettings.grassName;
	
	public int movingCost => GroundSettings.grassMovingCost;
	
	public MovingType movingType => MovingType.Walk;
	
	public TerrainType terrainType => TerrainType.Grass;
	
	public bool isNegativeEffected => false;
	
	public bool canBeTerraformated => true;
}
