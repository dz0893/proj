using System.Collections.Generic;

public interface IAreaAction
{
	public int area { get; }
	
	public List<GroundCell> GetAreaDistance(GroundCell position);
}
