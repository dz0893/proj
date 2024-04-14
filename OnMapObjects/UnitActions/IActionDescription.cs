using System.Collections.Generic;
using UnityEngine;

public interface IActionDescription
{
	public string Name { get; }
	public List<string> descriptionList { get; }
	
	public bool isAreaAction { get; }
	
	public Sprite icon { get; }

	public List<GroundCell> GetAreaDistance(GroundCell target);
	public string GetRenderedText(GroundCell cell);
}
