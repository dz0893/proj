using System.Collections.Generic;
using UnityEngine;

public class AIRecruitPointBehavior : AIObjectBehavior
{
	private MineDistanceFinder mineDistanceFinder = new MineDistanceFinder();
	private System.Random random = new System.Random();
	public bool reqruitingSucsessed { get; private set; }
	
	public override void ActivateAction(NullObject obj)
	{
		reqruitingSucsessed = false;
		
		RecruitPoint recruitPoint = obj as RecruitPoint;
		
		if (RecruitPointUI.CheckForReviveHero(recruitPoint) && recruitPoint.IsFree)
		{
			reqruitingSucsessed = true;
			RecruitUnitCell.AIReviveHero(recruitPoint);
			return;
		}
		
		UnitData unitData = GetRecrutedUnit(recruitPoint);
		
		if (recruitPoint.IsFree && unitData != null)
		{
			reqruitingSucsessed = true;
			RecruitUnitCell.AIRecruitUnit(recruitPoint, unitData);
		}
	}
	
	private UnitData GetRecrutedUnit(RecruitPoint recruitPoint)
	{
		UnitData unitData = recruitPoint.unitDataList[0];
		UnitData checkedUnit = null;
		List<UnitData> recrutedUnitList = new List<UnitData>();
		
		if (!ChekUnitForRecruting(recruitPoint, unitData))
			return null;
		
		if (CheckForBuilders(recruitPoint.player))
			return unitData;
		
		for (int i = 0; i < recruitPoint.unitDataList.Count; i++)
		{
			checkedUnit = recruitPoint.unitDataList[i];
			
			if (ChekUnitForRecruting(recruitPoint, checkedUnit) && checkedUnit.unit.unitType != UnitType.Builder)
				recrutedUnitList.Add(checkedUnit);
		}
		
		if (recrutedUnitList.Count > 0)
			unitData = recrutedUnitList[random.Next(recrutedUnitList.Count)];
		
		if (unitData.unit.unitType == UnitType.Builder)
			unitData = null;
		
		return unitData;
	}
	
	private bool ChekUnitForRecruting(RecruitPoint recruitPoint, UnitData unitData)
	{
		int count = 0;
		
		if (!unitData.CheckRequiredBuildings(recruitPoint.player.capital) || unitData.goldCost > recruitPoint.player.gold
		|| unitData.oreCost > recruitPoint.player.ore || unitData.unit.leadershipCost > recruitPoint.player.maxUnitLimit - recruitPoint.player.currentUnitLimit)
			return false;
		
		if (unitData != recruitPoint.unitDataList[0])
		{
			foreach (NullObject obj in recruitPoint.player.objectList)
			{
				if (obj.Name.Equals(unitData.unit.Name))
					count++;
			}
			
			if (count >= recruitPoint.player.aiPlayer.currentUnitsCountList[recruitPoint.player.aiPlayer.unitDataList.IndexOf(unitData)])
				return false;
		}
		
		return true;
	}
	
	private bool CheckForBuilders(Player player)
	{
		if (player.capital == null)
			return false;
					
		bool buildersAreNeeded = true;
		int currentBuilders = 0;
		
		int countOfDeposits = mineDistanceFinder.GetAllFreeDepositCells().Count;
		
		foreach (NullObject obj in player.objectList)
		{
			if (obj is Unit)
			{
				Unit unit = obj as Unit;
				
				if (unit.unitType == UnitType.Builder)
					currentBuilders++;
			}
		}
		
		if (player.capital.unitDataList[0].unit.unitType != UnitType.Builder)
			buildersAreNeeded = false;
		
		if (currentBuilders >= AISettings.GetCountOfBuilders(countOfDeposits))
			buildersAreNeeded = false;
		
		return buildersAreNeeded;
	}
}
