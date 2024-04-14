
public class Water : IGround
{
	public string Name => GroundSettings.waterName;
	
	public int movingCost => GroundSettings.waterMovingCost;
	
	public MovingType movingType => MovingType.Swim;
	
	public TerrainType terrainType => TerrainType.Water;
	
	public bool isNegativeEffected => false;
	
	public bool canBeTerraformated => false;
}
