using UnityEngine;

[CreateAssetMenu(menuName = "Events/Activator/DelayedOnTurnActivator")]
public class EventDelayedOnTurnActivator : EventOnTurnActivatorObject
{
	public override int turnOfActivation => _turnOfActivation + TurnController.turnCounter;
}
