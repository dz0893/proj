public class OreDeposits : IGround
{
	public string Name => GroundSettings.oreDepositName;
	
	public int movingCost => GroundSettings.depositMovingCost;
	
	public MovingType movingType => MovingType.Fly;
	
	public TerrainType terrainType => TerrainType.OreDeposit;
	
	public bool isNegativeEffected => false;
	
	public bool canBeTerraformated => false;
}
