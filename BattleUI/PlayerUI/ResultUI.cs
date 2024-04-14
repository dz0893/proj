using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
	[SerializeField] Text _result;
	private bool isWin;
	
	public void Render(bool isWin)
	{
		this.isWin = isWin;

		if (isWin)
			_result.text = UISettings.Win;
		else
			_result.text = UISettings.Lose; 
	}
	
	public void ContinueButton()
	{
		bool isCampainGame = BattleMap.instance.GetComponent<Scenario>() != null;

		PlayerUI.unlockInput.Invoke();
		MainMenu.toMenu.Invoke();

		gameObject.SetActive(false);

		if (isCampainGame && isWin)
			CampainLobby.openEpilogueScreen.Invoke();
	}
}
