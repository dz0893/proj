using System.Collections.Generic;
using UnityEngine;

public abstract class ActionEffect : MonoBehaviour
{
	[SerializeField] private int _index;
	public int index => _index;
	
	[SerializeField] private string _name;
	public virtual string Name => _name;
	
	[SerializeField] private int _liveTime;
	public int liveTime => _liveTime;
	
	[SerializeField] private bool _isStackable;
	[SerializeField] private bool _isNegative;
	public bool isNegative => _isNegative;
	
	[SerializeField] private bool _dotAtEndOfTurn;
	public bool dotAtEndOfTurn => _dotAtEndOfTurn;
	
	[SerializeField] private bool _workAtMechs;
	[SerializeField] private bool _workAtLives = true;

	[SerializeField] private ActionEffect _exceptionEffect;
	
	public virtual EffectType effectType => EffectType.None;

	public void Activate(Unit target)
	{
		if (target == null || !_workAtMechs && target.isMech || !_workAtLives && !target.isMech)
			return;
		
		CurrentEffect currentEffect = new CurrentEffect(TurnController.currentPlayer, target, this);
		
		if (_isStackable || !ChekForActiveEffect(target))
		{
			LocalActivate(target, currentEffect);
			
			if (_liveTime != 0)
				target.activeEffectList.Add(currentEffect);
		}
		else
		{
			foreach (CurrentEffect effect in target.activeEffectList)
			{
				if (effect.actionEffect == this)
				{
					effect.RefreshCounter();
					break;
				}
			}
		}

		foreach (CurrentEffect effect in target.activeEffectList)
		{
			if (effect.actionEffect == _exceptionEffect)
			{
				effect.Clean();
				break;
			}
		}

		IconSetter.setEffects.Invoke(target);
		
		if (!Name.Equals(null))
			AnimationController.write(target.transform.position, Name, Color.white);
	}
	
	public void ActivateOnLoad(Unit target, ActionEffectSaveInfo saveInfo)
	{
		CurrentEffect currentEffect = new CurrentEffect(saveInfo, target, this);
		
		LocalActivate(target, currentEffect);
		
		if (_liveTime != 0)
			target.activeEffectList.Add(currentEffect);
		
		IconSetter.setEffects.Invoke(target);
	}
	
	private bool ChekForActiveEffect(Unit target)
	{
		foreach (CurrentEffect effect in target.activeEffectList)
		{
			if (effect.actionEffect == this)
				return true;
		}
		
		return false;
	}

	public bool IsWrongTarget(MaterialObject obj)
	{
		if (obj == null || obj is Structure || obj.isMech && !_workAtMechs || !obj.isMech && !_workAtLives)
			return true;
		else
			return false;
	}
	
	public void Clean(Unit target, CurrentEffect effect)
	{
		IconSetter.setEffects.Invoke(target);
		LocalClean(target, effect);
	}
	
	public virtual void LocalClean(Unit target, CurrentEffect effect) {}

	public virtual void LocalActivate(Unit target, CurrentEffect effect) {}
	
	public virtual void Dot(Unit target, CurrentEffect effect) {}
	
	public virtual List<string> GetDescription() { return new List<string>(); }
}
