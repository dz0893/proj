using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampainMapSettings : MonoBehaviour
{
	[SerializeField] private int _mapIndex;
	public int mapIndex => _mapIndex;
	[SerializeField] private Race _race;
	public Race race => _race;

	[SerializeField] private int _maxHeroLevel;
	public int maxHeroLevel => _maxHeroLevel;

	[SerializeField] private List<Capital> _capitalList;
	[SerializeField] private List<Unit> _heroList;
	public List<Capital> capitalList => _capitalList;
	public List<Unit> heroList => _heroList;
	
	[SerializeField] private List<int> _teamList;
	public List<int> teamList => _teamList;
	
	[SerializeField] private List<bool> _aiStateList;
	public List<bool> aiStateList => _aiStateList;
	
	[SerializeField] private int _playerIndex;
	public int playerIndex => _playerIndex;

	[SerializeField] private List<GlobalActionObject> _playerGlobalActionList;
	[SerializeField] private List<GlobalActionObject> _humanGlobalActionList;
	[SerializeField] private List<GlobalActionObject> _dwarfGlobalActionList;
	[SerializeField] private List<GlobalActionObject> _orcGlobalActionList;
	[SerializeField] private List<GlobalActionObject> _elfGlobalActionList;
	[SerializeField] private List<GlobalActionObject> _undeadGlobalActionList;

	public List<GlobalActionObject> GetPlayerGlobals(Player player)
	{
		List<GlobalActionObject> playerGlobals = new List<GlobalActionObject>();

		if (!player.isAIPlayer)
			playerGlobals = _playerGlobalActionList;
		else
		{
			if (player.race == Race.Human)
				playerGlobals = _humanGlobalActionList;
			else if (player.race == Race.Dwarf)
				playerGlobals = _dwarfGlobalActionList;
			else if (player.race == Race.Orc)
				playerGlobals = _orcGlobalActionList;
			else if (player.race == Race.Elf)
				playerGlobals = _elfGlobalActionList;
			else if (player.race == Race.Undead)
				playerGlobals = _undeadGlobalActionList;
		}

		if (playerGlobals.Count == 0 && player.hero != null)
			playerGlobals = player.hero.heroGlobalActionList;

		return playerGlobals;
	}
}
