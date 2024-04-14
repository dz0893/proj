using System.Collections.Generic;
using UnityEngine;

public class RandomGroundCellGetter : MonoBehaviour
{
	public static RandomGroundCellGetter instance { get; private set; }
	
	[SerializeField] private List<Sprite> _roadSpriteList;
	[SerializeField] private List<Sprite> _grassSpriteList;
	[SerializeField] private List<Sprite> _forestSpriteList;
	[SerializeField] private List<Sprite> _sandSpriteList;
	[SerializeField] private List<Sprite> _hillSpriteList;
	[SerializeField] private List<Sprite> _mountainSpriteList;
	[SerializeField] private List<Sprite> _waterSpriteList;
	[SerializeField] private List<Sprite> _goldDepositSpriteList;
	[SerializeField] private List<Sprite> _oreDepositSpriteList;
	[SerializeField] private List<Sprite> _deadGroundSpriteList;
	[SerializeField] private List<Sprite> _holyGroundSpriteList;
	[SerializeField] private List<Sprite> _fortificalFloorSpriteList;

	private System.Random random = new System.Random();
	
	private void Start()
	{
		instance = this;
	}
	
	public TerrainType GetRandomTerrainType(List<TerrainType> list)
	{
		return list[random.Next(list.Count)];
	}

	public bool GetFlipState()
	{
		int state = random.Next(2);

		if (state == 0)
			return false;
		else
			return true;
	}
	
	public Sprite GetTerrainSprite(TerrainType terrainType)
	{
		Sprite sprite = null;
		
		switch (terrainType)
		{
			case TerrainType.Road:
				sprite = _roadSpriteList[random.Next(_roadSpriteList.Count)];
				break;
			case TerrainType.Grass:
				sprite = _grassSpriteList[random.Next(_grassSpriteList.Count)];
				break;
			case TerrainType.Forest:
				sprite = _forestSpriteList[random.Next(_forestSpriteList.Count)];
				break;
			case TerrainType.Sand:
				sprite = _sandSpriteList[random.Next(_sandSpriteList.Count)];
				break;
			case TerrainType.Hill:
				sprite = _hillSpriteList[random.Next(_hillSpriteList.Count)];
				break;
			case TerrainType.Mountain:
				sprite = _mountainSpriteList[random.Next(_mountainSpriteList.Count)];
				break;
			case TerrainType.Water:
				sprite = _waterSpriteList[random.Next(_waterSpriteList.Count)];
				break;
			case TerrainType.GoldDeposit:
				sprite = _goldDepositSpriteList[random.Next(_goldDepositSpriteList.Count)];
				break;
			case TerrainType.OreDeposit:
				sprite = _oreDepositSpriteList[random.Next(_oreDepositSpriteList.Count)];
				break;
			case TerrainType.DeadGround:
				sprite = _deadGroundSpriteList[random.Next(_deadGroundSpriteList.Count)];
				break;
			case TerrainType.HolyGround:
				sprite = _holyGroundSpriteList[random.Next(_holyGroundSpriteList.Count)];
				break;
			case TerrainType.FortificalFloor:
				sprite = _fortificalFloorSpriteList[random.Next(_fortificalFloorSpriteList.Count)];
				break;
		}
		
		if (sprite == null)
			Debug.Log("ALARM!!! Ground cell sprite not founded");
		
		return sprite;
	}
}
