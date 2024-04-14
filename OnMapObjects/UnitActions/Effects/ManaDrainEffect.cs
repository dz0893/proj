using System.Collections.Generic;
using UnityEngine;

public class ManaDrainEffect : ActionEffect
{
    [SerializeField] private int _value;
	public int value => _value;

    public override void LocalActivate(Unit target, CurrentEffect effect)
	{
	    target.WasteMana(value);
	}

    public override List<string> GetDescription()
	{
		List<string> description = new List<string>();
		
		description.Add(UISettings.effectValue + value + UISettings.mana + UISettings.perHit);
		
		return description;
	}
}
