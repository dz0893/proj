using System.Collections.Generic;

public abstract class AIGoal
{
	public bool currentUnitGoal { get; protected set; }
	public Unit currentUnit { get; protected set; }
	
	public abstract List<GroundCell> GetGoalList(Unit unit);
	public abstract AbstractAction GetAction(Unit unit);
}
