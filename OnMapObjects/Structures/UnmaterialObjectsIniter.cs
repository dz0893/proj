using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnmaterialObjectsIniter : GroundChanger
{
	[SerializeField] private UnmaterialObject _initedObject;
	
	[SerializeField] private bool _isGroundChanger;
	
	public override void StartTurn()
	{
		if (_isGroundChanger)
			SetGround(position);
		
		turnEnded = false;
	}

	protected override void CurrentStructureInit(GroundCell positionCell)
	{
		if (_isGroundChanger)
			SetGround(positionCell);
		
		InitRecruitPoints(positionCell);
	}
	
	protected void InitRecruitPoints(GroundCell positionCell)
	{
		foreach (GroundCell cell in positionCell.closestCellList)
		{
			if (cell.terrainType != TerrainType.Water && cell.terrainType != TerrainType.Mountain)
			{
				UnmaterialObject initedObject = InitUnmaterialObject(cell);
			}
		}
	}
	
	public UnmaterialObject InitUnmaterialObject(GroundCell position)
	{
		UnmaterialObject obj = Instantiate(_initedObject, map.objectMap.transform);
		obj.Init(position, player);
		obj.initer = this;
		return obj;
	}
	
	public override void LocalDeath()
	{
		currentHealth = 0;
		
		foreach (GroundCell cell in position.closestCellList)
		{
			if (cell.unmaterialOnCellObject != null && cell.unmaterialOnCellObject.initer == this)
				cell.unmaterialOnCellObject.Death();
		}
		
		position.onCellObject = null;
		position = null;
	}
}
