using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialObjectUI : ObjectUI
{
	[SerializeField] private CapitalUI _capitalUI;
	[SerializeField] private UnitUI _unitUI;
	
	private void Start()
	{
		_capitalUI.Init();
	}

	protected override void Render(NullObject obj)
	{
		_portraitUI.Render(obj);
		
		_capitalUI.buildingListUI.gameObject.SetActive(false);
		_capitalUI.upgradeListUI.gameObject.SetActive(false);
		
		if (obj is Structure)
		{
			_capitalUI.gameObject.SetActive(true);
			_unitUI.gameObject.SetActive(false);
			
			_capitalUI.Render(obj as Structure);
		}
		else if (obj is Unit)
		{
			_capitalUI.gameObject.SetActive(false);
			_unitUI.gameObject.SetActive(true);
			
			_unitUI.Render(obj as Unit);
		}
	}
}
