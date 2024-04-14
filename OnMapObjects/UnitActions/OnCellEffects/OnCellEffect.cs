using UnityEngine;

public class OnCellEffect
{
	public OnCellEffectData data { get; private set; }
	public EffectType effectType => data.effectType;

	public string Name => data.Name;
	
	public int counter { get; private set; }
	
	public GroundCell cell { get; private set; }
	
	public int team { get; private set; }
	
	public bool refreshedOnStartTurnOnly { get; private set; }
	public bool allUnitsEffected { get; private set; }
	public bool aliesEffected { get; private set; }
	public bool enemiesEffected { get; private set; }
	
	public OnCellEffect(OnCellEffectData data, GroundCell cell)
	{
		this.data = data;
		this.cell = cell;
		
		refreshedOnStartTurnOnly = data.refreshedOnStartTurnOnly;
		
		allUnitsEffected = true;
	}
	
	public OnCellEffect(OnCellEffectData data, GroundCell cell, int team)
	{
		this.data = data;
		this.cell = cell;
		
		this.team = team;
		
		allUnitsEffected = data.allUnitsEffected;
		aliesEffected = data.aliesEffected;
		enemiesEffected = data.enemiesEffected;
		refreshedOnStartTurnOnly = data.refreshedOnStartTurnOnly;
	}
	
	public void CleanObject(Unit unit)
	{
		data.CleanObject(unit);
		unit.onCellEffectList.Remove(this);
	}
	
	public void SetEffectOnObject()
	{
		if (cell.onCellObject != null && cell.onCellObject is Unit)
		{
			Unit unit = cell.onCellObject as Unit;
			
			foreach (OnCellEffect effect in unit.onCellEffectList)
			{
				if (effect.data == data)
					return;
			}
			
			if (allUnitsEffected)
				data.SetEffectOnObject(this, unit);
		
			else if (aliesEffected && team == cell.onCellObject.team)
				data.SetEffectOnObject(this, unit);
		
			else if (enemiesEffected && team != cell.onCellObject.team)
				data.SetEffectOnObject(this, unit);
		}
	}
	
	public void Clean()
	{
		cell.onCellEffectList.Remove(this);
		
		if (cell.onCellObject != null && cell.onCellObject is Unit)
			CleanObject(cell.onCellObject as Unit);
	}
}
