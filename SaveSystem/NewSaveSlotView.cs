using UnityEngine;
using UnityEngine.UI;

public class NewSaveSlotView : MonoBehaviour
{
	[SerializeField] private Text _inputField;
	private string Name => _inputField.text;
	
	private void OnEnable()
	{
		_inputField.text = "";
	}
	
	public void PushSaveButton()
	{
		if (!Name.Equals(""))
			Save();
	}
	
	private void Save()
	{
		DataSaver.save.Invoke(Name);
	}
}
