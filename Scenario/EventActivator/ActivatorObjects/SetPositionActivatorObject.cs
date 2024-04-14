using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Activator/SetPosition")]
public class SetPositionActivatorObject : EventActivatorObject
{
	[SerializeField] private int _positionIndex;
	public int positionIndex => _positionIndex;
	
	[SerializeField] private List<int> _playerIndex;
	public List<int> playerIndex => _playerIndex;
	
	public override EventActivator GetActivator()
	{
		return new SetPositionActivator(this);
	}
}
