using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogeUI : MonoBehaviour
{
	[SerializeField] private Transform _container;
	[SerializeField] private Text _nameField;
	[SerializeField] private Text _contentField;
	[SerializeField] private Image _iconField;
	[SerializeField] private JournalUI _journal;
	
	private DialogeEvent dialoge;
	private int sheetIndex;
	
	public delegate void Dialoge(DialogeEvent dialoge);
	public delegate void Close();
	
	public static Dialoge startDialoge;
	public static Close close;
	
	private void Start()
	{
		startDialoge = StartDialoge;
		close = EndDialoge;
	}	
	
	private void StartDialoge(DialogeEvent dialoge)
	{
		MapController.clear.Invoke();
		
		this.dialoge = dialoge;
		sheetIndex = 0;
		
		_container.gameObject.SetActive(true);
		
		OpenNextSheet();
	}
	
	public void ScrollDialoge()
	{
		if (sheetIndex < dialoge.contentList.Count)
			OpenNextSheet();
		else
			EndDialoge();
	}
	
	private void OpenNextSheet()
	{
		RenderDialogeSheet();
		sheetIndex++;
	}
	
	private void EndDialoge()
	{
		_container.gameObject.SetActive(false);
		
		if (dialoge != null)
			dialoge.EndEvent();
	}
	
	private void RenderDialogeSheet()
	{
		CharacterStruct character = CharacterFactory.singleton.GetCharacter(dialoge.namesList[sheetIndex]);
		_nameField.text = character.Name;
		_contentField.text = dialoge.contentList[sheetIndex];
		_iconField.sprite = character.icon;
		
		if (character.icon != null)
			_iconField.gameObject.SetActive(true);
		else
			_iconField.gameObject.SetActive(false);

		JournalSheet sheet = new JournalSheet(TurnController.turnCounter, character, dialoge.contentList[sheetIndex]);
		
		TurnController.currentPlayer.journalSheetList.Add(sheet);
	}
}
