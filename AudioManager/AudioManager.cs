using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField] private AudioSource _musicSource;
	[SerializeField] private AudioSource _soundSource;
	
	[SerializeField] private AudioClip _mainTheme;

	[SerializeField] private List<AudioClip> _humansClipList;
	[SerializeField] private List<AudioClip> _dwarfsClipList;
	[SerializeField] private List<AudioClip> _orcsClipList;
	[SerializeField] private List<AudioClip> _elfsClipList;
	[SerializeField] private List<AudioClip> _undeadsClipList;
	[SerializeField] private List<AudioClip> _neutralClipList;
	
	private List<AudioClip> fullClipList = new List<AudioClip>();
	private System.Random random = new System.Random();
	private int clipIndex;
	
	private float currentClipLength;
	
	public delegate void SetMusic();
	public delegate void SetMainMenuMusic();
	public delegate void PlaySound(float delay, AudioClip clip);
	public delegate void PlayUnitActionSound(Unit unit, float delay, AudioClip clip);
	public delegate void PlayVoice(AudioClip clip);
	
	public static SetMusic setMusic;
	public static SetMainMenuMusic setMainMenuMusic;
	public static PlaySound playSound;
	
	private void Awake()
	{
		setMusic = InitClipListAndPlay;
		setMainMenuMusic = SetMainTheme;
		playSound = StartSoundCoroutine;
	}

	public void ChangeMusicVolume(float value)
	{
		_musicSource.volume = value;
	}

	public void ChangeSoundVolume(float value)
	{
		_soundSource.volume = value;

		ChangeVoiceVolume();
	}

	public void ChangeVoiceVolume()
	{
		if (BattleMap.instance != null)
		{
			foreach (NullObject obj in BattleMap.instance.objectList)
			{
				obj.objectRenderer.SetVolume();
			}
		}
	}

	private void StartSoundCoroutine(float delay, AudioClip clip)
	{
		StartCoroutine(PlaySoundWithDelay(delay, clip));
	}

	private IEnumerator PlaySoundWithDelay(float delay, AudioClip clip)
	{
		yield return new WaitForSeconds(delay);

		_soundSource.clip = clip;
		_soundSource.Play();
	}

	private void InitClipListAndPlay()
	{
		SetFullClipList();
	}
	
	private void Update()
	{
		if (!_musicSource.isPlaying && fullClipList.Count != 0)
		{
			SetNextClip();
		}
	}

	private void StopTrackList()
	{
		_musicSource.Stop();
		fullClipList = new List<AudioClip>();
		clipIndex = 0;
	}
	
	private void SetMainTheme()
	{
		StopTrackList();
		fullClipList.Add(_mainTheme);
		SetNextClip();
	}

	private void SetNextClip()
	{
		_musicSource.clip = fullClipList[clipIndex];
		_musicSource.Play();
		
		clipIndex++;
		
		if (clipIndex >= fullClipList.Count)
			clipIndex = 0;
	}
	
	private void SetFullClipList()
	{
		StopTrackList();
		List<AudioClip> unsortedClipList = new List<AudioClip>();
		
		clipIndex = 0;
		int index = 0;
		
		foreach (Race race in GetRaceList())
			AddCurrentClipListToTotalUnsortedList(unsortedClipList, GetRaseClipList(race));

		AddCurrentClipListToTotalUnsortedList(unsortedClipList, _neutralClipList);

		while (unsortedClipList.Count > 0)
		{
			index = random.Next(unsortedClipList.Count);
			
			fullClipList.Add(unsortedClipList[index]);
			unsortedClipList.Remove(unsortedClipList[index]);
		}
	}
	
	private void AddCurrentClipListToTotalUnsortedList(List<AudioClip> totalClipList, List<AudioClip> currentClipList)
	{
		foreach (AudioClip clip in currentClipList)
			totalClipList.Add(clip);
	}

	private List<Race> GetRaceList()
	{
		List<Race> raceList = new List<Race>();

		foreach (Player player in BattleMap.instance.turnController.playerList)
		{
			if (!raceList.Contains(player.race))
				raceList.Add(player.race);
		}

		return raceList;
	}

	private List<AudioClip> GetRaseClipList(Race race)
	{
		List<AudioClip> clipList = new List<AudioClip>();
		List<AudioClip> copyClipList = new List<AudioClip>();
		
		if (race == Race.Human)
			clipList = _humansClipList;
		if (race == Race.Dwarf)
			clipList = _dwarfsClipList;
		if (race == Race.Orc)
			clipList = _orcsClipList;
		if (race == Race.Elf)
			clipList = _elfsClipList;
		if (race == Race.Undead)
			clipList = _undeadsClipList;
		
		if (clipList.Count == 0)
			clipList = _orcsClipList;
		
		foreach (AudioClip clip in clipList)
			copyClipList.Add(clip);
		
		return copyClipList;
	}
}
