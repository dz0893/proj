using UnityEngine;
using UnityEngine.UI;

public class RequiredBuildingCell : MonoBehaviour
{
	[SerializeField] private Text _bildingNameField;
	[SerializeField] private Text _bildingLevelField;
	
	public void Render(UnitDataInfo info, int index)
	{
		_bildingNameField.text = info.requiredBuildingList[index].Name;
		_bildingLevelField.text = UISettings.Lvl + info.buildingLevelList[index];
		
		if (info.buildingStateList[index])
		{
			_bildingNameField.color = Color.green;
			_bildingLevelField.color = Color.green;
		}
		else
		{
			_bildingNameField.color = UISettings.defaultTextColor;
			_bildingLevelField.color = UISettings.defaultTextColor;
		}
	}
}
