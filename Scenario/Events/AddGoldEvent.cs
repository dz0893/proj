using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/AddGold")]
public class AddGoldEvent : MapEvent
{
	[SerializeField] private int _goldValue;
	
	public override void CurrentEventActivate()
	{
		player.WasteGold(-_goldValue);
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.currentPlayer);
	}
}
