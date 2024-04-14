using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EffectIconDataBase")]
public class EffectIconDataBase : ScriptableObject
{
    [SerializeField] private List<EffectIconData> _dataBase;

    public EffectIconData GetIconData(EffectType type)
    {
        EffectIconData iconData = null;

        foreach (EffectIconData data in _dataBase)
        {
            if (data.type == type)
            {
                iconData = data;
                break;
            }
        }

        return iconData;
    }
}
