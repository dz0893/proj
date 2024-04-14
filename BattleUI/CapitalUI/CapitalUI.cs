using UnityEngine;

public class CapitalUI : MonoBehaviour
{
	[SerializeField] private CapitalInfoUI _capitalInfoUI;
	[SerializeField] private BuildingListUI _buildingListUI;
	[SerializeField] private UpgradeListUI _upgradeListUI;
	
	public BuildingListUI buildingListUI => _buildingListUI;
	public UpgradeListUI upgradeListUI => _upgradeListUI;
	
	private Structure structure;
	
	public void Init()
	{
		foreach (UpgradeCell cell in _upgradeListUI.upgradeCellList)
			cell.SetDelegate();
	}

	public void Render(Structure structure)
	{
		this.structure = structure;
		
		_capitalInfoUI.Render(structure);
		RenderBuildingList();
	}
	
	public void RenderBuildingList()
	{
		_upgradeListUI.gameObject.SetActive(false);

		if (NeedToRenderList())
		{
			_buildingListUI.gameObject.SetActive(true);
			_buildingListUI.Render(structure as Capital);
		}
		else
		{
			_buildingListUI.gameObject.SetActive(false);
		}
	}

	public void RenderUpgradeList()
	{
		_buildingListUI.gameObject.SetActive(false);

		if (NeedToRenderList())
		{
			_upgradeListUI.gameObject.SetActive(true);
			_upgradeListUI.Render(structure as Capital);
		}
		else
		{
			_upgradeListUI.gameObject.SetActive(false);
		}
	}

	private bool NeedToRenderList()
	{
		if (structure.player.id == CustomNetworkManager.localPlayerId && !structure.player.isAIPlayer && structure is Capital
		&& (CustomNetworkManager.IsOnlineGame || structure.player == TurnController.currentPlayer))
			return true;
		else
			return false;
	}
}
