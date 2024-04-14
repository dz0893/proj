using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/AddActivator")]
public class AddActivatorEvent : MapEvent
{
	[SerializeField] private EventActivatorObject _activatorObject;
	
	public override void CurrentEventActivate()
	{
		Scenario.addActivatorToList.Invoke(_activatorObject);
	}
	
	protected override void PlayOnLoad(EventRowObjectSaveInfo objectSaveInfo)
	{
		CurrentEventActivate();
	}
}
