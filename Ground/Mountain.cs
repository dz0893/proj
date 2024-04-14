

public class Mountain : IGround
{
	public string Name => GroundSettings.mountainName;
	
	public int movingCost => GroundSettings.mountainMovingCost;
	
	public MovingType movingType => MovingType.Fly;
	
	public TerrainType terrainType => TerrainType.Mountain;
	
	public bool isNegativeEffected => false;
	
	public bool canBeTerraformated => false;
}
