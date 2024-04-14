using UnityEngine;
using UnityEngine.UI;

public class ActionInfoContainer : AbstractInfoContainer
{
	[SerializeField] private Text _textPref;
	[SerializeField] private Transform _container;
	
	public override void Render(ObjectInfo info)
	{
		ActionInfo actionInfo = info as ActionInfo;
		
		Clean();
		InitDescription(actionInfo);
	}
	
	public void Clean()
	{
		foreach (Transform child in _container)
			Destroy(child.gameObject);
	}
	
	private void InitDescription(ActionInfo actionInfo)
	{
		foreach (string descriptionString in actionInfo.descriptionList)
			InitString(descriptionString);
	}
	
	private void InitString(string descriptionString)
	{
		Text descriptionField = Instantiate(_textPref, _container);
		
		descriptionField.text = descriptionString;
	}
}
