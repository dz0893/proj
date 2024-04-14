public class OnTurnEventActivator : EventActivator
{
	private int turnOfActivate;
	
	public OnTurnEventActivator(EventOnTurnActivatorObject activatorObject)
	{
		SetDefaultActivatorData(activatorObject);
		turnOfActivate = activatorObject.turnOfActivation;
		
		Player.PlayerStartedTurn.AddListener(CheckPlayerForStartingTurnOfActivation);
	}
	
	private void CheckPlayerForStartingTurnOfActivation(Player player)
	{
		if (!player.isAIPlayer)
			TryToActivateEvent();
	}
	
	protected override bool CheckForActiveEvent()
	{
		if (turnOfActivate == TurnController.turnCounter)
			return true;
		else
			return false;
	}
	
	public override void RemoveListener()
	{
		Player.PlayerStartedTurn.RemoveListener(CheckPlayerForStartingTurnOfActivation);
	}
}
