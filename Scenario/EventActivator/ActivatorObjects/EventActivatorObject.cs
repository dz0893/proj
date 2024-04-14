using UnityEngine;

public abstract class EventActivatorObject : ScriptableObject
{
	[SerializeField] private MapEvent _mapEvent;
	public MapEvent mapEvent => _mapEvent;
	
	[SerializeField] private int _activatorIndex;
	public int activatorIndex => _activatorIndex;

	public virtual bool multipleTimeActivating => false;
	
	public abstract EventActivator GetActivator();
}
