using UnityEngine;

[CreateAssetMenu(menuName = "Events/Goals/GoToCell")]
public class GoToCellGoalObject : AIGoalObject
{
	[SerializeField] private int _cellIndex;
	public int cellIndex => _cellIndex;
	
	public override AIGoal GetGoal()
	{
		return new GoToCellGoal(this);
	}
}
