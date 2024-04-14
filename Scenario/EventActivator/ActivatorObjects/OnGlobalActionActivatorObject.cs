using UnityEngine;

[CreateAssetMenu(menuName = "Events/Activator/OnGlobalAction")]
public class OnGlobalActionActivatorObject : EventActivatorObject
{
    [SerializeField] private GlobalActionObject _actionObject;
    public string actionName => _actionObject.Name;

    public override EventActivator GetActivator()
	{
		return new OnGlobalActionActivator(this);
	}
}
