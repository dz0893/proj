using System.Collections;
using System.Collections.Generic;

public abstract class ObjectInfo
{
	public string objectName { get; protected set; }
	
	public abstract void Init(object obj);
}
