using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectMission : Mission
{
	private string objectName;
	private NullObject destroyedObject;
	private int playerIndex;
	
	public DestroyObjectMission(DestroyObjectMissionObject missionObject, Player player)
	{
		this.missionObject = missionObject;
		this.player = player;
		
		//destroyedObject = missionObject.destroyedObject;
		objectName = missionObject.destroyedObject.Name;
		playerIndex = missionObject.playerIndex;
		
		InitTargets();

		NullObject.ObjectDied.AddListener(ActivateChecking);
	}

	protected override void SetTargetObjects()
	{
		foreach (NullObject obj in BattleMap.instance.objectList)
		{
			if (obj.Name.Equals(objectName) && obj.player == BattleMap.instance.playerList[playerIndex])
			{
				destroyedObject = obj;
				break;
			}
		}
	}

	protected override List<GroundCell> GetCurrentMissionTargetList()
	{
		List<GroundCell> targetList = new List<GroundCell>();

		if (!destroyedObject.isDead)
			targetList.Add(destroyedObject.position);

		return targetList;
	}
	
	private void ActivateChecking(NullObject obj)
	{
		//if (obj.Name.Equals(destroyedObject.Name) && obj.player == BattleMap.instance.playerList[playerIndex])
		if (obj == destroyedObject)
		{
			TryToEndMission();
		}
	}
	
	protected override bool CheckForEnded()
	{
		foreach (NullObject obj in BattleMap.instance.objectList)
		{
			if (destroyedObject.Name.Equals(obj.Name))
				return false;
		}
		
		return true;
	}
	
	public override void RemoveListener()
	{
		NullObject.ObjectDied.RemoveListener(ActivateChecking);
	}
}
