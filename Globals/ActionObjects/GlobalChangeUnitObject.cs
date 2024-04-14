using System.Collections.Generic;
using UnityEngine;

public class GlobalChangeUnitObject : GlobalActionObject
{
	[SerializeField] private List<Unit> _targetUnitList;
	public List<Unit> targetUnitList => _targetUnitList;
	
	[SerializeField] private List<Unit> _changetUnitList;
	public List<Unit> changetUnitList => _changetUnitList;
	
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalChangeUnit(this);
	}
}
