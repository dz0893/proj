using System.Collections.Generic;
using UnityEngine;

public abstract class OnCellEffectData : MonoBehaviour
{
	[SerializeField] private string _name;
	[SerializeField] private bool _allUnitsEffected;
	[SerializeField] private bool _aliesEffected;
	[SerializeField] private bool _enemiesEffected;
	[SerializeField] private bool  _refreshedOnStartTurnOnly;
	public string Name => _name;
	public bool allUnitsEffected => _allUnitsEffected;
	public bool aliesEffected => _aliesEffected;
	public bool enemiesEffected => _enemiesEffected;
	public bool refreshedOnStartTurnOnly => _refreshedOnStartTurnOnly;

	[SerializeField] private EffectType _effectType;
	public EffectType effectType => _effectType;
	
	public void Set(GroundCell cell)
	{
		OnCellEffect effect = new OnCellEffect(this, cell);
		
		cell.onCellEffectList.Add(effect);
		
		effect.SetEffectOnObject();
	}
	
	public void Set(GroundCell cell, int team)
	{
		OnCellEffect effect = new OnCellEffect(this, cell, team);
		
		cell.onCellEffectList.Add(effect);
		
		effect.SetEffectOnObject();
	}
	
	public virtual void SetEffectOnObject(OnCellEffect effect, Unit unit) {}
	
	public virtual void CleanObject(Unit unit) {}
	
	public virtual List<string> GetEffectDescription() { return new List<string>(); }
}
