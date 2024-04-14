using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Mirror;

public class PlayerListItem : NetworkBehaviour
{
	public string playerName;
	public int connectionID;
	public ulong playerSteamID;
	
	public bool isReady { get; set; }
	
	[SerializeField] private Text _playerNameText;
	//[SerializeField] private Text _playerReadyText;
	[SerializeField] private GameObject _playerReadyObject;
	[SerializeField] private GameObject _playerReadyIndicatorObject;

	public PlayerObjectController controller { get; set; }
	//
	public Capital currentCapital { get; private set; }
	
	public Race currentRace { get; private set; }

	public Unit currentHero { get; private set; }
	
	private int raceCount = Enum.GetValues(typeof(Race)).Length;
	private int heroCount = 3;
	
	public bool isRandomRace => raceCount <= _raceDropdown.value;
	public bool isRandomHero => heroCount <= _heroDropdown.value || isRandomRace;

	public Player player => controller.player;
	
	[SerializeField] private Dropdown _playerStateDropdown;
	public Dropdown playerStateDropdown => _playerStateDropdown;
	[SerializeField] private Dropdown _raceDropdown;
	public Dropdown raceDropdown => _raceDropdown;
	[SerializeField] private Dropdown _heroDropdown;
	public Dropdown heroDropdown => _heroDropdown;
	[SerializeField] private Dropdown _teamDropdown;
	public Dropdown teamDropdown => _teamDropdown;
	
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

	public List<Unit> currentHeroList { get; private set; }
	
	private void SetHeroList()
	{
		currentHeroList = _humansHeroList;

		switch (controller.raceIndex)
		{
			case 0:
				currentHeroList = _humansHeroList;
				break;
			case 1:
				currentHeroList = _dwarfsHeroList;
				break;
			case 2:
				currentHeroList = _orcsHeroList;
				break;
			case 3:
				currentHeroList = _elfsHeroList;
				break;
			case 4:
				currentHeroList = _undeadsHeroList;
				break;
		}
	}

	public void ChangeReady()
	{
		controller.ChangeReady();
	}

	public void RenderHeroList()
	{
		SetHeroList();

		for (int i = 0; i < currentHeroList.Count; i++)
		{
			_heroDropdown.options[i].text = currentHeroList[i].Name;
		}

		_heroDropdown.RefreshShownValue();
	}

	public void Render()
	{
		isReady = controller.isReady;
		SetPlayerValues();
	}

	public void RenderReadyText()
	{
		if (isReady)
		{
			_playerReadyIndicatorObject.SetActive(true);
		//	_playerReadyText.text = "Ready";
		//	_playerReadyText.color = Color.green;
		}
		else
		{
			_playerReadyIndicatorObject.SetActive(false);
		//	_playerReadyText.text = "Unready";
		//	_playerReadyText.color = Color.red;
		}
	}
	
	public void SetPlayerValues()
	{
		_playerNameText.text = playerName;
		RenderReadyText();
		RenderDropdown();
	}

	private void RenderDropdown()
	{
		_raceDropdown.value = controller.raceIndex;
		_teamDropdown.value = controller.team;
		_playerStateDropdown.value = controller.playerState;
		
		RenderStateDropDown();
		RenderHeroDropDown();
	}
	
	private void RenderStateDropDown()
	{
		if (_playerStateDropdown.value == 0)
		{
			_heroDropdown.gameObject.SetActive(true);
			_raceDropdown.gameObject.SetActive(true);
			_teamDropdown.gameObject.SetActive(true);

		//	_playerReadyText.gameObject.SetActive(true);
			_playerReadyObject.SetActive(true);
		}
		else
		{
			_heroDropdown.gameObject.SetActive(false);
			_raceDropdown.gameObject.SetActive(false);
			_teamDropdown.gameObject.SetActive(false);

		//	_playerReadyText.gameObject.SetActive(false);
			_playerReadyObject.SetActive(false);
		}
	}

	private void RenderHeroDropDown()
	{
		_heroDropdown.value = controller.heroIndex;
		RenderHeroList();

		if (!isRandomRace && _playerStateDropdown.value == 0)
		{
			_heroDropdown.gameObject.SetActive(true);
		}
		else
		{
			_heroDropdown.gameObject.SetActive(false);
		}
	}

	public void InitTeamDropDown()
	{
		_teamDropdown.ClearOptions();

		for (int i = 0; i < controller.manager.maxPlayerCount; i++)
		{
			_teamDropdown.options.Add(new Dropdown.OptionData(){ text = i.ToString() });
		}
	}

	public void SetPlayerContent()
	{
		currentRace = (Race)controller.currentRaceIndex;
		SetRaceContent();
		currentHero = currentHeroList[controller.currentHeroIndex];
		controller.InitPlayer();
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
	}
}
