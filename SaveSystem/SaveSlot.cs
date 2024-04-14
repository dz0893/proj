using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SaveSlot : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Image _back;
	[SerializeField] private Text _indexField;
	[SerializeField] private Text _nameField;

	[SerializeField] private Sprite _selectedSprite;
	[SerializeField] private Sprite _unselectedSprite;
	
	public string Name { get; private set; }
	
	public bool isLoading { get; private set; }
	private bool isSelected;
	
	public void Render(int index, string name)
	{
		_indexField.text = index.ToString();
		_nameField.text = name;
		Name = name;
	}
	
	public void OnPointerClick (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (!isSelected)
			{
				SelectSlot();
			}
			else
			{
				if(isLoading)
					DataSaver.load.Invoke(Name);
				else
					DataSaver.save.Invoke(Name);
			}
		}
	}
	
	public void SetLoadingState(bool isLoading)
	{
		this.isLoading = isLoading;
	}
	
	private void SelectSlot()
	{
		DataSaver.setSelectedSlot(this);
		isSelected = true;
		_back.sprite = _selectedSprite;
	}
	
	public void UnselectSlot()
	{
		isSelected = false;
		
		_back.sprite = _unselectedSprite;
	}
}
