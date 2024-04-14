using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeObject : ScriptableObject
{
    [SerializeField] private Sprite _icon;
	public Sprite icon => _icon;
	
	[SerializeField] private string _name;
	public string Name => _name;

    [SerializeField] private string _description;
	public string description => _description;

    [SerializeField] private Building _requiredBuilding;
    public Building requiredBuilding => _requiredBuilding;

    [SerializeField] private int _buildingLevel;
    public int buildingLevel => _buildingLevel;

    [SerializeField] private int _goldCost;
    public int goldCost => _goldCost;

    [SerializeField] private int _oreCost;
    public int oreCost => _oreCost;

    [SerializeField] private List<NullObject> _targetList;
    public List<NullObject> targetList => _targetList;

    public abstract Upgrade GetUpgrade();
}
