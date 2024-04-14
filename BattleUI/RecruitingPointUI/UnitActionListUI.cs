using System.Collections.Generic;
using UnityEngine;

public class UnitActionListUI : MonoBehaviour
{
    [SerializeField] private List<CurrentActionUI> _actionList;

    private Aura aura;

    public void Render(Unit unit)
    {
        Clean();
        Init(unit);
    }

    private void Clean()
    {
        foreach(CurrentActionUI action in _actionList)
            action.gameObject.SetActive(false);
    }

    private void Init(Unit unit)
    {
        int i = 0;

        foreach (AbstractAction action in unit.actionList)
        {
            RenderCurrentAction(action.GetActionData(unit), unit, i);
            i++;
        }

        if (unit.startedTurnAction != null)
        {
            RenderCurrentAction(unit.startedTurnAction.GetActionData(unit), unit, i);
            i++;
        }

        if (unit.auraData != null)
        {
            aura = new Aura(unit.auraData, unit);
            RenderCurrentAction(aura, unit, i);
            i++;
        }
    }

    private void RenderCurrentAction(IActionDescription action, Unit unit, int actionIndex)
    {
        _actionList[actionIndex].gameObject.SetActive(true);
        _actionList[actionIndex].Render(action, unit);
    }
}
