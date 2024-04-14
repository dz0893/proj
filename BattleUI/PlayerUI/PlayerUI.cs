using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	private BattleMap battleMap;
	
	[SerializeField] private TurnStateUI _turnStateUI;
	[SerializeField] private UnitListUI _unitListUI;
	[SerializeField] private ResultUI _resultUI;
	[SerializeField] private Text _turnCounter;
	[SerializeField] private Text _gold;
	[SerializeField] private Text _ore;
	[SerializeField] private Text _unitLimit;
	[SerializeField] private Text _playerIndex;
	[SerializeField] private Image _heroIcon;
	[SerializeField] private Image _heroIconBack;
	[SerializeField] private Image _playerIndexBack;
	[SerializeField] private Image _blackScreen;

	[SerializeField] private Image _onButtonHeroIconBack;
	
	[SerializeField] private Button _skipTurnButton;
	[SerializeField] private Button _journalButton;
	[SerializeField] private Button _capitalButton;
	[SerializeField] private Button _heroButton;
	
	[SerializeField] private GlobalActionUI _globalActionUI;
	
	public TurnController turnController => battleMap.turnController;
	private Player currentPlayer => TurnController.currentPlayer;
	
	public static bool inputIsLocked { get; private set; }
	
	public delegate void RefreshPlayerInfo(Player player);
	public delegate void InitMatch();
	public delegate void LockInput();
	public delegate void UnlockInput();
	public delegate void OpenGameResult(bool isWin);
	public delegate void RenderButtons(bool state);
	public delegate void OnScreen();
	public delegate void OffScreen();
	
	public static RefreshPlayerInfo refreshPlayerInfo;
	public static InitMatch initMatch;
	public static LockInput lockInput;
	public static UnlockInput unlockInput;
	public static OpenGameResult openGameResult;
	public static OnScreen onScreen;
	public static OffScreen offScreen;
	
	private void Start()
	{
		refreshPlayerInfo = RenderPlayerInfo;
		initMatch = Init;
		lockInput = LockPlayerInput;
		unlockInput = UnlockPlayerInput;
		openGameResult = OpenResult;
		onScreen = OnBlackScreen;
		offScreen = OffBlackScreen;
	}
	
	private void Init()
	{
		battleMap = BattleMap.instance;
		InitTurn(TurnController.turnCounter);
	}
	
	private void LockPlayerInput()
	{
		MapController.controllerIsBlocked = true;
		inputIsLocked = true;
		SetButtonsState(false);
	}
	
	private void UnlockPlayerInput()
	{
		MapController.controllerIsBlocked = false;
		inputIsLocked = false;
		SetButtonsState(true);
	}
	
	public void InitTurn(int turn)
	{
		_turnCounter.text = turn.ToString();
		SetButtonsState(!TurnController.currentPlayerNotLocal);
		
		RenderPlayerInfo(TurnController.lastNotComputerPlayer);

		_turnStateUI.Render();
	}

	private void RenderPlayerInfo(Player player)
	{
		if (TurnController.lastNotComputerPlayer == null)
			return;

		if (player.hero != null)
		{
			_heroIcon.sprite = player.hero.icon;
			_onButtonHeroIconBack.sprite = player.hero.icon;
		}

		_heroIconBack.color = new Vector4(player.color.r, player.color.g, player.color.b, 200/255f);
		_playerIndexBack.color = new Vector4(player.color.r, player.color.g, player.color.b, 200/255f);

		_unitListUI.Render(player);
		_playerIndex.text = player.nickname;
		_gold.text = player.gold + " (+" + player.goldIncome + ")";
		_ore.text = player.ore + " (+" + player.oreIncome + ")";
		_unitLimit.text = player.currentUnitLimit + " / " + player.maxUnitLimit;
		
		_globalActionUI.CleanSelect();
		_globalActionUI.Render(player);
		
		MiniMap.render.Invoke();

		SetHeroButtonState();

	//	BattleMap.instance.RenderObjectsState();
	}
	
	public void OpenResult(bool isWin)
	{
		_resultUI.gameObject.SetActive(true);
		LockPlayerInput();
		_resultUI.Render(isWin);
	}
	
	public void SetButtonsState(bool state)
	{
		_skipTurnButton.interactable = state;
		_journalButton.interactable = state;
		_capitalButton.interactable = state;
		_heroButton.interactable = state;
		
		if (inputIsLocked)
		{
			_skipTurnButton.interactable = false;
			_journalButton.interactable = false;
		}
		
		if (currentPlayer != null && currentPlayer.missionList.Count == 0 && currentPlayer.journalSheetList.Count == 0)
			_journalButton.interactable = false;
		
		if (TurnController.lastNotComputerPlayer != null && TurnController.lastNotComputerPlayer.capital != null)
			_capitalButton.interactable = true;
		else
			_capitalButton.interactable = false;

		//	SetHeroButtonState();
	}

	private void SetHeroButtonState()
	{
		if (TurnController.lastNotComputerPlayer != null && TurnController.lastNotComputerPlayer.hero != null && !TurnController.lastNotComputerPlayer.hero.isDead)
			_heroButton.interactable = true;
		else
			_heroButton.interactable = false;
	}
	
	public void SelectCapital()
	{
		MapController.select.Invoke(TurnController.lastNotComputerPlayer.capital.position);
		CameraController.setCameraPosition(TurnController.lastNotComputerPlayer.capital.transform.position);
	}

	public void SelectHero()
	{
		MapController.select.Invoke(TurnController.lastNotComputerPlayer.hero.position);
		CameraController.setCameraPosition(TurnController.lastNotComputerPlayer.hero.transform.position);
	}

	private void OnBlackScreen()
	{
		_blackScreen.gameObject.SetActive(true);
	}

	private void OffBlackScreen()
	{
		_blackScreen.gameObject.SetActive(false);
	}
}