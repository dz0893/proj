using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartedPosition : MonoBehaviour
{
	[SerializeField] private GroundCell _position;
	public GroundCell position => _position;
	
	[SerializeField] private int _playerIndex;
	[SerializeField] private Transform _container;
	
	public int playerIndex => _playerIndex;
	
	public NullObject obj { get; private set; }
	
	public void Init(NullObject obj)
	{
		NullObject initedObject = Instantiate(obj, _container);
		initedObject.Init(_position, BattleMap.instance.playerList[_playerIndex]);
		
		this.obj = initedObject;
	}
}
