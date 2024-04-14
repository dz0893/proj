using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventActionObject : GlobalActionObject
{
    [SerializeField] private int _playerIndex;
    public int playerIndex => _playerIndex;

    public override GlobalAction GetGlobalAction()
	{
		return new GlobalEventAction(this);
	}
}
