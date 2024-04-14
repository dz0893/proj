using UnityEngine;

[CreateAssetMenu(menuName = "Events/Activator/OnTurnActivator")]
public class EventOnTurnActivatorObject : EventActivatorObject
{
	[SerializeField] protected int _turnOfActivation;
	public virtual int turnOfActivation => _turnOfActivation;
	
	public override EventActivator GetActivator()
	{
		return new OnTurnEventActivator(this);
	}
}
