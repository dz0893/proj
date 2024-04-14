using System.Collections.Generic;
using UnityEngine;

public class MapSettings
{
	public static int basicHeroRevivingGoldCost = 50;
	public static int basicHeroRevivingOreCost = 0;

	public static int deltaRevivingGoldCostPerLevel = 10;
	public static int deltaRevivingGoldCostPerDeath = 20;
	public static int deltaRevivingOreCostPerLevel = 5;
	public static int deltaRevivingOreCostPerDeath = 5;
	public static int maxRevivingGoldCost = 200;
	public static int maxRevivingOreCost = 50;
	
	public static int normalGoldHandicap = 20;
	public static int highGoldHandicap = 40;

	public static int normalOreHandicap = 3;
	public static int highOreHandicap = 5;

	public static int goldDepositsPerPlayer = 2;
	
	public static int oreDepositsPerPlayer = 1;
	
	public static int maxUnitLimit = 100;
	
	public static int countOfRace = 5;
	
	public static List<Vector4> colors = new List<Vector4> { new Vector4(30/255f,50/255f,200/255f,1), new Vector4(175/255f,30/255f,30/255f,1), 
				new Vector4(30/255f,175/255f,50/255f,1), new Vector4(70/255f,10/255f,110/255f,1), 
				new Vector4(30/255f,210/255f,200/255f,1), new Vector4(1,150/255f,20/255f,1)};

	public static Vector4 GetColorWithIndex(int index)
	{
		return colors[index];
	}
	
	public static Vector4 goldDepositOnMapColor = new Vector4(1,1,140/255f,1);
	public static Vector4 oreDepositOnMapColor = new Vector4(140/255f,120/255f,1,1);
	public static Vector4 unmovableCellOnMapColor = new Vector4(50/255f,50/255f,50/255f,1);
	public static Vector4 normalCellOnMapColor = new Vector4(100/255f,100/255f,100/255f,1);
}
