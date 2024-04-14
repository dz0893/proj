using UnityEngine;

[CreateAssetMenu(menuName = "Events/Goals/DefencePosition")]
public class DefencePositionGoalObject : AIGoalObject
{
	[SerializeField] private int _defendedCellsIndex;
	public int defendedCellsIndex => _defendedCellsIndex;
	
	[SerializeField] private int _basicCellIndex;
	public int basicCellIndex => _basicCellIndex;
	
	public override AIGoal GetGoal()
	{
		return new DefencePositionGoal(this);
	}
}
