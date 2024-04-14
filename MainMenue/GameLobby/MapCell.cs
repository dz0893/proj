using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapCell : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private  Text _nameText;
	
	public BattleMap map { get; private set; }
	private bool isSelected;
	
	//private Vector4 selectedColor = new Vector4();
	private Color selectedColor = new Color(0xFE,0xFF,0xC8,0xFF);

	public void OnPointerClick (PointerEventData eventData)
	{
		MapPool.selectMap.Invoke(this);
		GameLobby.render.Invoke();
	}
	
	public void Select()
	{
		isSelected = true;
		_nameText.color = selectedColor;
	}
	
	public void Deselect()
	{
		isSelected = false;
		_nameText.color = Color.black;
	}
	
	public void Init(BattleMap map)
	{
		this.map = map;
		_nameText.text = map.name;
	}
}
