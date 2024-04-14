using System.Collections;
using UnityEngine;

public class AIGlobalActionsController
{
	public bool controllerInAction { get; private set; }
	
	public IEnumerator GlobalActionsCoroutine(Player player)
	{
		controllerInAction = true;
		GlobalAction globalAction = GetActivatedGlobalAction(player);
		
		if (globalAction != null)
		{
			GroundCell target = globalAction.GetAITarget(player);
			
			if (target != null)
			{
				MapController.selectGlobalAction.Invoke(globalAction);
				MapController.global.Invoke(target);
				yield return new WaitForSeconds(ActionSettings.GLOBALACTIONTIME);
			}
		}
		
		controllerInAction = false;
		yield return null;
	}
	
	
	private GlobalAction GetActivatedGlobalAction(Player player)
	{
		GlobalAction globalAction = null;
		
		foreach (GlobalAction action in player.globalActionList)
		{
			if (action.aiCanUse && action.CheckForActivating(player))
			{
				action.SetTargetList(player);
				
				if (action.areaList.Count != 0)
					globalAction = action;
			}
		}
		
		return globalAction;
	}
}
