using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraData : MonoBehaviour
{
	[SerializeField] private Sprite _icon;
	public Sprite icon => _icon;
	
	[SerializeField] private string _name;
	public string Name => _name;
	
	[SerializeField] private string _description;
	public string description => _description;
	
	[SerializeField] private OnCellEffectData _effectData;
	public OnCellEffectData effectData => _effectData;
	
	[SerializeField] private int _range;
	public int range => _range;
	
	public void ActivateAura(Unit unit, GroundCell position)
	{
		Aura aura = new Aura(this, unit);
		
		aura.SetArea(position);
	}
}
