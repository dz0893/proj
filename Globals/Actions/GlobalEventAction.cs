using System.Collections.Generic;
using UnityEngine;

public class GlobalEventAction : GlobalAction
{
    private int playerIndex;

    public GlobalEventAction(GlobalEventActionObject actionObject)
	{
		this.actionObject = actionObject;
        playerIndex = actionObject.playerIndex;
		
		InitDescriptionList();
	}

    protected override void CurrentActivate(Player player, GroundCell target)
	{
	}

    protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		List<GroundCell> removedCells = new List<GroundCell>();

        foreach (GroundCell cell in areaList)
        {
            if (cell.onCellObject != cell.onCellObject.player.hero || playerIndex != BattleMap.instance.playerList.IndexOf(cell.onCellObject.player))
                removedCells.Add(cell);
        }

		return removedCells;
	}
}
