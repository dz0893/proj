using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitCellUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Image _icon;
	[SerializeField] private Image _statusIcon;
	[SerializeField] private Image _healthBar;

	[SerializeField] private GameObject _selectingFrame;
	public GameObject selectingFrame => _selectingFrame;
	
	public Unit unit { get; private set; }
	
	public void Render(Unit unit)
	{
		this.unit = unit;
		
		_icon.sprite = unit.icon;
		RenderStatus(unit);
		RenderHealthBar(unit);
	}
	
	private void RenderStatus(Unit unit)
	{
		if (unit.turnEnded)
			_statusIcon.color = Color.gray;
			
		else if (unit.currentMovePoints < unit.currentStats.maxMovePoints)
			_statusIcon.color = Color.yellow;
		
		else
			_statusIcon.color = Color.green;
	}
	
	public void RenderSelectingFrame(Unit unit)
	{
		if (unit == this.unit)
		{
			_selectingFrame.SetActive(true);
		}
		else
		{
			_selectingFrame.SetActive(false);
		}
	}

	private void RenderHealthBar(Unit unit)
	{
		_healthBar.fillAmount = (float)unit.currentHealth / unit.currentStats.maxHealth;
	}
	
	public void OnPointerClick (PointerEventData eventData)
	{
		Select();
	}

	public void Select()
	{
		if (!TurnController.currentPlayerNotLocal)
		{
			MapController.select.Invoke(unit.position);
			CameraController.setCameraPosition.Invoke(unit.transform.position);
			PlayerUI.refreshPlayerInfo.Invoke(TurnController.currentPlayer);
			UnitListUI.select.Invoke(this);
		}
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		ObjectInfoUI.writeInfo.Invoke(unit.info);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		ObjectInfoUI.cleanInfo.Invoke();
	}
}
