using System.Collections.Generic;
using UnityEngine;

public class DataReader : MonoBehaviour
{
	GroundFactory groundFactory = new GroundFactory();
	private BattleMap battleMap;
	
	[SerializeField] private Transform _mapContainer;

	public void Read(TotalSaveInfo totalSaveInfo, BattleMap map)
	{
		SetMap(totalSaveInfo);
		SetObjects(totalSaveInfo);

		SetTerrain(totalSaveInfo.mapSaveInfo.groundCellSaveInfoList, battleMap.mapCellList);
		MiniMap.init.Invoke(battleMap);
		GameOptions.setScale.Invoke();

		battleMap.GetComponent<Scenario>()?.LoadActivatorList(totalSaveInfo.mapSaveInfo.evenRowObjectList);
		battleMap.StartLoadetMatch(totalSaveInfo);
	}
	
	private void SetTerrain(List<GroundCellSaveInfo> info, List<GroundCell> cellList)
	{
		if (info.Count != cellList.Count)
		{
			Debug.Log("Wrong Lists");
			return;
		}
		
		for (int i = 0; i < info.Count; i++)
		{
			cellList[i].InitTerrainType(groundFactory.GetTerrain((TerrainType)info[i].terrainType));
			cellList[i].currentOreValue = info[i].currentOreValue;

			foreach (UnitSaveInfo unitSaveInfo in info[i].grave)
			{
				Unit unit = SetUnit(unitSaveInfo);
				unit.Death();
			}
		}
	}
	
	private List<Player> GetPlayers(List<PlayerSaveInfo> info)
	{
		List<Player> playerList = new List<Player>();
		
		foreach (PlayerSaveInfo playerInfo in info)
		{
			if (playerInfo.notNullPlayer)
			{
				Player player = new Player();
				playerList.Add(player);
				player.Init(playerInfo);
			}
			else
			{
				playerList.Add(null);
			}
		}
		
		return playerList;
	}
	
	private void SetMap(TotalSaveInfo totalSaveInfo)
	{
		battleMap = Instantiate(DataBase.instance.GetMap(totalSaveInfo.mapSaveInfo.name), _mapContainer);
		battleMap.CleanObjectMap();
		battleMap.LoadetInit(GetPlayers(totalSaveInfo.playerSaveInfoList));
	}
	
	private void SetObjects(TotalSaveInfo totalSaveInfo)
	{
		SetCapitals(totalSaveInfo.mapSaveInfo.capitalSaveInfoList);
		SetHeroes(totalSaveInfo.playerSaveInfoList);
		SetUnits(totalSaveInfo.mapSaveInfo.unitSaveInfoList);
		SetStructures(totalSaveInfo.mapSaveInfo.structureSaveInfoList);
		SetUnmaterialObjects(totalSaveInfo.mapSaveInfo.unmaterialObjectSaveInfoList);
	}
	
	private void SetCapitals(List<CapitalSaveInfo> capitalSaveInfoList)
	{
		foreach(CapitalSaveInfo capitalSaveInfo in capitalSaveInfoList)
		{
			Capital capital = Instantiate((Capital)DataBase.instance.GetObject(capitalSaveInfo.index), battleMap.objectMap.transform);
			
			capital.Init(battleMap.mapCellList[capitalSaveInfo.cellIndex], battleMap.playerList[capitalSaveInfo.playerIndex]);
			
			for (int i = 0; i < capital.buildingList.Count; i++)
				SetCapitalBuilding(capital.buildingList[i], capitalSaveInfo.buildingSaveInfoList[i]);

			battleMap.playerList[capitalSaveInfo.playerIndex].capital = capital;
			
			capital.RefreshUnitList();

			if (capital.player.isAIPlayer)
				capital.player.aiPlayer.InitBehavior();

			SetUpgrades(capital, capitalSaveInfo.upgradeList);

			capital.currentHealth = capitalSaveInfo.currentHealth;
			capital.currentMana = capitalSaveInfo.currentMana;
			capital.turnEnded = capitalSaveInfo.turnEnded;

			if (capitalSaveInfo.flipped)
				capital.RotateToRight();
			else
				capital.RotateToLeft();

			capital.RefreshHealthBar();
		}
	}
	
