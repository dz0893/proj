using UnityEngine;

public class OnGlobalActionActivator : EventActivator
{
    private string actionName;

    public OnGlobalActionActivator(OnGlobalActionActivatorObject activatorObject)
	{
		SetDefaultActivatorData(activatorObject);
		actionName = activatorObject.actionName;

		GlobalAction.GlobalWasActivated.AddListener(CheckGlobalAction);
	}

    private void CheckGlobalAction(string actionName)
    {
        if (this.actionName.Equals(actionName))
            TryToActivateEvent();
    }

    public override void RemoveListener()
	{
        GlobalAction.GlobalWasActivated.RemoveListener(CheckGlobalAction);
	}
}
