
public interface IGround
{
	public string Name { get; }
	
	public int movingCost { get; }
	
	public MovingType movingType { get; }
	
	public TerrainType terrainType { get; }
	
	public bool isNegativeEffected { get; }
	
	public bool canBeTerraformated { get; }
}
