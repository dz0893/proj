using System.Collections.Generic;
using UnityEngine;

public class GlobalSummonObject : GlobalActionObject
{
	[SerializeField] private NullObject _summonedObject;
	public NullObject summonedObject => _summonedObject;
	
	[SerializeField] private bool _needFreeSpace;
	public bool needFreeSpace => _needFreeSpace;
	
	[SerializeField] private bool _onlyOnePerPlayer;
	public bool onlyOnePerPlayer => _onlyOnePerPlayer;

	[SerializeField] private bool _startTurnAfterSummon;
	public bool startTurnAfterSummon => _startTurnAfterSummon;
	
	[SerializeField] private List<TerrainType> _terrainTypeList;
	public List<TerrainType> terrainTypeList => _terrainTypeList;
	
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalSummon(this);
	}
}
