using UnityEngine;

[CreateAssetMenu(menuName = "Events/Activator/PlayerDefeat")]
public class PlayerDefeatActivatorObject : EventActivatorObject
{
	[SerializeField] private int _playerIndex;
	public int playerIndex => _playerIndex;
	
	[SerializeField] private bool _needToDestroyUnmaterialObject;
	public bool needToDestroyUnmaterialObject => _needToDestroyUnmaterialObject;
	
	public override EventActivator GetActivator()
	{
		return new PlayerDefeatActivator(this);
	}
}
