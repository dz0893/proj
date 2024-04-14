using UnityEngine;

public class GlobalChangeManaObject : GlobalActionObject
{
	[SerializeField] private bool _isRestoring;
	public bool isRestoring => _isRestoring;
	
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalChangeMana(this);
	}
}
