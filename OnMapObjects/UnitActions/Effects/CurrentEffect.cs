using UnityEngine;

public class CurrentEffect
{
	public string Name => actionEffect.Name;
	public int index => actionEffect.index;
	
	public Player casterPlayer { get; private set; }
	public Player targetPlayer { get; private set; }
	
	public Unit target { get; private set; }
	
	public ActionEffect actionEffect { get; private set; }
	public EffectType effectType => actionEffect.effectType;

	public int counter { get; private set; }
	
	public bool isCleaned { get; private set; }
	public bool isNegative { get; private set; }
	
	public void RefreshCounter()
	{
		counter = actionEffect.liveTime;
	}
	
	public void TurnEffect()
	{
		actionEffect.Dot(target, this);
		
		counter--;
		
		if (counter <= 0)
			Clean();
	}
	
	public void Clean()
	{
		isCleaned = true;
		
		target.activeEffectList.Remove(this);
		
		if (actionEffect is LiveTime && counter != 0)
			return;
		
		actionEffect.Clean(target, this);
	}
	
	public CurrentEffect(Player player, Unit target, ActionEffect actionEffect)
	{
		casterPlayer = player;
		targetPlayer = target.player;
		this.target = target;
		this.actionEffect = actionEffect;
		counter = actionEffect.liveTime;
		isNegative = actionEffect.isNegative;
	}
	
	public CurrentEffect(ActionEffectSaveInfo saveInfo, Unit target, ActionEffect actionEffect)
	{
		casterPlayer = BattleMap.instance.turnController.playerList[saveInfo.casterPlayerIndex];
		targetPlayer = BattleMap.instance.turnController.playerList[saveInfo.targetPlayerIndex];
		this.target = target;
		this.actionEffect = actionEffect;
		counter = saveInfo.counter;
		isNegative = actionEffect.isNegative;
	}
}
