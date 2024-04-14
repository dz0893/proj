using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/SwitchBlackScreenState")]
public class SwitchBlackScreenStateEvent : MapEvent
{
    [SerializeField] private bool _screenState;

    public override void CurrentEventActivate()
	{
        if (_screenState)
		    PlayerUI.onScreen.Invoke();
        else
            PlayerUI.offScreen.Invoke();
	}
}
