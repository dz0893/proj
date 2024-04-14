using UnityEngine;
using UnityEngine.UI;

public class TurnStateUI : MonoBehaviour
{
    [SerializeField] private PlayerUI _playerUI;

    [SerializeField] private Button _skipTurnButton;
    [SerializeField] private Text _turnStateText;
    [SerializeField] private Text _turnCounterText;

    [SerializeField] private GameObject _otherPlayerTurnedEffectObject;

    public delegate void SkipTurn();
	public delegate void RpcSkipTurn();

    public static SkipTurn skipTurn;
	public static RpcSkipTurn rpcSkipTurn;

    private void Start()
    {
        skipTurn = EndTurn;
		rpcSkipTurn = OfflineEndTurn;
    }

    public void Render()
    {
        _turnCounterText.text = TurnController.turnCounter.ToString();

        if (TurnController.currentPlayerNotLocal)
        {
            _turnStateText.text = UISettings.WaitingPlayer + TurnController.currentPlayer.nickname;
            _otherPlayerTurnedEffectObject.SetActive(true);
        }
        else
        {
            _turnStateText.text = UISettings.YourTurn;
            _otherPlayerTurnedEffectObject.SetActive(false);
        }
    }

    public void EndTurn()
	{
        if (!MapController.controllerIsBlocked)
        {
            _skipTurnButton.interactable = false;
            
            if (CustomNetworkManager.IsOnlineGame)
                LobbyController.localPlayerController.commandLouncher.SkipTurn();
            else
                OfflineEndTurn();
        }
        else
        {
            Debug.Log("SOMETHING WRONG!!!1 Controller is blocked");
        }
	}

	private void OfflineEndTurn()
	{
		if (!MapController.controllerIsBlocked && !PlayerUI.inputIsLocked)
		{
            if (!TurnController.currentPlayerNotLocal)
		    {
			    DataSaver.save.Invoke(UISettings.AutoSave);
		    }

			_playerUI.turnController.EndTurn();
			_playerUI.InitTurn(TurnController.turnCounter);
			MapController.clear.Invoke();
		}
	}
}
