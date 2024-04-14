using UnityEngine;

public class PlayerDefeatActivator : EventActivator
{
	private int playerIndex;
	private bool needToDestroyUnmaterialObject;
	
	private Player player => BattleMap.instance.playerList[playerIndex];
	
	public PlayerDefeatActivator(PlayerDefeatActivatorObject activatorObject)
	{
		SetDefaultActivatorData(activatorObject);
		playerIndex = activatorObject.playerIndex;
		needToDestroyUnmaterialObject = activatorObject.needToDestroyUnmaterialObject;
		
		NullObject.ObjectDied.AddListener(CheckPlayerForDefeat);
	}
	
	private void CheckPlayerForDefeat(NullObject obj)
	{
		if (BattleMap.instance.playerList.Count >= playerIndex)
		{
			if (needToDestroyUnmaterialObject && player.objectList.Count == 0 
			|| (player.capital != null && player.capital.isDead))
			{
				TryToActivateEvent();
			}
		}
	}
	
	public override void RemoveListener()
	{
		NullObject.ObjectDied.RemoveListener(CheckPlayerForDefeat);
	}
}