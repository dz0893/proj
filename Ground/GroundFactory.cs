public class GroundFactory
{
	public IGround GetTerrain(TerrainType type)
	{
		switch (type)
		{
			case TerrainType.Road:
				return new Road();
			case TerrainType.Grass:
				return new Grass();
			case TerrainType.Forest:
				return new Forest();
			case TerrainType.Sand:
				return new Sand();
			case TerrainType.Hill:
				return new Hill();
			case TerrainType.Mountain:
				return new Mountain();
			case TerrainType.Water:
				return new Water();
			case TerrainType.GoldDeposit:
				return new GoldDeposits();
			case TerrainType.OreDeposit:
				return new OreDeposits();
			case TerrainType.DeadGround:
				return new DeadGround();
			case TerrainType.HolyGround:
				return new HolyGround();
			case TerrainType.FortificalFloor:
				return new FortificalFloor();
		}
		
		return new Road();
	}
}
