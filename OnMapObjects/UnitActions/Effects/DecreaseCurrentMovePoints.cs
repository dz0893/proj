using System.Collections.Generic;
using UnityEngine;

public class DecreaseCurrentMovePoints : ActionEffect
{
	[SerializeField] private int _effectValue;
	[SerializeField] private bool _fullMovePoints;
	
	public override EffectType effectType => EffectType.DecreaseStats;

	public override void LocalActivate(Unit target, CurrentEffect effect)
	{
		target.currentMovePoints -= _effectValue;
		
		if (target.currentMovePoints < 0 || _fullMovePoints)
			target.currentMovePoints = 0;
	}
	
	public override void Dot(Unit target, CurrentEffect effect)
	{
		LocalActivate(target, effect);
	}
	
	public override List<string> GetDescription()
	{
		List<string> description = new List<string>();
		
		if (_fullMovePoints)
			description.Add(UISettings.effectValue + UISettings.fullMovePoints);
		else
			description.Add(UISettings.effectValue + _effectValue);
		
		if (liveTime != 0)
			description.Add(UISettings.LiveTime + liveTime + UISettings.turns);
		
		return description;
	}
}
