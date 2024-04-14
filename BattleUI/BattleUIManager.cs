using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
	[SerializeField] private MaterialObjectUI _materialObjectUI;
	[SerializeField] private RecruitPointUI _recruitPointUI;
	
	public delegate void OnUI(NullObject obj);
	public delegate void OffUI();
	
	public static OnUI onUI;
	public static OffUI offUI;
	
	private void Start()
	{
		onUI = OpenObjectUI;
		offUI = CloseAllUI;
		_recruitPointUI.Init();
	}
	
	public void OpenObjectUI(NullObject obj)
	{	
		CloseAllUI();
		
		if (obj is MaterialObject)
			_materialObjectUI.OnObjectUI(obj);
		
		else if (obj is RecruitPoint)
			_recruitPointUI.OnObjectUI(obj);
	}
	
	private void CloseAllUI()
	{
		_materialObjectUI.OffObjectUI();
		_recruitPointUI.OffObjectUI();
	}
}
