using UnityEngine;
using UnityEngine.UI;

public class MatchMenue : MonoBehaviour
{
	[SerializeField] private Button _saveButton;
	[SerializeField] private Button _loadButton;

	[SerializeField] private DataSaver _dataSaver;
	
	[SerializeField] private Transform _container;	
	
	public delegate void SwitchMenuState();
	public delegate void Close();
	
	public static SwitchMenuState switchMenuState;
	public static Close close;
	
	private void Start()
	{
		switchMenuState = SwitchContainerState;
		close = CloseMenu;
	}
	
	private void CloseMenu()
	{
		_container.gameObject.SetActive(false);
	}
	
	public void SwitchContainerState()
	{
		if (MapController.controllerIsBlocked || TurnController.currentPlayerIsAi)
			return;
			
		BattleUIManager.offUI.Invoke();
		_container.gameObject.SetActive(!_container.gameObject.activeSelf);
		
		if (!_container.gameObject.activeSelf)
			DataSaver.close.Invoke();
		else
			RenderButtonState();
	}

	private void RenderButtonState()
	{
		_saveButton.interactable = !CustomNetworkManager.IsOnlineGame;
		_loadButton.interactable = !CustomNetworkManager.IsOnlineGame;
	}
	
	public void ToMainMenue()
	{
		BattleMap.instance.EndGame();
		MainMenu.reload.Invoke();
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
