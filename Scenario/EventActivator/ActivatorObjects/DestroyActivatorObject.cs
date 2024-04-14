using UnityEngine;

[CreateAssetMenu(menuName = "Events/Activator/DestroyObject")]
public class DestroyActivatorObject : EventActivatorObject
{
	[SerializeField] private NullObject _destroedObject;
	public NullObject destroedObject => _destroedObject;
	
	[SerializeField] private int _playerIndex;
	public int playerIndex => _playerIndex;
	
	public override EventActivator GetActivator()
	{
		return new DestroyActivator(this);
	}
}
