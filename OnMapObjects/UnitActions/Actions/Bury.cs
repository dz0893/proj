using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bury : AbstractAction
{
//	public MovingDistanceFinder movingDistanceFinder { get; private set; } = new MovingDistanceFinder();
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();

	[SerializeField] private Unit _body;
	
	[SerializeField] private bool _needFullMovePoints;
	public bool needFullMovePoints => _needFullMovePoints;

	public override bool endedTurnAction => true;
	
	public override ActionType actionType => ActionType.Defensive;
	public override ActionRange range => ActionRange.Melee;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		//return movingDistanceFinder.GetMoveDistance(unit);

		return actionDistanceFinder.GetBuryDistance(unit, this);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		//AudioManager.playSound.Invoke(afterCastSoundDelay, castSound);
		
		unit.inAction = true;
		yield return new WaitForSeconds(actionTime);
		
		Unit body = Instantiate(_body, unit.map.objectMap.transform);
		body.Init(target, unit.player);
		body.Death();
		
		WasteResoursesAndEndTurn(unit);
		unit.inAction = false;
	}

	protected override bool CurrentActivateCheck(Unit unit)
	{
		if (_needFullMovePoints && unit.currentMovePoints < unit.currentStats.maxMovePoints)
			return false;
		else
			return true;
	}

	protected override void SetCurrentDescription(Unit unit)
	{
		if (_needFullMovePoints)
			AddStringToRequiresList(UISettings.NeedFullMovePoints);
	}
}
