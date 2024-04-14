using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitSaveInfo
{
	public bool notNullUnit;
	public int index;
	public bool attackIsRecharget;
	public int currentHealth;
	public int currentMana;
	public int currentMovePoints;
	public int playerIndex;
	
	public int cellIndex;
	
	public int totalExp;
	public bool turnEnded;
	public bool flipped;

	public List<ActionEffectSaveInfo> effectList;
	
	public UnitSaveInfo(Unit unit)
	{
		notNullUnit = true;
		index = unit.index;
		attackIsRecharget = unit.attackIsRecharget;
		currentHealth = unit.currentHealth;
		currentMana = unit.currentMana;
		currentMovePoints = unit.currentMovePoints;
		playerIndex = BattleMap.instance.playerList.IndexOf(unit.player);
		
		cellIndex = BattleMap.instance.mapCellList.IndexOf(unit.position);
		
		totalExp = unit.experience.totalExp;
		turnEnded = unit.turnEnded;
		flipped = unit.spriteFlipped;

		effectList = new List<ActionEffectSaveInfo>();
		
		foreach (CurrentEffect effect in unit.activeEffectList)
			effectList.Add(new ActionEffectSaveInfo(effect));
	}
}
