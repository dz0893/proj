using UnityEngine;

[CreateAssetMenu(menuName = "Events/Activator/CreateObject")]
public class CreateObjectEventActivatorObject : EventActivatorObject
{
    [SerializeField] private NullObject _createdObject;
    public NullObject createdObject => _createdObject;

    [SerializeField] private int _playerIndex;
	public int playerIndex => _playerIndex;

    [SerializeField] private bool _multipleTimeActivating;
    public override bool multipleTimeActivating => _multipleTimeActivating;

    public override EventActivator GetActivator()
	{
		return new CreateObjectActivator(this);
	}
}
