using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/AddActionToUnit")]
public class AddActionToUnitEvent : MapEvent
{
    [SerializeField] private AbstractAction _action;
    [SerializeField] private Unit _unit;
    [SerializeField] private int _indexOfPlayer;

    public override void CurrentEventActivate()
	{
		AddActionToUnits();
	}

    private void AddActionToUnits()
    {
        foreach (NullObject obj in BattleMap.instance.playerList[_indexOfPlayer].objectList)
        {
            if (obj.Name.Equals(_unit.Name) && obj is Unit)
            {
                Unit unit = obj as Unit;
                unit.AddActionToList(_action);
            }
        }
    }
	
	protected override void PlayOnLoad(EventRowObjectSaveInfo objectSaveInfo)
	{
		CurrentEventActivate();
	}
}
