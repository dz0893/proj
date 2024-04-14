using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject _background;
	public GameObject background => _background;

	[SerializeField] private GameObject _offlineMenu;
	[SerializeField] private GameObject _onlineMenu;
	[SerializeField] private GameObject _lobbieMenu;
	[SerializeField] private GameObject _onlineLobby;
	
	[SerializeField] private GameOptions _gameOptions;
	
	[SerializeField] private GameLobby _skirmishMenu;
	[SerializeField] private CampainLobby _campainMenu;
	
	public delegate void ToMenu();
	public static ToMenu toMenu;
	
	public delegate void Close();
	public static Close close;

	public delegate void Reload();
	public static Reload reload;

	public delegate void OpenSteamLobby();
	public static OpenSteamLobby openSteamLobby;
	
	private void Start()
	{
		toMenu = ReturnToMainMenue;
		close = CloseLobby;
		reload = ReloadScene;
		openSteamLobby = OpenOnlineLobby;
		AudioManager.setMainMenuMusic.Invoke();
		_gameOptions.Init();
	}
	
	public void OpenSkirmishMenu()
	{
		_background.SetActive(false);
		_skirmishMenu.Open();
	}
	
	public void OpenCampainMenu()
	{
		_background.SetActive(false);
		_campainMenu.Open();
	}

	private void OpenOnlineLobby()
	{
		_background.SetActive(false);
		_onlineLobby.SetActive(true);
	}

	public void CreateLobby()
	{
		SteamLobby.instance.CreateLobby();
	}
	
	public void GoToOffline()
	{
		_onlineMenu.SetActive(false);
		_offlineMenu.SetActive(true);
		_lobbieMenu.SetActive(false);
	}
	
	public void GoToOnline()
	{
		_onlineMenu.SetActive(true);
		_offlineMenu.SetActive(false);
		_lobbieMenu.SetActive(false);
	}
	
	public void GoToLobbyMenu()
	{
		_onlineMenu.SetActive(false);
		_offlineMenu.SetActive(false);
		_lobbieMenu.SetActive(true);
		
		SteamLobby.instance.GetLobbyList();
	}
	
	private void CloseLobby()
	{
		_background.SetActive(false);
		_onlineMenu.SetActive(false);
		_offlineMenu.SetActive(false);
		_lobbieMenu.SetActive(false);
		DataSaver.close.Invoke();
	}
	
	public void ReturnToMainMenue()
	{
		Debug.Log("ReturnToMainMenue");
		if (BattleMap.instance != null)
			BattleMap.instance.EndGame();
		
		_background.SetActive(true);
		_offlineMenu.SetActive(true);

		_skirmishMenu.Close();
		_campainMenu.Close();
		_onlineMenu.SetActive(false);
		_lobbieMenu.SetActive(false);
		DataSaver.close.Invoke();
	}
	
	private void ReloadScene()
	{
		PlayerUI.unlockInput.Invoke();
		SceneManager.LoadScene("MainMenu");
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void StartHost()
	{
		OpenOnlineLobby();
		CustomNetworkManager.singleton.StartHost();
	}

	public void StartClient()
	{
		OpenOnlineLobby();
		CustomNetworkManager.singleton.StartClient();
	}
}
