using UnityEngine;

[System.Serializable]
public class EffectIconData
{
    [SerializeField] private Sprite _icon;
    public Sprite icon => _icon;

    [SerializeField] private EffectType _type;
    public EffectType type => _type;
}
