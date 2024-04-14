using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public abstract class Mission : IEventRowObject
{
	public Player player { get; protected set; }
	
	public MissionObject missionObject { get; protected set; }
	
	public int turnOfActivation { get; private set; }
	
	public int index => missionObject.index;
	public string Name => missionObject.Name;
	public string description => missionObject.description;
	public MapEvent afterMissionEvent => missionObject.afterMissionEvent;

	public List<int> targetCellsIndexList => missionObject.targetCellsIndexList;
	private List<GroundCell> staticTargetCellList;
	
	public bool isEnded { get; private set; }
	public bool isFailed { get; private set; }
	public bool isMission => true;
	
	public static UnityEvent<IEventRowObject> missionEnded = new UnityEvent<IEventRowObject>();
	
	protected abstract bool CheckForEnded();
	
	public void TryToEndMission()
	{
		if (CheckForEnded() && !isEnded)
			EndMission();
	}
	
	public void EndMission()
	{
		turnOfActivation = TurnController.turnCounter;
		isEnded = true;
		missionEnded.Invoke(this);
		afterMissionEvent?.TryToActivateEvent(turnOfActivation);
		RemoveListener();
	}
	
	public void PlayOnLoad(EventRowObjectSaveInfo objectSaveInfo)
	{
		turnOfActivation = objectSaveInfo.turnOfActivation;
		isEnded = objectSaveInfo.isEnded;
		isFailed = objectSaveInfo.isFailed;
		RemoveListener();
		
		if (!isFailed)
			afterMissionEvent?.PlayEventRowOnLoad(objectSaveInfo);
	}
	
	public abstract void RemoveListener();
	
	public void FailMission()
	{
		turnOfActivation = TurnController.turnCounter;
		isEnded = true;
		isFailed = true;
		missionEnded.Invoke(this);
		RemoveListener();
	}

	protected void InitTargets()
	{
		InitStaticTargetList();
		SetTargetObjects();
	}

	private void InitStaticTargetList() 
	{
		staticTargetCellList = new List<GroundCell>();

		if (targetCellsIndexList.Count > 0)
		{
			foreach (GroundCell cell in BattleMap.instance.mapCellList)
			{
				if (targetCellsIndexList.Contains(cell.index))
					staticTargetCellList.Add(cell);
			}
		}
	}

	protected virtual void SetTargetObjects() {}

	public List<GroundCell> GetTargetList() 
	{
		if (isEnded)
		{
			return new List<GroundCell>();
		}
		else if (staticTargetCellList.Count > 0)
		{
			return staticTargetCellList;
		}
		else
		{
			return GetCurrentMissionTargetList();
		}
	}

	protected virtual List<GroundCell> GetCurrentMissionTargetList() { return new List<GroundCell>(); }
}
