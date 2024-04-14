using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/aiBehaviorSwitcher")]
public class SwitchAIBehaviorEvent : MapEvent
{
	[SerializeField] private int _indexOfPlayer;
	[SerializeField] private bool _newAIState = true;
	
	private AIPlayer aiPlayer => BattleMap.instance.turnController.playerList[_indexOfPlayer].aiPlayer;
	
	public override void CurrentEventActivate()
	{
		aiPlayer.active = _newAIState;
	}
}
