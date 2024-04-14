using System.Collections.Generic;

public class GroundInfo : ObjectInfo
{
	public GroundCell groundCell { get; private set; }
	
	public int currentOreValue => groundCell.currentOreValue;
	public int movingCost => groundCell.movingCost;
	public string movingType => groundCell.movingType.ToString();
	public List<Unit> grave => groundCell.grave;
	
	public override void Init(object obj)
	{
		groundCell = obj as GroundCell;
		
		objectName = groundCell.Name;
	}
}
