using System.Collections.Generic;
using UnityEngine;

public class Hypnose : ActionEffect
{
	public override EffectType effectType => EffectType.Hypnose;
	
	public override void LocalClean(Unit target, CurrentEffect effect)
	{
		target.player.objectList.Remove(target);
		
		target.SetPlayer(effect.targetPlayer);
		target.SetColor();
	}
	
	public override void LocalActivate(Unit target, CurrentEffect effect)
	{
		target.player.objectList.Remove(target);
		
		target.SetPlayer(effect.casterPlayer);
		target.SetColor();
		target.StartTurn();
		
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
	}
	
	public override List<string> GetDescription()
	{
		List<string> description = new List<string>();
		
		if (liveTime != 0)
			description.Add(UISettings.LiveTime + liveTime + UISettings.turns);
		
		return description;
	}
}
