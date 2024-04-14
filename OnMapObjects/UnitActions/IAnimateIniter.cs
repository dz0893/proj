using UnityEngine;

public interface IAnimateIniter
{
    public AnimationClip onCasterAnimation { get; }
    public AnimationClip flyAnimation { get; }
    public AnimationClip contactAnimation { get; }

    public bool needToRotateOnCasterAnimationToTarget { get; }
    public bool shotFlyingWithCurve { get; }
    public float startedOnCasterAnimationAngle { get; }
    
    public AudioClip castSound { get; }
    public AudioClip contactSound { get; }

    public float afterCastSoundDelay { get; }
    
    public void PlaySound(AudioClip sound);
}
