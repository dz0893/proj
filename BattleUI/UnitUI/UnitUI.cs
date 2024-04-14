using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
	[SerializeField] private SkillList _skillList;
	[SerializeField] private ParametresUI _parametresUI;
	
	public void Render(Unit unit)
	{
		_skillList.Render(unit);
		
		_parametresUI.Render(unit);
	}
}
