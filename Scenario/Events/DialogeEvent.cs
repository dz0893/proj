using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/Dialoge")]
public class DialogeEvent : MapEvent
{
	[SerializeField] private List<CharacterName> _namesList;
	[SerializeField] private List<string> _contentList;
	//[SerializeField] private List<Sprite> _iconList;
	
	public List<CharacterName> namesList => _namesList;
	public List<string> contentList => _contentList;
	//public List<Sprite> iconList => _iconList;
	
	protected override bool momentalEvent => false;
	
	public override void CurrentEventActivate()
	{
		DialogeUI.startDialoge.Invoke(this);
	}
	
	protected override void PlayOnLoad(EventRowObjectSaveInfo objectSaveInfo)
	{
		for (int i = 0; i < _namesList.Count; i++)
		{
			JournalSheet sheet = new JournalSheet(objectSaveInfo.turnOfActivation, CharacterFactory.singleton.GetCharacter(_namesList[i]), _contentList[i]);
		
			player.journalSheetList.Add(sheet);
		}
	}
}
