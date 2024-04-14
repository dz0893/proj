using UnityEngine;

public class JournalSheet
{
	public int turn { get; private set; }
	public string name { get; private set; }
	public string description { get; private set; }
	public Sprite icon { get; private set; }
	
	public JournalSheet(int turn, CharacterStruct character, string description)
	{
		this.turn = turn;
		this.name = character.Name;
		this.description = description;
		this.icon = character.icon;
	}
}
