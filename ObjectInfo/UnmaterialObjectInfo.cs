public class UnmaterialObjectInfo : ObjectInfo
{
	private UnmaterialObject unmaterialObject;
	
	public string description => unmaterialObject.description;
	
	public override void Init(object obj)
	{
		unmaterialObject = obj as UnmaterialObject;
		
		objectName = unmaterialObject.Name;
	}
}
