using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Image _iconFrame;
	[SerializeField] private Image _background;

	[SerializeField] private Image _icon;
	[SerializeField] private Image _goldIcon;
	[SerializeField] private Image _oreIcon;
	[SerializeField] private Text _nameField;
	[SerializeField] private Text _goldCostField;
	[SerializeField] private Text _oreCostField;
	[SerializeField] private Text _statusField;
	[SerializeField] private Button _buildButton;

	[SerializeField] private Sprite _activeBackgroundSprite;
	[SerializeField] private Sprite _inactiveBackgroundSprite;
	[SerializeField] private Sprite _activeIconFrameSprite;
	[SerializeField] private Sprite _inactiveIconFrameSprite;

	[SerializeField] private BuildingLevelRenderer _buildingLevelRenderer;
	
	private Capital capital;
	private BuildingData buildingData;
	private BuildingDataInfo info = new BuildingDataInfo();
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		ObjectInfoUI.writeInfo.Invoke(info);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		ObjectInfoUI.cleanInfo.Invoke();
	}
	
	public void Build()
	{
		_buildButton.interactable = false;
		capital.player.WasteGold(buildingData.upgradeGoldCost);
		capital.player.WasteOre(buildingData.upgradeOreCost);
		capital.Build(buildingData);
		
		ObjectInfoUI.cleanInfo.Invoke();
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
		BattleUIManager.offUI.Invoke();
	}
	
	public void Render(Capital capital, BuildingData buildingData)
	{
		this.capital = capital;
		this.buildingData = buildingData;
		
		info.Init(buildingData);
		
		_buildingLevelRenderer.Render(buildingData, capital.player);
		RenderInfo();
		RenderBuildButtonAndBackground();
	}
	
	private void RenderInfo()
	{
		_icon.sprite = buildingData.building.icon;
		_nameField.text = buildingData.building.Name;
		
		if (buildingData.maxLeveled)
		{
			_goldCostField.gameObject.SetActive(false);
			_oreCostField.gameObject.SetActive(false);

			_goldIcon.gameObject.SetActive(false);
			_oreIcon.gameObject.SetActive(false);
		}
		else
		{
			_goldCostField.gameObject.SetActive(true);
			_goldIcon.gameObject.SetActive(true);

			if (buildingData.upgradeOreCost > 0)
			{
				_oreCostField.gameObject.SetActive(true);
				_oreIcon.gameObject.SetActive(true);
			}
			else
			{
				_oreCostField.gameObject.SetActive(false);
				_oreIcon.gameObject.SetActive(false);
			}

			_goldCostField.text = buildingData.upgradeGoldCost.ToString();
			_oreCostField.text = buildingData.upgradeOreCost.ToString();
		}
	}

	private void RenderBuildButtonAndBackground()
	{
		buildingData.SetStatus(capital.player);
		_buildButton.gameObject.SetActive(true);

		if (buildingData.maxLeveled)
		{
			_buildButton.gameObject.SetActive(false);
		}
		else if (buildingData.canBeBuilded)
		{
			_buildButton.interactable = true;
		}
		else
		{
			_buildButton.interactable = false;
		}
	}
}
