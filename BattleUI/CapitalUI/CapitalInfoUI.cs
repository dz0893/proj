using UnityEngine;
using UnityEngine.UI;

public class CapitalInfoUI : MonoBehaviour
{
	[SerializeField] private Text _nameField;
	[SerializeField] private Text _healthField;
//	[SerializeField] private GameObject _unitLevelObject;
//	[SerializeField] private GameObject _unitExperienceObject;
	
	public void Render(Structure structure)
	{
		_nameField.text = structure.Name;
		_healthField.text = UISettings.health + structure.currentHealth + " / " + structure.currentStats.maxHealth;

	//	_unitLevelObject.SetActive(false);
	//	_unitExperienceObject.SetActive(false);
	}
}
