using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParametresUI : MonoBehaviour
{
	[SerializeField] private Text _nameField;
	[SerializeField] private Text _levelField;
	[SerializeField] private Text _experienceField;
	[SerializeField] private Text _healthField;
	[SerializeField] private Text _manaField;
	[SerializeField] private Text _movePointsField;
	
	public void Render(Unit unit)
	{
		RenderText(unit);
		RenderFields(unit);
	}
	
	private void RenderText(Unit unit)
	{
		_nameField.text = unit.Name;

		_levelField.text = UISettings.Lvl + unit.experience.currentLevel;
		
		if (unit.experience.currentLevel == unit.experience.maxLevel)
			_levelField.text += UISettings.max;

		_experienceField.text = UISettings.experience + unit.experience.currentExp;
		
		if (unit.experience.currentLevel < unit.experience.maxLevel)
			_experienceField.text += "/" + unit.experience.expToNextLevel;
			
		_healthField.text = UISettings.health + unit.currentHealth + "/" + unit.currentStats.maxHealth;
		_manaField.text = UISettings.Mana + unit.currentMana + "/" + unit.currentStats.maxMana;
		
		_movePointsField.text = UISettings.movePoints + unit.currentMovePoints + " / " + unit.currentStats.maxMovePoints;
	}
	
	private void RenderFields(Unit unit)
	{
		if (unit.experience.maxLevel == 0)
		{
			_levelField.gameObject.SetActive(false);
			_experienceField.gameObject.SetActive(false);
		}
		else
		{
			_levelField.gameObject.SetActive(true);

			if (unit.experience.currentLevel == unit.experience.maxLevel)
				_experienceField.gameObject.SetActive(false);
			else
				_experienceField.gameObject.SetActive(true);
		}

		if (unit.currentStats.maxMana == 0)
			_manaField.gameObject.SetActive(false);
		else
			_manaField.gameObject.SetActive(true);
		
		if (unit.currentStats.maxMovePoints == 0)
			_movePointsField.gameObject.SetActive(false);
		else
			_movePointsField.gameObject.SetActive(true);
	}
}
