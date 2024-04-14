using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoContainer : AbstractInfoContainer
{
	[SerializeField] private Text _level;
	[SerializeField] private Text _experience;
	[SerializeField] private Text _health;
	[SerializeField] private Text _mana;
	[SerializeField] private Text _damage;
	[SerializeField] private Text _physicalDefence;
	[SerializeField] private Text _piercingDefence;
	[SerializeField] private Text _magicDefence;
	[SerializeField] private Text _sturmDefence;
	[SerializeField] private Text _movePoints;
	[SerializeField] private Text _activeEffects;
	[SerializeField] private GameObject _effectsHeader;
	
	public override void Render(ObjectInfo info)
	{
		UnitInfo unitInfo = info as UnitInfo;
		
		RenderStats(unitInfo);
		RenderFields(unitInfo);
		RenderEffects(unitInfo.unit);
	}
	
	private void RenderEffects(Unit unit)
	{
		_activeEffects.text = "";
		
		List<string> totalEffectList = new List<string>();
		
		foreach (CurrentEffect effect in unit.activeEffectList)
		{
			if (!totalEffectList.Contains(effect.Name))
				totalEffectList.Add(effect.Name);
		}
		
		foreach (OnCellEffect effect in unit.onCellEffectList)
			totalEffectList.Add(effect.Name);
		
		if (totalEffectList.Count > 0)
		{
			_effectsHeader.SetActive(true);
			
			for (int i = 0; i < totalEffectList.Count; i++)
			{
				_activeEffects.text += totalEffectList[i];
				
				if (i < totalEffectList.Count - 1)
					_activeEffects.text += ", ";
			}
		}
		else
		{
			_effectsHeader.SetActive(false);
		}
	}
	
	private void RenderStats(UnitInfo unitInfo)
	{
		SetStatsValues(unitInfo);
		SetStatsColors(unitInfo);
	}
	
	private void SetStatsValues(UnitInfo unitInfo)
	{
		_level.text = UISettings.Lvl + unitInfo.experience.currentLevel;
		_experience.text = UISettings.experience + unitInfo.experience.currentExp;
		
		if (!unitInfo.maxLeveled)
			_experience.text += " / " + unitInfo.experience.expToNextLevel;
		else
			_level.text += UISettings.max;
		
		_health.text = $"{UISettings.health} {unitInfo.currentHealth}/{unitInfo.currentStats.maxHealth}";
		
		if (unitInfo.healthRegen > 0)
			_health.text += $" (+{unitInfo.healthRegen})";
		
		_mana.text = $"{UISettings.Mana} {unitInfo.currentMana}/{unitInfo.currentStats.maxMana}";
		
		if (unitInfo.manaRegen > 0)
			_mana.text += $" (+{unitInfo.manaRegen})";

		_damage.text = UISettings.damage + unitInfo.currentStats.damage;
		_physicalDefence.text = UISettings.Physical + unitInfo.currentStats.physicalDefence;
		_piercingDefence.text = UISettings.Piercing + unitInfo.currentStats.piercingDefence;
		_magicDefence.text = UISettings.Magical + unitInfo.currentStats.magicDefence;
		_sturmDefence.text = UISettings.Siege + unitInfo.currentStats.siegeDefence;
		
		_movePoints.text = $"{UISettings.movePoints}{unitInfo.currentMovePoints}/{unitInfo.currentStats.maxMovePoints}";
	}
	
	private void SetStatsColors(UnitInfo unitInfo)
	{
		SetTextColor(_damage, unitInfo.basicStats.damage, unitInfo.currentStats.damage);
		SetTextColor(_physicalDefence, unitInfo.basicStats.physicalDefence, unitInfo.currentStats.physicalDefence);
		SetTextColor(_piercingDefence, unitInfo.basicStats.piercingDefence, unitInfo.currentStats.piercingDefence);
		SetTextColor(_magicDefence, unitInfo.basicStats.magicDefence, unitInfo.currentStats.magicDefence);
		SetTextColor(_sturmDefence, unitInfo.basicStats.siegeDefence, unitInfo.currentStats.siegeDefence);
		SetTextColor(_movePoints, unitInfo.basicStats.maxMovePoints, unitInfo.currentStats.maxMovePoints);
	}
	
	private void RenderFields(UnitInfo unitInfo)
	{
		if (unitInfo.currentStats.maxMana == 0)
			_mana.gameObject.SetActive(false);
		else
			_mana.gameObject.SetActive(true);
		
		if (unitInfo.currentStats.maxMovePoints == 0)
			_movePoints.gameObject.SetActive(false);
		else
			_movePoints.gameObject.SetActive(true);
		
		if (unitInfo.experience.maxLevel == 0)
		{
			_level.gameObject.SetActive(false);
			_experience.gameObject.SetActive(false);
		}
		else
		{
			_level.gameObject.SetActive(true);
			
			if (unitInfo.maxLeveled)
				_experience.gameObject.SetActive(false);
			else
				_experience.gameObject.SetActive(true);
		}
	}
	
	private void SetTextColor(Text text, int normalValue, int currentValue)
	{
		if (normalValue > currentValue)
			text.color = Color.red;
		else if (normalValue < currentValue)
			text.color = Color.green;
		else
			text.color = UISettings.defaultTextColor;
	}
}
