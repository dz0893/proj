using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitPointUI : ObjectUI
{
	[SerializeField] private RecrutingList _recrutingList;
	[SerializeField] private UnitInfoUI _unitInfoUI;

	[SerializeField] private ReviveHeroCell _reviveHeroCell;
	[SerializeField] private Button _reviveHeroButton;
	
	public RecruitPoint recruitPoint { get; private set; }
	
	protected override bool canBeOpenedByOtherPlayer => false;
	
	private Player player => recruitPoint.player;

	public delegate void OpenUnitInfo(Unit unit);
	public delegate void OpenUnitInfoWithIndex(int index);
	public delegate void OnlineReviveHero(int cellIndex, int playerId);

	public static OpenUnitInfo openUnitInfo;
	public static OpenUnitInfoWithIndex openUnitInfoWithIndex;
	public static OnlineReviveHero onlineReviveHero;
	
	public void Init()
	{
		onlineReviveHero = NetworkReviveHero;
		openUnitInfo = OpenUnitInfoUi;
		openUnitInfoWithIndex = OpenUnitInfoUiWithIndex;

		foreach (RecruitUnitCell cell in _recrutingList.CellList)
			cell.SetDelegate();
	}

	public void OpenUnitInfoUi(Unit unit)
	{
		_recrutingList.gameObject.SetActive(false);
		_unitInfoUI.gameObject.SetActive(true);
		_unitInfoUI.Render(unit, player, recruitPoint);
	}
	
	public void OpenUnitInfoUiWithIndex(int index)
	{
		_recrutingList.gameObject.SetActive(false);
		_unitInfoUI.gameObject.SetActive(true);
		_unitInfoUI.Render(GetUnitWithIndex(index), player, recruitPoint);
	}

	public Unit GetUnitWithIndex(int unitIndex)
    {
        if (recruitPoint.unitDataList.Count == unitIndex)
            return player.hero;
        else
            return recruitPoint.unitDataList[unitIndex].unit;
    }

	public void OpenHeroInfo()
	{
		_recrutingList.gameObject.SetActive(false);
		_unitInfoUI.gameObject.SetActive(true);
		_unitInfoUI.Render(recruitPoint.player.hero, player, recruitPoint);
	}

	public void OpenUintList()
	{
		_recrutingList.gameObject.SetActive(true);
		_unitInfoUI.gameObject.SetActive(false);
	}

	protected override void Render(NullObject obj)
	{	
		RecruitPoint recruitPoint = obj as RecruitPoint;
		OpenUintList();
		
		this.recruitPoint = recruitPoint;
		_recrutingList.recruitPoint = recruitPoint;
		
		_recrutingList.Render(recruitPoint.unitDataList, player);
		
		_reviveHeroCell.Render(player, recruitPoint);
	}

	public static bool CheckForReviveHero(RecruitPoint recruitPoint)
	{
		if (recruitPoint.player.hero != null 
		&& recruitPoint.player.hero.isDead 
		&& recruitPoint.player.gold >= recruitPoint.player.reviveHeroGoldCost
		&& recruitPoint.player.ore >= recruitPoint.player.reviveHeroOreCost 
		&& recruitPoint.player == TurnController.currentPlayer
		&& recruitPoint.player.maxUnitLimit - recruitPoint.player.currentUnitLimit >= recruitPoint.player.hero.leadershipCost)
			return true;
		else
			return false;
	}

	public void ReviveHero()
	{
		_reviveHeroButton.interactable = false;
		
		if (CustomNetworkManager.IsOnlineGame)
		{
			int cellIndex = BattleMap.instance.mapCellList.IndexOf(recruitPoint.position);
			LobbyController.localPlayerController.commandLouncher.ReviveHero(cellIndex, player.id);
		}
		else
		{
			OfflineReviveHero();
		}
	}

	public void OfflineReviveHero()
	{
		player.hero.Revive(player, recruitPoint.position);
		player.WasteGold(player.reviveHeroGoldCost);
		player.WasteOre(player.reviveHeroOreCost);
		player.countOfHeroReviving++;
		player.hero.EndTurn();
		
		ObjectInfoUI.cleanInfo.Invoke();
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
		BattleUIManager.offUI.Invoke();
	}

	private void NetworkReviveHero(int positionIndex, int playerId)
	{
		GroundCell cell = BattleMap.instance.mapCellList[positionIndex];

		if (cell != null)
		{
			Player player = BattleMap.instance.GetPlayerWithID(playerId);

			player.hero.Revive(player, cell);
			player.WasteGold(player.reviveHeroGoldCost);
			player.WasteOre(player.reviveHeroOreCost);
			player.countOfHeroReviving++;
			player.hero.EndTurn();

			if (!TurnController.currentPlayerNotLocal)
			{
				ObjectInfoUI.cleanInfo.Invoke();
				PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
				BattleUIManager.offUI.Invoke();
			}
		}
	}
}
