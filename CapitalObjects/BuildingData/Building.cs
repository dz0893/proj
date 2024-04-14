using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CapitalData/Building")]
public class Building : ScriptableObject
{
	[SerializeField] private Sprite _icon;
	public Sprite icon => _icon;
	
	[SerializeField] private string _name;
	public string Name => _name;
	
	[SerializeField] private int _maxLevel;
	public int maxLevel => _maxLevel;
	
	[SerializeField] private List<int> _upgradeCostList;
	public List<int> upgradeCostList => _upgradeCostList;

	[SerializeField] private List<int> _upgradeGoldCostList;
	public List<int> upgradeGoldCostList => _upgradeGoldCostList;

	[SerializeField] private List<int> _upgradeOreCostList;
	public List<int> upgradeOreCostList => _upgradeOreCostList;
}
