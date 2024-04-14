using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    [SerializeField] private CharacterFactory _characterFactory;
    [SerializeField] private Transform _optionsContainer;
    [SerializeField] private Transform _mapContainer;

    [SerializeField] private AudioManager _audioManager;

    [SerializeField] private Slider _mapScaleSlider;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private Slider _voiceVolumeSlider;
    [SerializeField] private Toggle _aiFollowToggle;
    [SerializeField] private Toggle _playerFollowToggle;

    public static float mapScale { get; private set; } = 1.2f;
    public static float masterVolume { get; private set; } = 1f;
    public static float musicVolume { get; private set; } = 0.5f;
    public static float soundVolume { get; private set; } = 0.5f;
    public static float voiceVolume { get; private set; } = 0.5f;
    public static bool cameraFollowingAI { get; private set; } = false;
    public static bool cameraFollowingPlayer { get; private set; } = true;

    private Vector3 defaultMapScale = new Vector3();

    public delegate void SetScale();
    public delegate void SaveNewMapScale();
    public delegate void SetDefaultMapScale();
    public delegate void SetDefaultOnLoadScale();

    public static SetScale setScale;
    public static SaveNewMapScale saveNewMapScale;
    public static SetDefaultMapScale setDefaultMapScale;
    public static SetScale setDefaultOnLoadScale;

    public void OpenGameOptions()
    {
        _optionsContainer.gameObject.SetActive(true);
    }

    public void CloseGameOptions()
    {
        _optionsContainer.gameObject.SetActive(false);
    }

    public void Init()
    {
        _characterFactory.Init();

        setScale = SetMapScale;
        saveNewMapScale = SetScaleFromGame;
        setDefaultMapScale = NormalizeMapContainerScale;
        setDefaultOnLoadScale = SetDefaultScale;

        InitScale();
        InitVolume();
        InitCameraToggles();
    }

    private void NormalizeMapContainerScale()
    {
        _mapContainer.localScale = new Vector3(1,1,1);
    }

    public void PlayerToggleSwitch()
    {
        cameraFollowingPlayer = _playerFollowToggle.isOn;
        PlayerPrefs.SetInt("cameraFollowingPlayer", GetInt(cameraFollowingPlayer));
    }

    public void AIToggleSwitch()
    {
        cameraFollowingAI = _aiFollowToggle.isOn;
        PlayerPrefs.SetInt("cameraFollowingAI", GetInt(cameraFollowingAI));
    }

    private void InitCameraToggles()
    {
        if (PlayerPrefs.HasKey("cameraFollowingPlayer"))
        {
            cameraFollowingPlayer = GetBool(PlayerPrefs.GetInt("cameraFollowingPlayer"));
            _playerFollowToggle.isOn = cameraFollowingPlayer;
        }

        if (PlayerPrefs.HasKey("cameraFollowingAI"))
        {
            cameraFollowingAI = GetBool(PlayerPrefs.GetInt("cameraFollowingAI"));
            _aiFollowToggle.isOn = cameraFollowingAI;
        }
    }

    private bool GetBool(int value)
    {
        if (value == 0)
            return false;
        else
            return true;
    }

    private int GetInt(bool boolean)
    {
        if (boolean)
            return 1;
        else   
            return 0;
    }

    public void ChangeMasterVolume()
    {
        masterVolume = _masterVolumeSlider.value;
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.Save();
        
        InitMusicVolume();
        InitSoundVolume();
        InitVoiceVolume();
    }

    public void ChangeMusicVolume()
    {
        musicVolume = _musicVolumeSlider.value;
        _audioManager.ChangeMusicVolume(musicVolume * masterVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public void ChangeSoundVolume()
    {
        soundVolume = _soundVolumeSlider.value;
        _audioManager.ChangeSoundVolume(soundVolume * masterVolume);
        PlayerPrefs.SetFloat("soundVolume", soundVolume);
        PlayerPrefs.Save();
    }

    public void ChangeVoiceVolume()
    {
        voiceVolume = _voiceVolumeSlider.value;
        _audioManager.ChangeVoiceVolume();
        PlayerPrefs.SetFloat("voiceVolume", voiceVolume);
        PlayerPrefs.Save();
    }

    private void InitVolume()
    {
        InitMasterVolume();
        InitMusicVolume();
        InitSoundVolume();
        InitVoiceVolume();
    }

    private void InitMasterVolume()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            masterVolume = PlayerPrefs.GetFloat("masterVolume");
        }

        _masterVolumeSlider.value = masterVolume;
    }


    private void InitSoundVolume()
    {
        if (PlayerPrefs.HasKey("soundVolume"))
        {
            soundVolume = PlayerPrefs.GetFloat("soundVolume");
        }

        _audioManager.ChangeSoundVolume(soundVolume * masterVolume);
        _soundVolumeSlider.value = soundVolume;
    }

    private void InitMusicVolume()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
        }

        _audioManager.ChangeMusicVolume(musicVolume * masterVolume);
        _musicVolumeSlider.value = musicVolume;
    }

    private void InitVoiceVolume()
    {
        if (PlayerPrefs.HasKey("voiceVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("voiceVolume");
        }

        _audioManager.ChangeVoiceVolume();
        _voiceVolumeSlider.value = voiceVolume;
    }

    private void SetDefaultScale()
    {
        _mapContainer.localScale = defaultMapScale;
    }

    private void SetMapScale()
    {
        _mapContainer.localScale = new Vector3(mapScale, mapScale, _mapContainer.localScale.z);
    }

    public void ScaleMapContainer()
	{
        mapScale = (float)Math.Round(_mapScaleSlider.value, 1, MidpointRounding.AwayFromZero);
        PlayerPrefs.SetFloat("basicMapScale", mapScale);
        PlayerPrefs.Save();
	}

    private void InitScale()
    {
        defaultMapScale = _mapContainer.localScale;

        if (PlayerPrefs.HasKey("basicMapScale"))
        {
            _mapScaleSlider.value = PlayerPrefs.GetFloat("basicMapScale");
            mapScale = _mapScaleSlider.value;
        }
    }

    private void SetScaleFromGame()
    {
        mapScale = _mapContainer.transform.localScale.x;
        _mapScaleSlider.value = mapScale;

        PlayerPrefs.SetFloat("basicMapScale", mapScale);
        PlayerPrefs.Save();
    }
}