	private void SetHeroes(List<PlayerSaveInfo> playerSaveInfoList)
	{
		foreach(PlayerSaveInfo playerSaveInfo in playerSaveInfoList)
		{
			if (playerSaveInfo.notNullPlayer && playerSaveInfo.hero.notNullUnit)
			{
				Unit unit = SetUnit(playerSaveInfo.hero);
				unit.player.hero = unit;

				if (battleMap.GetComponent<CampainMapSettings>() == null)
					unit.player.InitGlobals(unit.heroGlobalActionList);
				else
					unit.player.InitGlobals(battleMap.GetComponent<CampainMapSettings>().GetPlayerGlobals(unit.player));

				if (playerSaveInfo.heroIsDead)
				unit.Death();
			}
		}
	}

	private void SetUpgrades(Capital capital, List<bool> upgradeList)
	{
		for (int i = 0; i < upgradeList.Count; i++)
		{
			if (upgradeList[i])
			{
				capital.player.SearchUpgrade(capital.upgradeList[i]);
			}
		}
	}
	
	private void SetCapitalBuilding(BuildingData building, BuildingSaveInfo info)
	{
		if (info.isBlocked)
			building.SetLevelOfBlocking(info.levelOfBlocking);
		
		while (building.currentLevel < info.currentLevel)
			building.IncreaseLevel();
	}
	
	private void SetUnits(List<UnitSaveInfo> unitSaveInfoList)
	{
		foreach(UnitSaveInfo unitSaveInfo in unitSaveInfoList)
			SetUnit(unitSaveInfo);
	}
	
	private Unit SetUnit(UnitSaveInfo unitSaveInfo)
	{
		Unit unit = Instantiate((Unit)DataBase.instance.GetObject(unitSaveInfo.index), battleMap.objectMap.transform);
			
		unit.Init(battleMap.mapCellList[unitSaveInfo.cellIndex], battleMap.playerList[unitSaveInfo.playerIndex]);
		
		foreach (ActionEffectSaveInfo effectInfo in unitSaveInfo.effectList)
			DataBase.instance.GetActionEffect(effectInfo.index).ActivateOnLoad(unit, effectInfo);
		
		unit.experience.AddExp(unitSaveInfo.totalExp);
		unit.currentHealth = unitSaveInfo.currentHealth;
		unit.currentMana = unitSaveInfo.currentMana;
		unit.currentMovePoints = unitSaveInfo.currentMovePoints;
		unit.turnEnded = unitSaveInfo.turnEnded;
		unit.attackIsRecharget = unitSaveInfo.attackIsRecharget;
		unit.RefreshHealthBar();
		
		if (unitSaveInfo.flipped)
				unit.RotateToRight();
			else
				unit.RotateToLeft();
			
		return unit;
	}
	
	private void SetStructures(List<StructureSaveInfo> structureSaveInfoList)
	{
		foreach(StructureSaveInfo structureSaveInfo in structureSaveInfoList)
		{
			Structure structure = Instantiate((Structure)DataBase.instance.GetObject(structureSaveInfo.index), battleMap.objectMap.transform);
			
			structure.Init(battleMap.mapCellList[structureSaveInfo.cellIndex], battleMap.playerList[structureSaveInfo.playerIndex]);
			
			structure.currentHealth = structureSaveInfo.currentHealth;
			structure.currentMana = structureSaveInfo.currentMana;
			structure.turnEnded = structureSaveInfo.turnEnded;
			structure.RefreshHealthBar();

			if (structureSaveInfo.flipped)
				structure.RotateToRight();
			else
				structure.RotateToLeft();
		}
	}
	
	private void SetUnmaterialObjects(List<UnmaterialObjectSaveInfo> unmaterialObjectSaveInfoList)
	{
		foreach(UnmaterialObjectSaveInfo unmaterialObjectSaveInfo in unmaterialObjectSaveInfoList)
		{
			UnmaterialObject unmaterialObject = Instantiate((UnmaterialObject)DataBase.instance.GetObject(unmaterialObjectSaveInfo.index), battleMap.objectMap.transform);
			
			unmaterialObject.Init(battleMap.mapCellList[unmaterialObjectSaveInfo.cellIndex], battleMap.playerList[unmaterialObjectSaveInfo.playerIndex]);
			
			unmaterialObject.turnEnded = unmaterialObjectSaveInfo.turnEnded;

			if (unmaterialObjectSaveInfo.flipped)
				unmaterialObject.RotateToRight();
			else
				unmaterialObject.RotateToLeft();
		}
	}
}
