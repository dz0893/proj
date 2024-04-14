using UnityEngine;

public class GlobalHealingObject : GlobalActionObject
{
	[SerializeField] private bool _onMechs;
	public bool onMechs => _onMechs;
	
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalHealing(this);
	}
}
