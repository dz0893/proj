using UnityEngine;

public class GlobalSetEffectObject : GlobalActionObject
{
	[SerializeField] private ActionEffect _actionEffect;
	public ActionEffect actionEffect => _actionEffect;
	
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalSetEffect(this);
	}
}
