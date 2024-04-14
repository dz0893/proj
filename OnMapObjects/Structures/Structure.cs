using System.Collections.Generic;
using UnityEngine;

public class Structure : MaterialObject
{
	public override bool isMovable => false;
	public override bool goingToGraveAtDeath => false;
	public override bool isMech => true;
	public override bool isUndead => false;
	
	public override void LocalDeath()
	{
		currentHealth = 0;
		
		position.onCellObject = null;
		position = null;
	}
	
	protected override void LocalInit(GroundCell positionCell)
	{
		InitStats();
		InitInfo(new StructureInfo());

		turnEnded = true;
		
		CurrentStructureInit(positionCell);
	}
	
	public override void SetNewPosition(GroundCell newPosition)
	{
		if (position != null)
			position.onCellObject = null;
		
		newPosition.onCellObject = this;
		position = newPosition;
		
		Vector3 pos = new Vector3(newPosition.transform.position.x, 
						newPosition.transform.position.y, 
						GroundSettings.OBJECTZPOSITION);
		
		transform.position = pos;
		
		PositionWasSetted.Invoke(position, this);
	}
	
	protected virtual void CurrentStructureInit(GroundCell positionCell) {}
}
