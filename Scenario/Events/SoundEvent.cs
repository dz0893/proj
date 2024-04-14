using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/Sound")]
public class SoundEvent : MapEvent
{
    [SerializeField] private AudioClip _soundClip;

//    protected override float eventTime => _soundClip.currentClipLength;

    public override void CurrentEventActivate()
	{
		AudioManager.playSound.Invoke(0, _soundClip);
	}
}
