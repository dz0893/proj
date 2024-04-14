using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
	[SerializeField] private int _maxLevel;
	[SerializeField] private List<int> expStepList;
	
	[SerializeField] private List<int> healthPerLevelList;
	[SerializeField] private List<int> manaPerLevelList;
	[SerializeField] private List<int> damagePerLevelList;
	[SerializeField] private List<int> physicalDefencePerLevelList;
	[SerializeField] private List<int> piercingDefencePerLevelList;
	[SerializeField] private List<int> magicDefencePerLevelList;
	[SerializeField] private List<int> sturmDefencePerLevelList;
	[SerializeField] private List<int> movePointsPerLevelList;
	[SerializeField] private List<int> expForDestroingPerLevelList;
	[SerializeField] private List<int> attackRangePerLevelList;
	[SerializeField] private List<int> minAttackRangePerLevelList;
	[SerializeField] private List<int> restoreManaPerLevelList;
	[SerializeField] private List<int> healthRegenPerLevelList;
	[SerializeField] private List<int> manaRegenPerLevelList;
	
	private List<UnitStats> statsPerLevelList;
	
	private Unit unit;
	
	public int currentLevel { get; private set; }
	//public int maxLevel => _maxLevel;//expStepList.Count;
	public int maxLevel { get; private set; }
	
	public int currentExp { get; private set; }
	public int totalExp { get; private set; }
	
	public int expToNextLevel { get; private set; }
	
	[ContextMenu("IncreaseExp")]
	public void IncreaseExp()
	{
		expStepList[0] = 20;
		int exp = 20;
		for (int i = 0; i < expStepList.Count; i++)
		{	
			exp += 20;
			expStepList[i] = exp;
		}
	}
	
	public void Init()
	{
		unit = GetComponent<Unit>();
		currentLevel = 0;
		
		InitMaxLevel();

		if (maxLevel != 0)
		{
			SetExpToNextLevel();
			InitStats();
		}
	}

	private void InitMaxLevel()
	{
		if (_maxLevel == 0)
			return;

		CampainMapSettings mapSettings = BattleMap.instance.GetComponent<CampainMapSettings>();
		maxLevel = _maxLevel;

		if (mapSettings != null && mapSettings.maxHeroLevel < maxLevel)
			maxLevel = mapSettings.maxHeroLevel;
	}

	private void InitStats()
	{
		statsPerLevelList = new List<UnitStats>();
		
		for (int i = 0; i < expStepList.Count; i++)
		{
			statsPerLevelList.Add(new UnitStats(healthPerLevelList[i], manaPerLevelList[i], damagePerLevelList[i], physicalDefencePerLevelList[i], piercingDefencePerLevelList[i], magicDefencePerLevelList[i], sturmDefencePerLevelList[i], movePointsPerLevelList[i], expForDestroingPerLevelList[i], attackRangePerLevelList[i], minAttackRangePerLevelList[i], restoreManaPerLevelList[i], healthRegenPerLevelList[i], manaRegenPerLevelList[i]));
		}
	}
	
	public void AddExp(int exp)
	{
		currentExp += exp;
		totalExp += exp;
		
		while (currentExp >= expToNextLevel && currentLevel < maxLevel)
		{
			currentExp -= expToNextLevel;
			LevelUp();
		}
	}
	
	private void LevelUp()
	{
		IncreaseLevel();
		
		AnimationController.write(unit.transform.position, UISettings.LevelUp, Color.yellow);
	}

	private void IncreaseLevel()
	{
		unit.ChangeBasicStats(statsPerLevelList[currentLevel]);
		
		unit.AddHealth(statsPerLevelList[currentLevel].maxHealth);
		unit.AddMana(statsPerLevelList[currentLevel].maxMana);
		
		currentLevel++;
		
		if (currentLevel < maxLevel)
			SetExpToNextLevel();
	}

	public void IncreseLevelTo(int count)
	{
		for (int i = 0; i < count; i++)
		{
			totalExp += expToNextLevel;
			IncreaseLevel();
		}
	}
	
	private void SetExpToNextLevel()
	{
		expToNextLevel = expStepList[currentLevel];
	}
}
