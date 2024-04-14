using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DefeatEnemyMission : Mission
{
	private bool fullDestroing;
	private bool defeatTeam;
	private int playerIndex;
	
	private Player targetedPlayer;
	private List<Capital> capitalList;

	public DefeatEnemyMission(DefeatEnemyMissionObject missionObject, Player player)
	{
		this.missionObject = missionObject;
		this.player = player;
		
		fullDestroing = missionObject.fullDestroing;
		defeatTeam = missionObject.defeatTeam;
		playerIndex = missionObject.playerIndex;
		
		InitTargets();

		NullObject.ObjectDied.AddListener(ActivateChecking);
	}

	private void ActivateChecking(NullObject obj)
	{
		TryToEndMission();
	}
	
	protected override bool CheckForEnded()
	{
		return CheckPlayerForDefeat();
	}
	
	private bool CheckPlayerForDefeat()
	{
		bool missionComplete = true;
		
		if (capitalList.Count > 0 && !fullDestroing)
		{
			foreach (Capital capital in capitalList)
			{
				if (!capital.isDead)
					missionComplete = false;
			}
		}
		else
		{
			foreach (NullObject obj in targetedPlayer.objectList)
			{
				if (obj is MaterialObject || obj is RecruitPoint)
					missionComplete = false;
			}
		}

		return missionComplete;
	}
	
	protected override void SetTargetObjects()
	{
		targetedPlayer = BattleMap.instance.playerList[playerIndex];
		capitalList = new List<Capital>();

		if (defeatTeam)
		{
			foreach (Player checkedPlayer in BattleMap.instance.playerList)
			{
				if (checkedPlayer.team == targetedPlayer.team && checkedPlayer.capital != null)
					capitalList.Add(checkedPlayer.capital);
			}
		}
		else if (targetedPlayer.capital != null)
		{
			capitalList.Add(targetedPlayer.capital);
		}
	}

	protected override List<GroundCell> GetCurrentMissionTargetList()
	{
		List<GroundCell> targetList = new List<GroundCell>();

		foreach (Capital capital in capitalList)
		{
			if (!capital.isDead)
				targetList.Add(capital.position);
		}

		Debug.Log("Target list " + targetList.Count);
		return targetList;
	}

	public override void RemoveListener()
	{
		NullObject.ObjectDied.RemoveListener(ActivateChecking);
	}
}
