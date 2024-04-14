using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCell : MonoBehaviour
{
	public Capital currentCapital { get; private set; }
	
	public Race currentRace { get; private set; }
	
	public Unit currentHero { get; private set; }
	
	public Player player { get; private set; } = new Player();
	
	private List<Unit> currentHeroList;
	
	private System.Random random = new System.Random();
	
	private int raceCount = Enum.GetValues(typeof(Race)).Length;
	
	private bool isRandomRace;
	
	public int goldHandicap { get; private set; }
	public int oreHandicap { get; private set; }

	public int team { get; private set; }
	
	[SerializeField] private Text _playerName;
	
	[SerializeField] private Dropdown _playerTypeDropdown;
	public Dropdown playerTypeDropdown => _playerTypeDropdown;
	[SerializeField] private Dropdown _raceDropdown;
	[SerializeField] private Dropdown _heroDropdown;
	[SerializeField] private Dropdown _teamDropdown;
	public Dropdown teamTypeDropdown => _teamDropdown;
	
	[SerializeField] private List<Unit> _humansHeroList;
	[SerializeField] private List<Unit> _dwarfsHeroList;
	[SerializeField] private List<Unit> _orcsHeroList;
	[SerializeField] private List<Unit> _elfsHeroList;
	[SerializeField] private List<Unit> _undeadsHeroList;
	
	[SerializeField] private Capital _humanCapital;
	[SerializeField] private Capital _dwarfCapital;
	[SerializeField] private Capital _orcCapital;
	[SerializeField] private Capital _elfCapital;
	[SerializeField] private Capital _undeadCapital;
	
	public void InitPlayer()
	{
		if (_playerTypeDropdown.value != 4)
		{
			player.Init(team);
			SetRace();
			SetPlayerType();
			player.ChangeGoldIncome(goldHandicap);
			player.ChangeOreIncome(oreHandicap);
		}
		else
			player = null;
	}
	
	public void RenderPlayerName(int playerIndex)
	{
		_playerName.text = UISettings.Player + playerIndex;
	}
	
	public void SetTeamDropdown(int countOfPlayers, int currentIndex)
	{
		for (int i = 0; i < countOfPlayers; i++)
		{
			_teamDropdown.options.Add(new Dropdown.OptionData(){ text = i.ToString() });
		}
		_teamDropdown.value = currentIndex;
	}
	
	public void SetTeam()
	{
		team = _teamDropdown.value;
	}
	
	public void SetPlayerType()
	{
		if (_playerTypeDropdown.value == 0)
		{
			player.SetNotComputerPlayer();
			goldHandicap = 0;
			oreHandicap = 0;
		}
		else if (_playerTypeDropdown.value == 1)
		{
			player.SetAI(true);
			goldHandicap = 0;
			oreHandicap = 0;
		}
		else if (_playerTypeDropdown.value == 2)
		{
			player.SetAI(true);
			goldHandicap = MapSettings.normalGoldHandicap;
			oreHandicap = MapSettings.normalOreHandicap;
		}
		else if (_playerTypeDropdown.value == 3)
		{
			player.SetAI(true);
			goldHandicap = MapSettings.highGoldHandicap;
			oreHandicap = MapSettings.highOreHandicap;
		}
	}
	
	public void SetRace()
	{
		if (_raceDropdown.value >= raceCount)
		{
			currentRace = (Race)random.Next(raceCount);
			isRandomRace = true;
		}
		else
		{
			currentRace = (Race)_raceDropdown.value;
			isRandomRace = false;
		}
		
		SetRaceContent();
		RenderHeroDropdown();
		SetHero();
	}
	
	public void SetHero()
	{
		if (isRandomRace || _heroDropdown.value >= currentHeroList.Count)
		{
			currentHero = currentHeroList[random.Next(currentHeroList.Count)];
		}
		else
		{
			currentHero = currentHeroList[_heroDropdown.value];
		}
		
		player.hero = currentHero;
		player.InitGlobals(currentHero.heroGlobalActionList);
	}
	
	private void SetRaceContent()
	{
		if (currentRace == Race.Human)
		{
			currentHeroList = _humansHeroList;
			currentCapital = _humanCapital;
		}
		
		else if (currentRace == Race.Dwarf)
		{
			currentHeroList = _dwarfsHeroList;
			currentCapital = _dwarfCapital;
		}
		
		else if (currentRace == Race.Orc)
		{
			currentHeroList = _orcsHeroList;
			currentCapital = _orcCapital;
		}
		
		else if (currentRace == Race.Elf)
		{
			currentHeroList = _elfsHeroList;
			currentCapital = _elfCapital;
		}
		else if (currentRace == Race.Undead)
		{
			currentHeroList = _undeadsHeroList;
			currentCapital = _undeadCapital;
		}
		
		player.capital = currentCapital;
		player.race = currentCapital.race;
	}
	
	private void RenderHeroDropdown()
	{
		if (_raceDropdown.value >= raceCount)
			_heroDropdown.gameObject.SetActive(false);
		else
		{
			_heroDropdown.gameObject.SetActive(true);
		
			for (int i = 0; i < currentHeroList.Count; i++)
			{
				_heroDropdown.options[i].text = currentHeroList[i].Name;
			}
			
			_heroDropdown.RefreshShownValue();
		}
	}
}
