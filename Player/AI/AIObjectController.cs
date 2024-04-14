using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIObjectController : MonoBehaviour
{
	AIGlobalActionsController aiGlobalActionsController = new AIGlobalActionsController();
	
	[SerializeField] private ActionController _actionController;
	[SerializeField] private AbstractAction _buildMineAction;
	
	public delegate void MakeTurn(Player player);
	public static MakeTurn makeTurn;
	
	private AIRecruitPointBehavior aiRecruitPointBehavior = new AIRecruitPointBehavior();
	private AIUnitBehavior aiUnitBehavior = new AIUnitBehavior();
	
	private NullObject currentObject;
	
	private bool aiObjectControllerInAction;

	private float actionTime;
	
	private void Start()
	{
		makeTurn = ActivateAllActions;
	}
	
	public void ActivateAllActions(Player player)
	{
		StartCoroutine(PlayCoroutines(player));
	}
	
	private IEnumerator PlayCoroutines(Player player)
	{
		yield return new WaitForSeconds(0.4f);
		
		StartCoroutine(aiGlobalActionsController.GlobalActionsCoroutine(player));
		
		while (aiGlobalActionsController.controllerInAction)
			yield return null;
		
		StartCoroutine(ActionsCoroutine(player));
		
		while (aiObjectControllerInAction)
			yield return null;
	}
	
	private IEnumerator ActionsCoroutine(Player player)
	{
		aiObjectControllerInAction = true;

		if (player.aiPlayer.movingAtFirstTurn || TurnController.turnCounter != 0)
		{
			for (int i = 0; i < player.objectList.Count; i++)
			{
				currentObject = player.objectList[i];
				
				ActivateUnitAction(currentObject);
				
				while (ActionController.controllerInAction)
				{
					yield return null;
				}

				while (!currentObject.isDead && aiUnitBehavior.CheckForSecondAction(currentObject))
				{
					ActivateUnitAction(currentObject);
					
					while (ActionController.controllerInAction)
						yield return null;
				}
				
				if (currentObject.isDead)
					i--;
			}
		}
		
		for (int i = player.objectList.Count - 1; i >= 0; i--)
		{
			
			RecruitNewUnits(player.objectList[i]);
			yield return new WaitForSeconds(actionTime);
		}
		
		aiObjectControllerInAction = false;
		TurnStateUI.skipTurn.Invoke();
		yield return null;
	}
	
	private void RecruitNewUnits(NullObject obj)
	{
		actionTime = 0;
		
		if (obj is RecruitPoint)
		{
			RecruitPoint recruitPoint = obj as RecruitPoint;
			
			aiRecruitPointBehavior.ActivateAction(obj);
			
			if (aiRecruitPointBehavior.reqruitingSucsessed)
			{
				if (GameOptions.cameraFollowingAI)
				{
					if (recruitPoint.initer != null)
						CameraController.setCameraPosition.Invoke(recruitPoint.initer.transform.position);
					else
						CameraController.setCameraPosition.Invoke(recruitPoint.transform.position);
				}
				
				actionTime = 0.3f;
			}
		}
	}
	
	private void ActivateUnitAction(NullObject obj)
	{
		actionTime = 0;
		
		try
		{
			if (obj is Unit)
			{
				BattleMap.instance.ClearMapCash();
				
				aiUnitBehavior.ActivateAction(obj);
				Unit unit = obj as Unit;
				
				if (aiUnitBehavior.actionTarget != null)
					StartCoroutine(_actionController.MakeAction(unit, aiUnitBehavior.actionTarget));
			}
		}
		catch (Exception e)
		{
			Debug.Log(obj.Name);
			obj.position.GetComponent<SpriteRenderer>().color = new Vector4(1,0,0,1);
			Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			Debug.Log(e);
		}
	}
}
