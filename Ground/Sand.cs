
public class Sand : IGround
{
	public string Name => GroundSettings.sandName;
	
	public int movingCost => GroundSettings.sandMovingCost;
	
	public MovingType movingType => MovingType.Walk;
	
	public TerrainType terrainType => TerrainType.Sand;
	
	public bool isNegativeEffected => true;
	
	public bool canBeTerraformated => true;
}
