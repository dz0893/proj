using System.Collections.Generic;
using UnityEngine;

public abstract class UnmaterialObject : NullObject
{
	[SerializeField] private bool _destroedByTerramorfing = true;
	public bool destroedByTerramorfing => _destroedByTerramorfing && initer == null;
	public NullObject initer { get; set; }
	
	public override void SetNewPosition(GroundCell position)
	{
		if (position.unmaterialOnCellObject != null)
		{
			if (position.unmaterialOnCellObject.initer == null)
				position.unmaterialOnCellObject.Death();
			else
			{
				Death();
				return;
			}
		}
		
		position.unmaterialOnCellObject = this;
		this.position = position;
		
		Vector3 pos = new Vector3(position.transform.position.x, position.transform.position.y, GroundSettings.UNMATERIALZPOSITION);
		
		transform.position = pos;
	}

	protected override void SetDirectionOnInit(Player player) {}
	
	public virtual void ContactWithOtherObject(NullObject obj) {}
	
	public override void LocalDeath()
	{
		position.unmaterialOnCellObject = null;
	}
	
	protected override void LocalInit(GroundCell positionCell)
	{
		InitInfo(new UnmaterialObjectInfo());
		turnEnded = true;
	}

	protected override void CleanPositionOfThisObject()
	{
		position.unmaterialOnCellObject = null;
	}
	
/*	protected override void SetColor()
	{
		GetComponent<SpriteRenderer>().color = player.color;
	}*/
}
