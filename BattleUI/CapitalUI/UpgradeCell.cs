using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _icon;
	[SerializeField] private Text _nameField;
	[SerializeField] private Text _goldCostField;
    [SerializeField] private Text _oreCostField;
	[SerializeField] private Text _statusField;
	[SerializeField] private Button _researchButton;

    private Upgrade upgrade;
    private Capital capital;
    private Player player;

    private UpgradeInfo upgradeInfo = new UpgradeInfo();

    public delegate void OnlineResearchUpgrade(int playerId, int upgradeIndex);

	public static OnlineResearchUpgrade onlineResearchUpgrade;

    public void SetDelegate()
    {
        onlineResearchUpgrade = NetworkResearchUpgrade;
    }

    public void OnPointerEnter(PointerEventData eventData)
	{
		ObjectInfoUI.writeInfo.Invoke(upgradeInfo);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		ObjectInfoUI.cleanInfo.Invoke();
	}

    private void NetworkResearchUpgrade(int playerId, int upgradeIndex)
    {
        Player player = BattleMap.instance.GetPlayerWithID(playerId);
        Upgrade upgrade = player.capital.upgradeList[upgradeIndex];
        player.WasteGold(upgrade.goldCost);
        player.WasteOre(upgrade.oreCost);
        player.capital.turnEnded = true;
        player.SearchUpgrade(upgrade);

        if (!TurnController.currentPlayerNotLocal)
        {
		    ObjectInfoUI.cleanInfo.Invoke();
		    PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
		    BattleUIManager.offUI.Invoke();
        }
    }

    public void ResearchUpgrade()
    {
        capital.player.WasteGold(upgrade.goldCost);
        capital.player.WasteOre(upgrade.oreCost);
        
        capital.turnEnded = true;
        player.SearchUpgrade(upgrade);
		
		ObjectInfoUI.cleanInfo.Invoke();
        PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
		BattleUIManager.offUI.Invoke();
    }

    private void PressUpgradeButton()
    {
        _researchButton.interactable = false;
        if (CustomNetworkManager.IsOnlineGame)
		{
			LobbyController.localPlayerController.commandLouncher.ResearchUpgrade(player.id, capital.upgradeList.IndexOf(upgrade));
		}
		else
		{
			ResearchUpgrade();
		}
    }

    public void Render(Capital capital, Upgrade upgrade)
    {
        this.capital = capital;
        this.upgrade = upgrade;
        player = capital.player;

        upgradeInfo.Init(upgrade);

        RenderStatus();
		RenderFields();
    }

    private void RenderFields()
    {
        if (player.upgradeList.Contains(upgrade))
        {
			_goldCostField.gameObject.SetActive(false);
            _oreCostField.gameObject.SetActive(false);
            _researchButton.gameObject.SetActive(false);
        }
        else
        {
            if (upgrade.goldCost > 0)
                _goldCostField.gameObject.SetActive(true);
            
            if (upgrade.oreCost > 0)
                _oreCostField.gameObject.SetActive(true);

            _researchButton.gameObject.SetActive(true);
        }
    }

    private void RenderStatus()
    {
        _icon.sprite = upgrade.icon;
        _nameField.text = upgrade.Name;
        _goldCostField.text = upgrade.goldCost.ToString();
		_oreCostField.text = upgrade.oreCost.ToString();

        upgrade.SetStatus(player);
        _statusField.text = upgrade.statusDescription;

        if (upgrade.isResearched)
        {
            _researchButton.gameObject.SetActive(false);
        }
        else
        {
            _researchButton.gameObject.SetActive(true);

            if (upgrade.canBeResearched)
                _researchButton.interactable = true;
            else
                _researchButton.interactable = false;
        }
    }
}
