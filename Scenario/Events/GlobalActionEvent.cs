using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/GlobalAction")]
public class GlobalActionEvent : MapEvent
{
    [SerializeField] private int _targetCellIndex;
    [SerializeField] private int _indexOfHostPlayer;
    [SerializeField] private GlobalActionObject _actionObject;

    [SerializeField] private ActionType _actionType;

    public override void CurrentEventActivate()
    {
        List<GroundCell> cellList = BattleMap.instance.GetAllCellsWithIndex(_targetCellIndex);
        GlobalAction action = _actionObject.GetGlobalAction();
        Player player = BattleMap.instance.playerList[_indexOfHostPlayer];

        GroundCell target = GetTargetCell(cellList, player);

        if (target != null)
            MapController.activateGlobalActionEvent.Invoke(target, player, action);
        else
            Debug.Log("ALARM NO RIGHT TARGET FOR GLOBAL ACTION EVENT");
    }

    private GroundCell GetTargetCell(List<GroundCell> cellList, Player player)
    {
        GroundCell groundCell = null;
        foreach (GroundCell cell in cellList)
        {
            if (cell.onCellObject != null)
            {
                if (_actionType == ActionType.Offensive && player.team != cell.onCellObject.team)
                {
                    groundCell = cell;
                    break;
                }

                else if (_actionType == ActionType.Defensive && player.team == cell.onCellObject.team)
                {
                    groundCell = cell;
                    break;
                }
            }
            else if (_actionType == ActionType.Create)
            {
                groundCell = cell;
                break;
            }
        }
        
        return groundCell;
    }
}
