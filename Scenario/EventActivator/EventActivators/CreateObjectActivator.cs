public class CreateObjectActivator : EventActivator
{
    public NullObject createdObject { get; private set; }
    public int playerIndex { get; private set; }

    public bool multipleTimeActivating { get; private set; }

    public CreateObjectActivator(CreateObjectEventActivatorObject activatorObject)
	{
		SetDefaultActivatorData(activatorObject);
		createdObject = activatorObject.createdObject;
		playerIndex = activatorObject.playerIndex;
        multipleTimeActivating = activatorObject.multipleTimeActivating;
		
		NullObject.ObjectInited.AddListener(CheckCreatedObject);
	}

    public override void RemoveListener()
	{
		NullObject.ObjectInited.RemoveListener(CheckCreatedObject);
	}

    private void CheckCreatedObject(NullObject obj)
    {
        if (obj.Name.Equals(createdObject.Name) && obj.player == BattleMap.instance.playerList[playerIndex])
			TryToActivateEvent();
    }
}
