using UnityEngine;

public class GlobalRiseDeadObject : GlobalActionObject
{
	[SerializeField] private Unit _warriorUnit;
	public Unit warriorUnit => _warriorUnit;
	
	[SerializeField] private Unit _archerUnit;
	public Unit archerUnit => _archerUnit;
	
	[SerializeField] private Unit _mageUnit;
	public Unit mageUnit => _mageUnit;
	
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalRiseDead(this);
	}
}
