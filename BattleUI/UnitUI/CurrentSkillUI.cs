using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CurrentSkillUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Image _icon;
	[SerializeField] private Image _back;
	[SerializeField] private Image _iconBack;
	[SerializeField] private GameObject _filter;
	
	[SerializeField] private Sprite _activeIconBackSprite;
	[SerializeField] private Sprite _inactiveIconBackSprite;

	[SerializeField] private Sprite _selectedBackSprite;
	[SerializeField] private Sprite _unselectedBackSprite;

	public ActionData skill { get; private set; }
	private ActionInfo info = new ActionInfo();
	
	public Unit unit { get; private set; }
	
	public void Clean()
	{
		skill = null;
		gameObject.SetActive(false);
	}
	
	public void Render(ActionData skill, Unit unit)
	{
		this.unit = unit;
		this.skill = skill;
		
		info.Init(skill);
		
		_icon.sprite = skill.icon;
		
		BackRender();
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		ObjectInfoUI.writeInfo.Invoke(info);
		BattleMap.rendertAreaOfAction.Invoke(unit, skill);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		BattleMap.cleanArea.Invoke();
		ObjectInfoUI.cleanInfo.Invoke();
	}
	
	public void BackRender()
	{
		if (unit.choosenAction == skill.abstractAction)
			_back.sprite = _selectedBackSprite;
		else
			_back.sprite = _unselectedBackSprite;
		
		if (unit.canUseActions && skill.abstractAction.ChekActionForActive(unit))
		{
			_iconBack.sprite = _activeIconBackSprite;
			_filter.SetActive(false);
		}
		else
		{
			_iconBack.sprite = _inactiveIconBackSprite;
			_filter.SetActive(true);
		}
	}
	
	public void OnPointerClick (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && unit.canUseActions && skill.abstractAction.ChekActionForActive(unit)
		&& unit.choosenAction != skill.abstractAction && !TurnController.currentPlayerNotLocal)
		{
			unit.ChooseAction(skill.abstractAction);

			if (skill.abstractAction.fastActivate)
			{
				MapController.action.Invoke(unit.position);
			}
			else
			{
				SkillList.renderCurrentSkill.Invoke();
			}
		}
	}
}
