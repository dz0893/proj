using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecruitUnitCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Image _icon;
	[SerializeField] private Text _nameField;
	[SerializeField] private Text _goldCostField;
	[SerializeField] private Text _oreCostField;
	[SerializeField] private Text _leadershipCostField;
	[SerializeField] private Button _recruitButton;
	
	public UnitData unitData { get; private set;}
	
	private UnitDataInfo unitDataInfo = new UnitDataInfo();
	
	public RecruitPoint recruitPoint { get; set; }
	
	private Player player;
	
	public delegate void RecruitUnitInCellWithCurrentPosition(int cellIndex, int playerId, int unitIndex);

	public static RecruitUnitInCellWithCurrentPosition recruitUnitInCellWithCurrentPosition;

	public void SetDelegate()
	{
		recruitUnitInCellWithCurrentPosition = NetworkRecruitUnit;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ObjectInfoUI.writeInfo.Invoke(unitDataInfo);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		ObjectInfoUI.cleanInfo.Invoke();
	}
	
	public void OpenUnitInfo()
	{
		RecruitPointUI.openUnitInfo.Invoke(unitData.unit);
	}

	public void Render(UnitData unitData, Player player)
	{
		this.unitData = unitData;
		this.player = player;
		
		unitDataInfo.Init(unitData);
		
		_icon.sprite = unitData.unit.icon;
		_nameField.text = unitData.unit.Name;

		_goldCostField.text = unitData.goldCost.ToString();
		_oreCostField.text = unitData.oreCost.ToString();

		if (unitData.oreCost == 0)
			_oreCostField.gameObject.SetActive(false);
		else
			_oreCostField.gameObject.SetActive(true);

		_leadershipCostField.text = unitData.unit.leadershipCost.ToString();
		RenderStatusFieldAndRecruitButton(unitData);
	}
	
	public static void AIReviveHero(RecruitPoint recruitPoint)
	{
		recruitPoint.player.hero.Revive(recruitPoint.player, recruitPoint.position);
		recruitPoint.player.WasteGold(recruitPoint.player.reviveHeroGoldCost);
		recruitPoint.player.WasteOre(recruitPoint.player.reviveHeroOreCost);
		recruitPoint.player.countOfHeroReviving++;
		recruitPoint.player.hero.EndTurn();
	}
	
	public static void AIRecruitUnit(RecruitPoint recruitPoint, UnitData unitData)
	{
		Unit recrutedUnit = Instantiate(unitData.unit, recruitPoint.map.objectMap.transform);
		recrutedUnit.Init(recruitPoint.position, recruitPoint.player);
		
		recruitPoint.player.WasteGold(unitData.goldCost);
		recruitPoint.player.WasteOre(unitData.oreCost);
		
		recrutedUnit.EndTurn();
	}
	
	public void RecruitUnit()
	{
		_recruitButton.interactable = false;

		if (CustomNetworkManager.IsOnlineGame)
		{
			int cellIndex = BattleMap.instance.mapCellList.IndexOf(recruitPoint.position);
			LobbyController.localPlayerController.commandLouncher.RecruitUnit(cellIndex, recruitPoint.player.id, player.capital.unitDataList.IndexOf(unitData));
		}
		else
		{
			OfflineRecruitUnit();
		}
	}

	public void OfflineRecruitUnit()
	{
		Unit recrutedUnit = Instantiate(unitData.unit, recruitPoint.map.objectMap.transform);
		recrutedUnit.Init(recruitPoint.position, recruitPoint.player);
		
		player.WasteGold(unitData.goldCost);
		player.WasteOre(unitData.oreCost);
		
		recrutedUnit.EndTurn();
		
		ObjectInfoUI.cleanInfo.Invoke();
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
		BattleUIManager.offUI.Invoke();
	}
	
	private void NetworkRecruitUnit(int positionIndex, int playerId, int unitDataIndex)
	{
		GroundCell cell = BattleMap.instance.mapCellList[positionIndex];

		if (cell != null)
		{
			Player player = BattleMap.instance.GetPlayerWithID(playerId);
			UnitData unitData = player.capital.unitDataList[unitDataIndex];

			Unit recrutedUnit = Instantiate(unitData.unit, BattleMap.instance.objectMap.transform);
			recrutedUnit.Init(cell, player);
			recrutedUnit.EndTurn();

			player.WasteGold(unitData.goldCost);
			player.WasteOre(unitData.oreCost);

			if (!TurnController.currentPlayerNotLocal)
			{
				ObjectInfoUI.cleanInfo.Invoke();
				PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
				BattleUIManager.offUI.Invoke();
			}
		}
	}

	private void RenderStatusFieldAndRecruitButton(UnitData unitData)
	{
		unitData.SetStatus(player);

		if (unitData.canBeRecruited)
			_recruitButton.interactable = true;
		else
			_recruitButton.interactable = false;
	}
}
