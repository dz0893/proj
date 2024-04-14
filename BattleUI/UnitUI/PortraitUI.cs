using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PortraitUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Image _icon;
	
	private ObjectInfo info;
	
	public void Render(NullObject obj)
	{
		_icon.sprite = obj.icon;
		
		info = obj.info;
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		ObjectInfoUI.writeInfo.Invoke(info);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		ObjectInfoUI.cleanInfo.Invoke();
	}
}
