using UnityEngine;
using UnityEngine.UI;

public class ActionTargetCell : MonoBehaviour
{
	[SerializeField] private Text _text;
	[SerializeField] private Image _icon;
	
	[SerializeField] private GroundCell _cell;
	
	[SerializeField] private SpriteRenderer _graveIcon;

	[SerializeField] private Image _possibleTargetSelecter;
	public Image possibleTargetSelecter => _possibleTargetSelecter;

	public void Render(IActionDescription action)
	{
		_text.gameObject.SetActive(true);
		_icon.gameObject.SetActive(true);
		
		_icon.sprite = action.icon;
		_text.text = action.GetRenderedText(_cell);
	}
	
	public void Clean()
	{
		_text.gameObject.SetActive(false);
		_icon.gameObject.SetActive(false);
	}

	public void OnGraveIcon()
	{
		_graveIcon.gameObject.SetActive(true);
	}

	public void OffGraveIcon()
	{
		_graveIcon.gameObject.SetActive(false);
	}
}
