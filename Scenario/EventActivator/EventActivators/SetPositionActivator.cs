using UnityEngine;
using System.Collections.Generic;

public class SetPositionActivator : EventActivator
{
	private int positionIndex;
	private List<int> playerIndex;
	
	public SetPositionActivator(SetPositionActivatorObject activatorObject)
	{
		SetDefaultActivatorData(activatorObject);
		positionIndex = activatorObject.positionIndex;
		playerIndex = activatorObject.playerIndex;
		
		MaterialObject.PositionWasSetted.AddListener(CheckPositionForIndex);
	}
	
	private void CheckPositionForIndex(GroundCell position, MaterialObject obj)
	{
		if (position.index == positionIndex
		&& playerIndex.Contains(BattleMap.instance.playerList.IndexOf(obj.player)))//TurnController.currentPlayer)))
		{
			TryToActivateEvent();
		}
	}
	
	protected override bool CheckForActiveEvent()
	{
		return true;
	}
	
	public override void RemoveListener()
	{
		MaterialObject.PositionWasSetted.RemoveListener(CheckPositionForIndex);
	}
}
