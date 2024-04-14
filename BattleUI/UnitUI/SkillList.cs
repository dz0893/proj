using System.Collections.Generic;
using UnityEngine;

public class SkillList : MonoBehaviour
{
	[SerializeField] private List<CurrentSkillUI> _skillContainer;
	[SerializeField] private UnitAuraUI _startedTurnActionIcon;
	[SerializeField] private UnitAuraUI _auraIcon;
	
	private Unit unit;
	
	public delegate void RenderCurrentSkill();
	
	public static RenderCurrentSkill renderCurrentSkill;
	
	private void Start()
	{
		renderCurrentSkill = RenderChoosenSkill;
	}
	
	public void Render(Unit unit)
	{
		this.unit = unit;
		
		Clean();
		
		Init(unit.actionDataList);
		
		RenderAuraAndStartedTurnAction();
	}
	
	public void Clean()
	{
		foreach (CurrentSkillUI skill in _skillContainer)
			skill.Clean();
	}
	
	public void Init(List<ActionData> actionList)
	{
		for (int i = 0; i < actionList.Count; i++)
		{
			_skillContainer[i].gameObject.SetActive(true);
			_skillContainer[i].Render(actionList[i], unit);
		}
	}
	
	private void RenderChoosenSkill()
	{
		foreach (CurrentSkillUI skill in _skillContainer)
		{
			if (skill.skill != null)
				skill.BackRender();
		}
	}
	
	public void RenderAuraAndStartedTurnAction()
	{
		if (unit.aura != null)
		{
			_auraIcon.gameObject.SetActive(true);
			_auraIcon.Render(unit.position, unit.aura);
		}
		else
		{
			_auraIcon.gameObject.SetActive(false);
		}
		
		if (unit.startedTurnAction != null)
		{
			_startedTurnActionIcon.gameObject.SetActive(true);
			_startedTurnActionIcon.Render(unit.position, unit.startedTurnActionData);
		}
		else
		{
			_startedTurnActionIcon.gameObject.SetActive(false);
		}
	}
}
