using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GlobalActionCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Image _icon;
	[SerializeField] private Image _back;
	[SerializeField] private Image _frame;
	[SerializeField] private GameObject _filter;

	[SerializeField] private Sprite _activeBackSprite;
	[SerializeField] private Sprite _inactiveBackSprite;

	[SerializeField] private Sprite _selectedFrameSprite;
	[SerializeField] private Sprite _unselectedFrameSprite;
	
	private GlobalAction action;
	private GlobalActionInfo info = new GlobalActionInfo();
	
	private Player player;
	
	public bool canBeActivated { get; private set; }
	public bool isSelected { get; set; }
	
	public void Init(GlobalAction action, Player player)
	{
		this.action = action;
		this.player = player;
		
		info.Init(action);
		
		_icon.sprite = action.icon;
		
		SetActivatingStatus();
	}
	
	public void SetActivatingStatus()
	{
		if (action.CheckForActivating(player) && TurnController.currentPlayer == player)
		{
			canBeActivated = true;
			_back.sprite = _activeBackSprite;
			_filter.SetActive(false);
		}
		else
		{
			canBeActivated = false;
			_back.sprite = _inactiveBackSprite;
			_filter.SetActive(true);
		}

		if (isSelected)
			_frame.sprite = _selectedFrameSprite;
		else
			_frame.sprite = _unselectedFrameSprite;
	}

	public void SelectRender()
	{
		if (isSelected)
			_frame.sprite = _selectedFrameSprite;
		else
			_frame.sprite = _unselectedFrameSprite;
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		ObjectInfoUI.writeInfo.Invoke(info);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		ObjectInfoUI.cleanInfo.Invoke();
	}
	
	public void OnPointerClick (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && canBeActivated)
		{
			GlobalActionUI.select.Invoke(this, player);
			MapController.selectGlobalAction.Invoke(action);
		}
	}
}
