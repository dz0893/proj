using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Move
{
	[SerializeField] private bool _isTeleport;
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return movingDistanceFinder.GetFlyingDistance(unit, 0);
	}
	
	public override IEnumerator MakeAction(Unit unit, GroundCell target)
	{
		unit.inAction = true;
		int flyRange = target.totalMovingCost / GroundSettings.defaulTerrainMovingCost;
		
		float flyTime = GetTotalFlyingTime(flyRange);
		
		actionTime = flyTime + 0.1f;
		unit.currentMovePoints -= target.totalMovingCost;
		
		if (_isTeleport)
		{
			AnimationController.play.Invoke(this, unit.position, unit.position, unit.spriteFlipped);
			yield return new WaitForSeconds(actionTime / 2);

			unit.SetNewPosition(target);

			if (!unit.player.isAIPlayer && GameOptions.cameraFollowingPlayer || unit.player.isAIPlayer && GameOptions.cameraFollowingAI)
				CameraController.setCameraPosition.Invoke(target.transform.position);

			AnimationController.play.Invoke(this, unit.position, target, unit.spriteFlipped);
			yield return new WaitForSeconds(actionTime / 2);
		}
		else
		{
			ActionController.instance.OneCellMoving(unit, target, actionTime);

			while (unit.position != target)
			{
				yield return null;
			}
		}
		
		if (target.unmaterialOnCellObject != null)
			target.unmaterialOnCellObject.ContactWithOtherObject(unit);
		
		if (unit.currentMovePoints < 0)
			unit.currentMovePoints = 0;
		
		while (unit.position != target)
				yield return null;
		
		WasteResoursesAndEndTurn(unit);
		unit.inAction = false;
		
		yield return null;
	}
	
	private float GetTotalFlyingTime(int flyRange)
	{
		float totalFlyingTime = ActionSettings.FLYINGTIME;
		
		if (_isTeleport)
		{
			return totalFlyingTime;
		}
		
		for (int i = 1; i < flyRange; i++)
		{
			totalFlyingTime += totalFlyingTime * ActionSettings.FLYNTIMEDECREASINGKOEF / (1 + i);
		}
		
		return totalFlyingTime;
	}
	
	public override List<GroundCell> GetRoadToCell(Unit unit, GroundCell goal)
	{
		return movingDistanceFinder.GetFlyTargetTo(unit, goal);
	}
}
