using System.Collections.Generic;
using UnityEngine;

public class IconSetter : MonoBehaviour
{
    [SerializeField] private EffectIconDataBase _effectIconDataBase;

    public delegate void SetEffects(Unit unit);
    public static SetEffects setEffects;

    private void Awake()
    {
        setEffects = SetEffectIcons;
    }

    private List<EffectType> GetEffectTypeList(Unit unit)
    {
        List<EffectType> currentEffectTypeList = new List<EffectType>();

        if (!unit.attackIsRecharget)
            currentEffectTypeList.Add(EffectType.NeedToRecharges);

        foreach (CurrentEffect effect in unit.activeEffectList)
        {
            if (!currentEffectTypeList.Contains(effect.effectType) && effect.effectType != EffectType.None)
            {
                currentEffectTypeList.Add(effect.effectType);
            }
        }

        foreach (OnCellEffect effect in unit.onCellEffectList)
        {
            if (!currentEffectTypeList.Contains(effect.effectType) && effect.effectType != EffectType.None)
            {
                currentEffectTypeList.Add(effect.effectType);
            }
        }

        return currentEffectTypeList;
    }

    private void SetEffectIcons(Unit unit)
    {
        List<EffectType> currentEffectTypeList = GetEffectTypeList(unit);

        unit.objectRenderer.CleanIconContainer();

        foreach (EffectType effectType in currentEffectTypeList)
        {
            unit.objectRenderer.AddEffectIcon(_effectIconDataBase.GetIconData(effectType));
        }
    }
}
