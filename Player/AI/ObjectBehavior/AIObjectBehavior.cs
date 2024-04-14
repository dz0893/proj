using System.Collections;
using System.Collections.Generic;

public abstract class AIObjectBehavior
{
	public float actionTime { get; protected set; }
	
	public abstract void ActivateAction(NullObject obj);
}
