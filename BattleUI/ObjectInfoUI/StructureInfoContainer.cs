using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureInfoContainer : AbstractInfoContainer
{
	[SerializeField] private Text _health;
	
	[SerializeField] private Text _physicalDefence;
	[SerializeField] private Text _piercingDefence;
	[SerializeField] private Text _magicDefence;
	[SerializeField] private Text _sturmDefence;
	[SerializeField] private Text _oreValueField;
	[SerializeField] private Text _description;
	
	public override void Render(ObjectInfo info)
	{
		StructureInfo structureInfo = info as StructureInfo;
		
		RenderStats(structureInfo);
		RenderMine(structureInfo);
	}
	
	private void RenderStats(StructureInfo structureInfo)
	{
		_health.text = UISettings.health + structureInfo.currentHealth + " / " + structureInfo.maxHealth;
		
		_physicalDefence.text = UISettings.Physical + structureInfo.physicalDefence;
		_piercingDefence.text = UISettings.Piercing + structureInfo.piercingDefence;
		_magicDefence.text = UISettings.Magical + structureInfo.magicDefence;
		_sturmDefence.text = UISettings.Siege + structureInfo.siegeDefence;
		
		_description.text = structureInfo.description;
	}

	private void RenderMine(StructureInfo structureInfo)
	{
		if (structureInfo.currentOreValue == 0)
		{
			_oreValueField.gameObject.SetActive(false);
		}
		else
		{
			_oreValueField.gameObject.SetActive(true);
			_oreValueField.text =  UISettings.OreValue + structureInfo.currentOreValue;
		}
	}
}
