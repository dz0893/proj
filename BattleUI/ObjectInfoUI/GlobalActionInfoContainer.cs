using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalActionInfoContainer : AbstractInfoContainer
{
	[SerializeField] private Text _descriptionStringPref;
	[SerializeField] private Transform _container;
	
	[SerializeField] private Text _activatingState;
	
	public override void Render(ObjectInfo info)
	{
		GlobalActionInfo actionInfo = info as GlobalActionInfo;
		
		RenderText(actionInfo);
		RenderActivatingStateText(actionInfo);
	}
	
	private void RenderText(GlobalActionInfo actionInfo)
	{
		Clean();
		InitDescription(actionInfo.descriptionList);
	}
	
	private void Clean()
	{
		foreach (Transform child in _container)
			Destroy(child.gameObject);
	}
	
	private void InitDescription(List<string> descriptionList)
	{
		foreach(string descriptionString in descriptionList)
		{
			Text descriptionField = Instantiate(_descriptionStringPref, _container);
			descriptionField.text = descriptionString;
		}
	}
	
	private void RenderActivatingStateText(GlobalActionInfo actionInfo)
	{
		_activatingState.color = Color.red;
		
		if (!actionInfo.currentActivatingWarningText.Equals(""))
		{
			_activatingState.text = actionInfo.currentActivatingWarningText;
		}
		else if (actionInfo.usedAtThisTurn)
		{
			_activatingState.text = UISettings.alreadyUsed;
		}
		else if (!actionInfo.requiredStructureIsBuilded)
		{
			_activatingState.text = UISettings.mustBuild + actionInfo.action.requiredBuilding.Name
			+ " " + UISettings.Lvl + actionInfo.action.buildingLevel;
		}
		else if (!actionInfo.heroIsAlive)
			_activatingState.text = UISettings.heroMustBeAlive;
		
		else if (!actionInfo.enoughtResources)
			_activatingState.text = UISettings.notEnoughtResources;
			
		else
		{
			_activatingState.color = Color.green;
			_activatingState.text = UISettings.canBeActivated;
		}
	}
}
