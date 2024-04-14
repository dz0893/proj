using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitAuraUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private ActionRender actionRender = new ActionRender();
	[SerializeField] private Image _icon;
	
	private GroundCell position;

	private IActionDescription action;
	private ActionInfo info = new ActionInfo();
	
	public void Render(GroundCell position, IActionDescription action)
	{
		this.position = position;
		this.action = action;
		info.Init(action);
		
		_icon.sprite = action.icon;
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		ObjectInfoUI.writeInfo.Invoke(info);
		
		actionRender.Render(action, position);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		ObjectInfoUI.cleanInfo.Invoke();
		
		actionRender.Clean();
	}
}
