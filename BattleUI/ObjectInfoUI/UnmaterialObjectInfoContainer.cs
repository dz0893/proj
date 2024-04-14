using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnmaterialObjectInfoContainer : AbstractInfoContainer
{
	[SerializeField] private Text _description;
	
	public override void Render(ObjectInfo info)
	{
		UnmaterialObjectInfo unmaterialObjectInfo = info as UnmaterialObjectInfo;
		
		_description.text = unmaterialObjectInfo.description;
	}
}
