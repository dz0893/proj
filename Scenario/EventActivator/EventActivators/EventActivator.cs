using UnityEngine;
using UnityEngine.Events;

public abstract class EventActivator : IEventRowObject
{
	public EventActivatorObject activatorObject { get; private set; }

	private MapEvent mapEvent => activatorObject.mapEvent;
	
	public int turnOfActivation { get; private set; }
	
	public int activatorIndex => activatorObject.activatorIndex;
	
	public int index => activatorIndex;
	
	private bool multipleTimeActivating => activatorObject.multipleTimeActivating;

	protected virtual bool CheckForActiveEvent() { return true; }
	
	public bool isEnded { get; private set; }
	public bool isFailed => false;
	public bool isMission => false;
	
	public static UnityEvent<IEventRowObject> eventWasPlayed = new UnityEvent<IEventRowObject>();
	
	protected void SetDefaultActivatorData(EventActivatorObject activatorObject)
	{
		this.activatorObject = activatorObject;
	}
	
	protected void TryToActivateEvent()
	{
		if (CheckForActiveEvent() && !isEnded)
		{
			turnOfActivation = TurnController.turnCounter;
			eventWasPlayed.Invoke(this);
			mapEvent.TryToActivateEvent(turnOfActivation);

			if (!multipleTimeActivating)
			{
				isEnded = true;
				RemoveListener();
			}
		}
	}
	
	public void ActivateOnLoad(EventRowObjectSaveInfo objectSaveInfo)
	{
		turnOfActivation = objectSaveInfo.turnOfActivation;
		mapEvent.PlayEventRowOnLoad(objectSaveInfo);
		
		if (!multipleTimeActivating)
		{
			isEnded = true;
			RemoveListener();
		}
	}
	
	public abstract void RemoveListener();
}
