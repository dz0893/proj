using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/ReviveUndeads")]
public class ReviveUndeadsEvent : MapEvent 
{
    [SerializeField] private int _revivedUnitsPlayerIndex;

    [SerializeField] private List<Unit> _revivedUnitList;

    private System.Random random = new System.Random();

    public override void CurrentEventActivate()
	{
		foreach (GroundCell cell in BattleMap.instance.mapCellList)
        {
            if (cell.grave.Count > 0 && cell.onCellObject == null)
                ReviveUnitOnPosition(cell);
        }

        TeamsRendererUI.renderTeams.Invoke();
	}

    private void ReviveUnitOnPosition(GroundCell position)
    {
        position.RemoveUnitFromGrave(position.grave[0]);

        BattleMap.initObject.Invoke(GetUnit(), BattleMap.instance.turnController.playerList[_revivedUnitsPlayerIndex], position);
    }

    private Unit GetUnit()
    {
        return _revivedUnitList[random.Next(_revivedUnitList.Count)];
    }
}
