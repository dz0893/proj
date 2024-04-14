using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MissionCell : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Text _nameText;
	
	public Mission mission { get; private set; }
	public JournalSheet journalSheet { get; private set; }
	private bool isSelected;
	
	public void OnPointerClick (PointerEventData eventData)
	{
		JournalUI.selectMission.Invoke(this);
	}
	
	public void Init(Mission mission)
	{
		this.mission = mission;
		
		_nameText.text = mission.Name;
		
		RenderColor();
	}
	
	public void Init(JournalSheet journalSheet)
	{
		this.journalSheet = journalSheet;
		
		_nameText.text = UISettings.Turn + journalSheet.turn + ": " + journalSheet.name;
		
		RenderColor();
	}
	
	private void RenderColor()
	{
		if (isSelected)
			_nameText.color = Color.white;
			
		else if (mission != null && mission.isFailed)
			_nameText.color = Color.red;
		else if (mission != null && mission.isEnded)
			_nameText.color = Color.green;
		else
			_nameText.color = Color.yellow;
	}
	
	public void Select()
	{
		isSelected = true;
		RenderColor();
	}
	
	public void Deselect()
	{
		isSelected = false;
		RenderColor();
	}
}
