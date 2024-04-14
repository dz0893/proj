using UnityEngine;

public class DestroyActivator : EventActivator
{
	public NullObject destroedObject { get; private set; }
	public int playerIndex { get; private set; }
	
	public DestroyActivator(DestroyActivatorObject activatorObject)
	{
		SetDefaultActivatorData(activatorObject);
		destroedObject = activatorObject.destroedObject;
		playerIndex = activatorObject.playerIndex;
		
		NullObject.ObjectDied.AddListener(CheckDestroingObject);
	}
	
	private void CheckDestroingObject(NullObject obj)
	{
		if (obj.Name.Equals(destroedObject.Name) && obj.player == BattleMap.instance.playerList[playerIndex])
			TryToActivateEvent();
	}
	
	public override void RemoveListener()
	{
		NullObject.ObjectDied.RemoveListener(CheckDestroingObject);
	}
}
