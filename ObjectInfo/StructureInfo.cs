using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureInfo : ObjectInfo
{
	private Structure structure;
	
	public string description => structure.description;
	
	public int currentHealth => structure.currentHealth;
	public int maxHealth => structure.currentStats.maxHealth;
	
	public int physicalDefence => structure.currentStats.physicalDefence;
	public int piercingDefence => structure.currentStats.piercingDefence;
	public int magicDefence => structure.currentStats.magicDefence;
	public int siegeDefence => structure.currentStats.siegeDefence;
	
	public int currentOreValue
	{
		get
		{
			if (structure is Mine)
			{
				return structure.position.currentOreValue;
			}
			else
			{
				return 0;
			}
		}
	}

	public override void Init(object obj)
	{
		structure = obj as Structure;
		
		objectName = structure.Name;
	}
}
