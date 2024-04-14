using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CapitalData/Unit")]
public class UnitDataObject : ScriptableObject
{	
	[SerializeField] private int _index;
	public int index => _index;
	
	[SerializeField] private int _goldCost;
	public int goldCost => _goldCost;

	[SerializeField] private int _oreCost;
	public int oreCost => _oreCost;
	
	[SerializeField] private Unit _unit;
	public Unit unit => _unit;
	
	[SerializeField] private List<Building> _requiredBuildingList;
	public List<Building> requiredBuildingList => _requiredBuildingList;
	
	[SerializeField] private List<int> _buildingLevelList;
	public List<int> buildingLevelList => _buildingLevelList;
	
	public UnitData GetUnitData(Player player)
	{
		return new UnitData(this, player);
	}
}
