using UnityEngine;

public class GlobalSacrificeObject : GlobalActionObject
{
	[SerializeField] private bool _isMaxHealthConverted;
	public bool isMaxHealthConverted => _isMaxHealthConverted;
	
	[SerializeField] private float _effectivnese;
	public float effectivnese => _effectivnese;
	
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalSacrifice(this);
	}
}
