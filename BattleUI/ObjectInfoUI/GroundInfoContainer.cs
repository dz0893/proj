using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundInfoContainer : AbstractInfoContainer
{
	[SerializeField] private Text _movingCostField;
	[SerializeField] private Text _oreField;
	[SerializeField] private Transform _descriptionContainer;
	[SerializeField] private Text _descriptionString;
	
	[SerializeField] private Text _graveField;
	[SerializeField] private Text _graveHeader;
	
	public override void Render(ObjectInfo info)
	{
		GroundInfo groundInfo = info as GroundInfo;
		
		RenderOreField(groundInfo);
		RenderMovingCostField(groundInfo);
		RenderTerrainEffect(groundInfo.groundCell.terrainEffect);
		RenderGrave(groundInfo.grave);
	}
	
	private void RenderMovingCostField(GroundInfo groundInfo)
	{
		if (groundInfo.groundCell.movingType == MovingType.Walk)
			_movingCostField.text = UISettings.movingCost + groundInfo.movingCost;
		else
			_movingCostField.text = UISettings.UnmovedCell;
	}
	
	private void RenderTerrainEffect(OnCellEffectData terrainEffect)
	{
		CleanTerrain();
		
		if (terrainEffect != null)
			InitEffectDescription(terrainEffect);
	}
	
	private void CleanTerrain()
	{
		foreach (Transform child in _descriptionContainer)
			Destroy(child.gameObject);
	}
	
	private void RenderOreField(GroundInfo groundInfo)
	{
		if (groundInfo.groundCell.terrainType == TerrainType.GoldDeposit || groundInfo.groundCell.terrainType == TerrainType.OreDeposit)
		{
			_oreField.gameObject.SetActive(true);
			_oreField.text = UISettings.OreValue + groundInfo.currentOreValue;
		}
		else
		{
			_oreField.gameObject.SetActive(false);
		}
	}

	private void InitEffectDescription(OnCellEffectData terrainEffect)
	{
		List<string> effectDescription = terrainEffect.GetEffectDescription();
			
		foreach (string descriptionString in effectDescription)
		{
			Text effectText = Instantiate(_descriptionString, _descriptionContainer);
			effectText.text = descriptionString;
		}
	}
	
	private void RenderGrave(List<Unit> unitList)
	{
		CleanGrave();
		InitGrave(unitList);
		
		if (unitList.Count == 0)
			_graveHeader.gameObject.SetActive(false);
		else
			_graveHeader.gameObject.SetActive(true);
	}
	
	private void CleanGrave()
	{
		_graveField.text = "";
	}
	
	private void InitGrave(List<Unit> unitList)
	{
		for (int i = 0; i < unitList.Count; i++)
		{
			_graveField.text += unitList[i].Name;
			
			if (i < unitList.Count - 1)
				_graveField.text += ", ";
		}
	}
}
