public class GoldDeposits : IGround
{
	public string Name => GroundSettings.goldDepositName;
	
	public int movingCost => GroundSettings.depositMovingCost;
	
	public MovingType movingType => MovingType.Fly;
	
	public TerrainType terrainType => TerrainType.GoldDeposit;
	
	public bool isNegativeEffected => false;
	
	public bool canBeTerraformated => false;
}
