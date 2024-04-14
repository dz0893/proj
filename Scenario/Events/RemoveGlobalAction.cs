using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/RemoveGlobalAction")]
public class RemoveGlobalAction : MapEvent
{
    [SerializeField] private GlobalActionObject _action;
	
	public override void CurrentEventActivate()
	{
		player.RemoveGlobal(_action);
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.currentPlayer);
	}

    protected override void PlayOnLoad(EventRowObjectSaveInfo objectSaveInfo)
	{
		player.RemoveGlobal(_action);
	}
}
