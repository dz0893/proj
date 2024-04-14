using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : AbstractAction
{
	public MovingDistanceFinder movingDistanceFinder { get; private set; } = new MovingDistanceFinder();
	
	public override bool endedTurnAction => false;
	
	public override ActionType actionType => ActionType.Moving;
	public override ActionRange range => ActionRange.Ranged;
	
	[SerializeField] private int _teleportDistance;
	public int teleportDistance => _teleportDistance;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return movingDistanceFinder.GetFlyingDistance(unit, _teleportDistance);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		actionTime = ActionSettings.FLYINGTIME + 0.1f;
		AnimationController.play.Invoke(this, unit.position, unit.position, unit.spriteFlipped);
		
		yield return new WaitForSeconds(ActionSettings.FLYINGTIME / 2);
		
		unit.SetNewPosition(target);
		
		CameraController.setCameraPosition.Invoke(target.transform.position);
		AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
		
		yield return new WaitForSeconds(ActionSettings.FLYINGTIME / 2);
		
		WasteResoursesAndEndTurn(unit);
		
		if (target.unmaterialOnCellObject != null)
			target.unmaterialOnCellObject.ContactWithOtherObject(unit);
			
		unit.inAction = false;
		yield return null;
	}
}
