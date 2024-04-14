public class GroundSettings
{
	public const int TERRAINREGEN = 2;
	
	public const float OBJECTZPOSITION = 10f;
	public const float OBJECTONMOVEZPOSITION = 0f;
	public const float UNMATERIALZPOSITION = 11f;
	
	public const float HORIZONTALDX = 1.218f;	// 1f;		//0.87f
	public const float HORIZONTALDY = 0;
	
	public const float DIAGONALDX = 0.609f;	//0.5f;	//0.435f
	public const float DIAGONALDY = 1.05f;	//0.75f	//0.75f
	
	public static string defaulTerrainName = "default";
	public static string roadName = "Дорога";
	public static string grassName = "Трава";
	public static string forestName = "Лес";
	public static string sandName = "Пустошь";
	public static string hillName = "Холмы";
	public static string waterName = "Вода";
	public static string mountainName = "Горы";
	public static string goldDepositName = "Залежи золота";
	public static string oreDepositName = "Залежи магической руды";
	public static string deadGroundName = "Проклятая земля";
	public static string holyGroundName = "Освященная земля";
	public static string fortificalFloorName = "Форт";
	
	public static int defaulTerrainMovingCost = 2;
	public static int roadMovingCost = 2;
	public static int grassMovingCost = 2;
	public static int forestMovingCost = 3;
	public static int sandMovingCost = 2;
	public static int hillMovingCost = 3;
	public static int waterMovingCost = 2;
	public static int mountainMovingCost = 2;
	public static int depositMovingCost = 3;
	public static int deadGroundMovingCost = 2;
	public static int holyGroundMovingCost = 2;
	public static int fortificalFloorMovingCost = 2;

	public static int maxGoldValue = 300;
	public static int maxOreValue = 150;
	
	public static string GetTerrainName(TerrainType type)
	{
		if (type == TerrainType.Road)
			return roadName;
		else if (type == TerrainType.Grass)
			return grassName;
		else if (type == TerrainType.Forest)
			return forestName;
		else if (type == TerrainType.Sand)
			return sandName;
		else if (type == TerrainType.Hill)
			return hillName;
		else if (type == TerrainType.Mountain)
			return mountainName;
		else if (type == TerrainType.Water)
			return waterName;
		else if (type == TerrainType.GoldDeposit)
			return goldDepositName;
		else if (type == TerrainType.OreDeposit)
			return oreDepositName;
		else if (type == TerrainType.DeadGround)
			return deadGroundName;
		else if (type == TerrainType.HolyGround)
			return holyGroundName;
		else
			return defaulTerrainName;
	}
}
