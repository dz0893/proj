using System.Collections.Generic;

public class GlobalSummon : GlobalAction
{
	public NullObject summonedObject { get; private set; }
	public bool needFreeSpace { get; private set; }
	public bool onlyOnePerPlayer { get; private set; }
	public bool startTurnAfterSummon { get; private set; }
	public List<TerrainType> terrainTypeList { get; private set; }
	
	public GlobalSummon(GlobalSummonObject actionObject)
	{
		this.actionObject = actionObject;
		
		summonedObject = actionObject.summonedObject;
		needFreeSpace = actionObject.needFreeSpace;
		onlyOnePerPlayer = actionObject.onlyOnePerPlayer;
		startTurnAfterSummon = actionObject.startTurnAfterSummon;
		terrainTypeList = actionObject.terrainTypeList;
		
		InitDescriptionList();
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		NullObject obj = BattleMap.initObject.Invoke(summonedObject, player, target);

		if (startTurnAfterSummon)
		{
			obj.StartTurn();
		}
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		return globalActionDistanceFinder.GetRemovedCellsForSummon(areaList, this);
	}
	
	protected override bool CheckCurrentActivate(Player player)
	{
		currentActivatingWarningText = "";
		
		if (onlyOnePerPlayer)
		{
			foreach (NullObject obj in player.objectList)
			{
				if (summonedObject.Name.Equals(obj.Name))
				{
					currentActivatingWarningText = UISettings.onlyOnePerPlayer;
					return false;
				}
			}
		}
		
		else if (summonedObject.leadershipCost > player.maxUnitLimit - player.currentUnitLimit)
		{
			currentActivatingWarningText = UISettings.notEnoughtLimit;
			return false;
		}
		
		return true;
	}

	protected override void InitChildrenDescription()
	{
		if (terrainTypeList.Count == 0)
			return;
		
		string currentDescription = UISettings.CanBeBuildedOn + '\n';
		
		for (int i = 0; i < terrainTypeList.Count; i++)
		{
			currentDescription += GroundSettings.GetTerrainName(terrainTypeList[i]);

			if (i < terrainTypeList.Count - 1)
				currentDescription += ", ";
			else
				currentDescription += " ";
		}
		
		AddStringToRequiresList(currentDescription);
	}
}
