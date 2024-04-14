using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CurrentActionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _icon;

    private ActionInfo info = new ActionInfo();

    public void Render(IActionDescription action, Unit unit)
    {
        info.Init(action);

        _icon.sprite = action.icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
	{
		ObjectInfoUI.writeInfo.Invoke(info);
	//	BattleMap.rendertAreaOfAction.Invoke(unit, skill);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
	//	BattleMap.cleanArea.Invoke();
		ObjectInfoUI.cleanInfo.Invoke();
	}
}
