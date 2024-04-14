using UnityEngine;

public abstract class AIGoalObject : ScriptableObject
{
	[SerializeField] private bool _currentUnitGoal;
	[SerializeField] private Unit _currentUnit;
	[SerializeField] private int _playerIndex;
	
	public bool currentUnitGoal => _currentUnitGoal;
	public Unit currentUnit => _currentUnit;
	public int playerIndex => _playerIndex;
	
	public abstract AIGoal GetGoal();
}
