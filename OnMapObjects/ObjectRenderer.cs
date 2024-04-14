using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjectRenderer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public SpriteRenderer spriteRenderer => _spriteRenderer;

    [SerializeField] private GameObject _healthObject;
	[SerializeField] private Image _healthBar;

    [SerializeField] private Transform _effectIconContainer;
    [SerializeField] private EffectIcon _effectIconPref;

    [SerializeField] private AudioSource _voiceSource;
    public AudioSource voiceSource => _voiceSource;

    [SerializeField] private AudioSource _soundSource;
    public AudioSource soundSource => _soundSource;

    [SerializeField] private Image _turnStateRenderer;
    public Image turnStateRenderer => _turnStateRenderer;

    [SerializeField] private Image _selecter;

    private NullObject obj;
    private MaterialObject matObj;

    public void Init(NullObject obj)
    {
        this.obj = obj;

        if (obj is MaterialObject)
            matObj = obj as MaterialObject;

        _spriteRenderer.sprite = obj.idleSprite;

        SetColor();
        SetVolume();
    }

    public void SetVolume()
    {
        _voiceSource.volume = GameOptions.masterVolume * GameOptions.voiceVolume;
        _soundSource.volume = GameOptions.masterVolume * GameOptions.soundVolume;
    }

    public void PlayVoice(AudioClip clip)
    {
        _voiceSource.clip = clip;
        _voiceSource.Play();
    }

    public void PlayActionSound(AbstractAction action)
    {
        StartCoroutine(PlayActionSoundCoroutine(action.afterCastSoundDelay, action.castSound));
    }

    private IEnumerator PlayActionSoundCoroutine(float delay, AudioClip clip)
    {
        yield return new WaitForSeconds(delay);

		_soundSource.clip = clip;
        _soundSource.Play();
    }
    
    public void CleanIconContainer()
    {
        foreach (Transform child in _effectIconContainer)
            Destroy(child.gameObject);
    }

    public void AddEffectIcon(EffectIconData iconData)
    {
        EffectIcon effectIcon = Instantiate(_effectIconPref, _effectIconContainer);
        effectIcon.Init(iconData);
    }

    public void SetColor()
	{
        if (obj is MaterialObject)
        {
		    _healthBar.color = obj.player.color;
            _healthObject.SetActive(true);
        }
        else
        {
            _spriteRenderer.color = obj.player.color;
             _healthObject.SetActive(false);
        }
	}

    public void RenderHealthBar()
	{
		if (_healthBar != null)
			_healthBar.fillAmount = (float)matObj.currentHealth / matObj.currentStats.maxHealth;
	}

    public void OnSelecter()
    {
        _selecter.gameObject.SetActive(true);
    }

    public void OffSelecter()
    {
        _selecter.gameObject.SetActive(false);
    }
}
