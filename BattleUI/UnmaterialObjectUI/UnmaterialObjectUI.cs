using UnityEngine;
using UnityEngine.UI;

public class UnmaterialObjectUI : ObjectUI
{
	[SerializeField] protected Text _infoField;
	
	protected override void Render(NullObject obj)
	{
		_portraitUI.Render(obj);
		_infoField.text = "its a trap!";
	}
}
