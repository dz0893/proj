using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/UnitAction")]
public class UnitActionEvent : MapEvent
{
    private MovingDistanceFinder movingDistanceFinder = new MovingDistanceFinder();

    [SerializeField] private Unit _unit;
    [SerializeField] private AbstractAction _action;

    [SerializeField] private int _indexOfUnitHost;
    [SerializeField] private int _targetCellIndex;
    [SerializeField] private bool _isFlipAction;
    [SerializeField] private bool _isMoveAction;

    private Unit unit;

    public override void CurrentEventActivate()
	{
        EventAction();
    }

    private void EventAction()
	{
        GroundCell target = BattleMap.instance.GetCellWithIndex(_targetCellIndex);

        if (unit != null && target != null)
        {
            if (_action != null)
                PlayAnim(unit, target);

            if (_isMoveAction)
                UnitMoving(unit, target);

            if (_isFlipAction)
                FlipUnit(unit);
        }
	}

    private void PlayAnim(Unit unit, GroundCell target)
    {
        AnimationController.play.Invoke(_action, unit.position, target, unit.spriteFlipped);
    }

    private void UnitMoving(Unit unit, GroundCell target)
    {
        unit.SetMapEventMovingRoad(movingDistanceFinder.GetClosestRoadBetweenCells(unit.position, target));
        ActionController.instance.MoveUnitInScenarioEvent(unit, target);
    }

    protected override void SetCameraPosition()
	{
		SetUnit();

		if (unit != null)
			CameraController.setCameraPosition.Invoke(unit.transform.position);
	}

    private void SetUnit()
    {
        unit = null;

        foreach (NullObject obj in BattleMap.instance.playerList[_indexOfUnitHost].objectList)
		{
			if (obj.index == _unit.index)
			{
				unit = obj as Unit;
				break;
			}
		}
    }

    private void FlipUnit(Unit unit)
    {
        if (unit.spriteFlipped)
            unit.RotateToLeft();
        else
            unit.RotateToRight();
    }
}
