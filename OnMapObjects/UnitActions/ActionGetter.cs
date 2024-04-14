using System.Collections;
using System.Collections.Generic;

public class ActionGetter 
{
	public AbstractAction GetActionWithType(List<AbstractAction> actionList, ActionType actionType)
	{
		foreach (AbstractAction action in actionList)
		{
			if (action.actionType == actionType)
				return action;
		}
		
		return null;
	}
}
