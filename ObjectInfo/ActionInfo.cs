using System.Collections.Generic;

public class ActionInfo : ObjectInfo
{
	public IActionDescription actionDescription { get; private set; }
	
	public List<string> descriptionList => actionDescription.descriptionList;
	
	public override void Init(object obj)
	{
		actionDescription = obj as IActionDescription;
		
		objectName = actionDescription.Name;
	}
}
