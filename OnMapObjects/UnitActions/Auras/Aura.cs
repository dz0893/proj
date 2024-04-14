using System.Collections.Generic;
using UnityEngine;

public class Aura : IActionDescription, IAreaAction
{
	private AttackDistanceFinder attackDistanceFinder = new AttackDistanceFinder();
	
	public string Name => data.Name;
	public Sprite icon => data.icon;
	public OnCellEffectData effectData => data.effectData;
	
	public int area => data.range;
	public List<GroundCell> areaList { get; private set; } = new List<GroundCell>();
	
	public AuraData data { get; private set; }
	public Unit unit { get; private set; }
	
	public bool isAreaAction => true;

	private List<string> _descriptionList;
	public List<string> descriptionList => _descriptionList;
	
	public Aura(AuraData data, Unit unit)
	{
		this.data = data;
		this.unit = unit;
		unit.aura = this;
		SetDescription();
	}
	
	public void SetDescription()
	{
		_descriptionList = new List<string>();
		
		if (data.description != "")
			_descriptionList.Add(data.description);
		
		_descriptionList.Add(UISettings.range + area);
		
		List<string> effectDescription = effectData.GetEffectDescription();
		
		foreach (string descriptionString in effectDescription)
			_descriptionList.Add(descriptionString);
	}
	
	public void SetArea(GroundCell position)
	{
		areaList = GetAreaDistance(position);
		
		if (!unit.isDead)
		{
			foreach (GroundCell cell in areaList)
				data.effectData.Set(cell, unit.team);
		}
	}
	
	public List<GroundCell> GetAreaDistance(GroundCell position)
	{
		return attackDistanceFinder.GetRangedAttackDistance(position, data.range);
	}
	
	public void CleanArea()
	{
		foreach (GroundCell cell in areaList)
			cell.ClearCurrentEffect(data.effectData, unit.team);
	}
	
	public void ResetArea(GroundCell position)
	{
		CleanArea();
		SetArea(position);
	}
	
	public virtual string GetRenderedText(GroundCell cell) { return ""; }
}
