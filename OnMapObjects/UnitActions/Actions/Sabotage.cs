using System.Collections.Generic;

public class Sabotage : Attack
{
	private ActionDistanceFinder actionDistanceFinder = new ActionDistanceFinder();
	
	public override List<GroundCell> GetDistance(Unit unit)
	{
		return actionDistanceFinder.GetMechDistance(unit);
	}
}
