using System.Collections.Generic;
using UnityEngine;

public abstract class MissionObject : ScriptableObject
{
	[SerializeField] private int _index;
	[SerializeField] private string _name;
	[SerializeField] private string _description;
	[SerializeField] private MapEvent _afterMissionEvent;

	[SerializeField] private List<int> _targetCellsIndexList;
	public List<int> targetCellsIndexList => _targetCellsIndexList;

	public int index => _index;
	public string Name => _name;
	public string description => _description;
	public MapEvent afterMissionEvent => _afterMissionEvent;
	
	public abstract Mission AddMissionToPlayer(Player player);
}
