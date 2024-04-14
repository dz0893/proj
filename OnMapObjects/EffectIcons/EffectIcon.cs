using UnityEngine;

public class EffectIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private EffectType _type;

    public void Init (EffectIconData iconData)
    {
        _spriteRenderer.sprite = iconData.icon;
        _type = iconData.type;
    }
}
