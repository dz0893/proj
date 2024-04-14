using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/AddGlobalAction")]
public class AddGlobalAction : MapEvent
{
    [SerializeField] private GlobalActionObject _action;
	
	public override void CurrentEventActivate()
	{
		player.AddGlobal(_action);
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.currentPlayer);
	}

    protected override void PlayOnLoad(EventRowObjectSaveInfo objectSaveInfo)
	{
		player.AddGlobal(_action);
	}
}
