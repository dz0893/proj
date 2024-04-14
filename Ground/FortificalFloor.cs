public class FortificalFloor : IGround
{
    public string Name => GroundSettings.fortificalFloorName;
	
	public int movingCost => GroundSettings.fortificalFloorMovingCost;
	
	public MovingType movingType => MovingType.Walk;
	
	public TerrainType terrainType => TerrainType.FortificalFloor;
	
	public bool isNegativeEffected => false;
	
	public bool canBeTerraformated => false;
}
