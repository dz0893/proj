using UnityEngine;

public class GlobalAddExpObject : GlobalActionObject
{
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalAddExp(this);
	}
}
