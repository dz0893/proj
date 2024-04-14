using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GlobalAction : IActionDescription, IAnimateIniter
{
	protected GlobalActionDistanceFinder globalActionDistanceFinder = new GlobalActionDistanceFinder();
	protected ActionTextGetter actionTextGetter = new ActionTextGetter();
	
	public GlobalActionObject actionObject { get; protected set; }
	
	public AnimationClip onCasterAnimation => actionObject.onCasterAnimation;
	public AnimationClip flyAnimation => actionObject.flyAnimation;
	public AnimationClip contactAnimation => actionObject.contactAnimation;

	public bool needToRotateOnCasterAnimationToTarget => false;
    public bool shotFlyingWithCurve => false;
    public float startedOnCasterAnimationAngle => 0;

	public AudioClip castSound => actionObject.castSound;
	public AudioClip contactSound => actionObject.contactSound;

	public float afterCastSoundDelay => actionObject.afterCastSoundDelay;

	public Sprite icon => actionObject.icon;
	
	public string Name => actionObject.Name;
	public string description => actionObject.description;
	
	public int oreCost => actionObject.oreCost;
	public int goldCost => actionObject.goldCost;
	
	public int actionValue => actionObject.actionValue;
	
	public bool aiCanUse => actionObject.aiCanUse;
	
	public int buildingLevel => actionObject.buildingLevel;
	public Building requiredBuilding => actionObject.requiredBuilding;
	
	public GlobalActionRange actionRange => actionObject.actionRange;
	public int distance => actionObject.distance;
	
	public List<GroundCell> areaList { get; private set; }
	public bool usedAtThisTurn;
	
	public bool isAreaAction => actionObject.isAreaAction;

	public string currentActivatingWarningText { get; protected set; } = "";
	
	public List<string> descriptionList { get; private set; } = new List<string>();
	
	public static UnityEvent<string> GlobalWasActivated = new UnityEvent<string>();

	public virtual void PlaySound(AudioClip sound)
	{
		if (sound != null)
			AudioManager.playSound.Invoke(afterCastSoundDelay, sound);
	}

	protected void InitDescriptionList()
	{
		InitParentDescription();
		InitChildrenDescription();
	}
	
	protected void AddStringToRequiresList(string addedString)
	{
		if (!addedString.Equals(""))
			descriptionList.Add(addedString);
	}
	
	private void InitParentDescription()
	{
		string oreCostString = UISettings.Ore + oreCost;
		string goldCostString = "";
		
		if (goldCost > 0)
			goldCostString = UISettings.Gold + goldCost;
		
		AddStringToRequiresList(description + "\n");
		AddStringToRequiresList(oreCostString);
		AddStringToRequiresList(goldCostString);
		AddStringToRequiresList(GetRangeString());
		AddStringToRequiresList(GetActionValueString());
	}
	
	protected virtual string GetRangeString()
	{
		if (actionRange == GlobalActionRange.OnHero)
			return UISettings.range + distance + UISettings.fromHero;
		else if (actionRange == GlobalActionRange.OnCapital)
			return UISettings.range + distance + UISettings.fromCapital;
		else 
			return UISettings.range + UISettings.allMap;
		
	}
	
	protected virtual void InitChildrenDescription() {}
	
	protected virtual string GetActionValueString() { return "";}
	
	public void Activate(Player player, GroundCell target)
	{
		AnimationController.play.Invoke(this, null, target, true);

		player.WasteOre(oreCost);
		player.WasteGold(goldCost);
		
		usedAtThisTurn = true;
		
		CurrentActivate(player, target);

		GlobalWasActivated.Invoke(Name);
	}
	
	public bool CheckForActivating(Player player)
	{
		if (CheckBuildings(player) && CheckResourses(player) && CheckCurrentActivate(player) && !usedAtThisTurn
		&& (!player.hero.isDead || actionRange != GlobalActionRange.OnHero))
			return true;
		else
			return false;
	}
	
	public bool CheckResourses(Player player)
	{
		if (player.ore >= oreCost && player.gold >= goldCost)
			return true;
		
		else
			return false;
	}
	
	public bool CheckBuildings(Player player)
	{
		if (requiredBuilding == null || player.capital == null)
			return true;
		
		foreach (BuildingData buildingData in player.capital.buildingList)
		{
			if (buildingData.building == requiredBuilding && buildingData.currentLevel >= buildingLevel)
				return true;
		}
		
		return false;
	}
	
	protected virtual bool CheckCurrentActivate(Player player) { return true; }
	
	protected abstract void CurrentActivate(Player player, GroundCell target);
	
	public virtual GroundCell GetAITarget(Player player)
	{
		return areaList[0];
	}
	
	public void SetTargetList(Player player)
	{
		areaList = new List<GroundCell>();
		
		if (actionRange == GlobalActionRange.OnHero)
			areaList = globalActionDistanceFinder.GetAreaInRange(player.hero.position, distance);
		else if (actionRange == GlobalActionRange.OnCapital)
			areaList = globalActionDistanceFinder.GetAreaInRange(player.capital.position, distance);
		else if (actionRange == GlobalActionRange.OnAllies)
			areaList = globalActionDistanceFinder.GetAllObjects(player, true);
		else if (actionRange == GlobalActionRange.OnEnemies)
			areaList = globalActionDistanceFinder.GetAllObjects(player, false);
		else
			areaList = globalActionDistanceFinder.GetFullMapCells();
		
		List<GroundCell> removedCells = GetRemoovedCells(player, areaList);
		
		foreach (GroundCell cell in removedCells)
			areaList.Remove(cell);
	}
	
	protected abstract List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList);
	
	public virtual string GetRenderedText(GroundCell cell) { return ""; }

	public virtual List<GroundCell> GetAreaDistance(GroundCell target)
	{
		List<GroundCell> areaList = new List<GroundCell>();
		
		areaList.Add(target);
		
		if (isAreaAction)
		{
			foreach (GroundCell cell in target.closestCellList)
				areaList.Add(cell);
		}
		
		return areaList;
	}
}
