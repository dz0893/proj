using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/RemoveActivator")]
public class RemoveActivatorEvent : MapEvent
{
	[SerializeField] private int _activatorIndex;
	
	public override void CurrentEventActivate()
	{
		Scenario.removeActivator.Invoke(_activatorIndex);
	}
	
	protected override void PlayOnLoad(EventRowObjectSaveInfo objectSaveInfo)
	{
		CurrentEventActivate();
	}
}
