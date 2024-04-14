using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractAction : MonoBehaviour, IAnimateIniter
{
	protected AttackDistanceFinder attackDistanceFinder = new AttackDistanceFinder();
	protected ActionTextGetter actionTextGetter = new ActionTextGetter();
	
	[SerializeField] private AnimationClip _onCasterAnimation;
	[SerializeField] private AnimationClip _flyAnimation;
	[SerializeField] private AnimationClip _contactAnimation;

	public AnimationClip onCasterAnimation => _onCasterAnimation;
	public AnimationClip flyAnimation => _flyAnimation;
	public AnimationClip contactAnimation => _contactAnimation;

	[SerializeField] private bool _needToRotateOnCasterAnimationToTarget;
	public bool needToRotateOnCasterAnimationToTarget => _needToRotateOnCasterAnimationToTarget;

	[SerializeField] private bool _shotFlyingWithCurve;
	public bool shotFlyingWithCurve => _shotFlyingWithCurve;

	[SerializeField] private float _startedOnCasterAnimationAngle;
	public float startedOnCasterAnimationAngle => _startedOnCasterAnimationAngle;

	[SerializeField] private AudioClip _castSound;
	[SerializeField] private AudioClip _contactSound;

	[SerializeField] private float _afterCastSoundDelay;
	public float afterCastSoundDelay => _afterCastSoundDelay;

	public AudioClip castSound => _castSound;
	public AudioClip contactSound => _contactSound;
	
	[SerializeField] private bool _fastActivate;
	public bool fastActivate => _fastActivate;

	[SerializeField] private bool _aiCanUse = true;
	public bool aiCanUse => _aiCanUse;
	
	[SerializeField] private bool _autoActivateAtTurnEnded;
	public bool autoActivateAtTurnEnded => _autoActivateAtTurnEnded;
	
	[SerializeField] private bool _needToRenderRoad = true;
	public bool needToRenderRoad => _needToRenderRoad;
	
	[SerializeField] private int _recuiredLevel;
	public int recuiredLevel => _recuiredLevel;
	
	[SerializeField] private Building _requiredBuilding;
	public Building requiredBuilding => _requiredBuilding;

	[SerializeField] private UpgradeObject _requiredUpgrade;
	public string requiredUpgradeName => _requiredUpgrade.Name;
	
	public bool needBuilding { get; private set; }
	public bool needLevel { get; private set; }
	public bool needUpgrade { get; private set; }
	
	[SerializeField] protected string _name;
	public virtual string Name => _name;
	
	[SerializeField] protected Sprite _icon;
	public virtual Sprite icon => _icon;
	
	[SerializeField] protected string _description;
	public virtual string description => _description;
	
	public List<string> descriptionList { get; private set; }
	
	[SerializeField] private bool _needFreeSpaceForActivate;
	public bool needFreeSpaceForActivate => _needFreeSpaceForActivate;

	[SerializeField] private UpgradeObject _costDecreasingUpgrade;

	[SerializeField] private int _goldCost;
	[SerializeField] private int _decreasedGoldCost;
	
	[SerializeField] private int _manaCost;
	[SerializeField] private int _decreasedManaCost;
	
	[SerializeField] protected int _damageIntegerModifire;
	[SerializeField] protected int _constDamage;
	[SerializeField] protected bool _modifiredAfterMove;
	[SerializeField] protected int _attackRange;
	[SerializeField] protected int _minAttackRange;
	
	[SerializeField] protected ActionEffect _actionEffect;
	public ActionEffect actionEffect => _actionEffect;
	
	public int damageIntegerModifire => _damageIntegerModifire;
	public int constDamage => _constDamage;
	public bool modifiredAfterMove => _modifiredAfterMove;
	public int AttackRange => _attackRange;
	public int MinAttackRange => _minAttackRange;
	
	public virtual bool usedOnCasterOnly => false;
	public virtual bool damageFromUnitStats => false;
	public virtual bool rangeFromUnitStats => false;
	
	public abstract bool endedTurnAction { get; }
	
	public abstract ActionType actionType { get; }
	public abstract ActionRange range { get; }
	
	public float actionTime { get; protected set; }  = ActionSettings.ATTACKTIME;
	
	public abstract IEnumerator MakeAction(Unit unit, GroundCell target);
	
	public abstract List<GroundCell> GetDistance(Unit unit);
	
	public virtual void PlaySound(AudioClip sound)
	{
		if (sound != null)
		{
			if (sound == _castSound)
				AudioManager.playSound.Invoke(_afterCastSoundDelay, sound);
			else
				AudioManager.playSound.Invoke(0, sound);
		}
	}

	public virtual int GetCurrentActionDamageModifire(Unit unit) { return 0; }

	public virtual List<GroundCell> GetAreaOfAction(Unit unit)
	{
		List<GroundCell> areaOfAction = new List<GroundCell>();
		
		if (this == unit.moveAction)
		{
			return areaOfAction;
		}
		if (range == ActionRange.OnCaster)
		{
				areaOfAction.Add(unit.position);
		}
		else if (range == ActionRange.Melee)
		{
			foreach (GroundCell cell in unit.position.closestCellList)
			{
				areaOfAction.Add(cell);
			}
		}
		else
		{
			areaOfAction = attackDistanceFinder.GetRangedActionDistanceWithMinRange(unit.position, unit.GetActionData(this).minAttackRange, unit.GetActionData(this).attackRange);
		}

		return areaOfAction;
	}

	public virtual GroundCell GetAITarget(Unit unit)
	{
		if (unit.currentActionTargetList.Count > 0)
			return unit.currentActionTargetList[0];
		else
			return null;
	}
	
	public ActionData GetActionData(Unit unit)
	{
		return new ActionData(unit, this);
	}

	public bool ChekActionForActive(Unit unit)
	{
		if (unit.player.gold < GetGoldCost(unit) && GetGoldCost(unit) != 0)
			return false;
		
		else if (unit.currentMana < GetManaCost(unit) && GetManaCost(unit) != 0)
			return false;
			
		return CheckForRequirements(unit) && CurrentActivateCheck(unit) && !unit.turnEnded && CheckForClosestEnemy(unit);
	}
	
	protected virtual bool CurrentActivateCheck(Unit unit) { return true; }
	
	private bool CheckForClosestEnemy(Unit unit)
	{
		bool canBeUsed = true;

		if (_needFreeSpaceForActivate)
		{
			foreach (GroundCell cell in unit.position.closestCellList)
			{
				if (cell.onCellObject != null && unit.team != cell.onCellObject.team && cell.onCellObject.canSurround)
				{
					canBeUsed = false;
					break;
				}
			}
		}

		return canBeUsed;
	}

	public bool CheckForRequirements(Unit unit)
	{
		needBuilding = IsNeededBuildingToUse(unit);
		needLevel = IsNeededLevelToUse(unit);
		needUpgrade = IsNeedUpgradeToUse(unit);
		
		if (!needBuilding && !needLevel && !needUpgrade)
			return true;
		else
			return false;
	}
	
	private bool IsNeededBuildingToUse(Unit unit)
	{
		if (_requiredBuilding != null && unit.player.capital != null)
		{
			foreach (BuildingData buildingData in unit.player.capital.buildingList)
			{
				if (buildingData.building == _requiredBuilding && buildingData.currentLevel < _recuiredLevel)
					return true;
			}
		}
		
		return false;
	}
	
	private bool IsNeededLevelToUse(Unit unit)
	{
		if (unit.experience.currentLevel < _recuiredLevel && _requiredBuilding == null)
			return true;
		else
			return false;
	}

	private bool IsNeedUpgradeToUse(Unit unit)
	{
		if (_requiredUpgrade == null)
			return false;
		
		foreach (Upgrade upgrade in unit.upgradeList)
		{
			if (upgrade.Name.Equals(requiredUpgradeName))
			{
				return false;
			}
		}

		return true;
	}
	
	protected void WasteResoursesAndEndTurn(Unit unit)
	{
		EndAction(unit);
		unit.WasteMana(GetManaCost(unit));
		unit.player.WasteGold(GetGoldCost(unit));
	}
	
	public int GetGoldCost(Unit unit)
	{
		if (IsCostDecreased(unit))
			return _decreasedGoldCost;
		else
			return _goldCost;
	}

	public int GetManaCost(Unit unit)
	{
		if (IsCostDecreased(unit))
			return _decreasedManaCost;
		else
			return _manaCost;
	}

	private bool IsCostDecreased(Unit unit)
	{
		if (_costDecreasingUpgrade == null)
			return false;
		
		foreach (Upgrade upgrade in unit.upgradeList)
		{
			if (upgrade.Name.Equals(_costDecreasingUpgrade.Name))
				return true;
		}

		return false;
	}

	protected virtual void EndAction(Unit unit)
	{
		if (endedTurnAction)
		{
			unit.EndTurn();
			unit.currentMovePoints = 0;
		}
		
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
	}
	
	public List<string> GetDescription(Unit unit, ActionData actionData)
	{
		descriptionList = new List<string>();
		SetBasicDescription(unit, actionData);
		SetCurrentDescription(unit);

		return descriptionList;
	}
	
	private void SetBasicDescription(Unit unit, ActionData actionData)
	{
		AddStringToRequiresList(description);
		AddStringToRequiresList(GetCostString(unit));
		AddStringToRequiresList(GetDamageString(actionData));
		AddStringToRequiresList(GetRangeString(actionData));
		SetEffectDescription();
		AddStringToRequiresList(GetRequiresString(unit));
		SetFreeSpaceString();
	}
	
	private void SetFreeSpaceString()
	{
		if (_needFreeSpaceForActivate)
			AddStringToRequiresList(UISettings.needFreeSpaceForActivate);
	}

	protected void AddStringToRequiresList(string addedString)
	{
		if (!addedString.Equals(""))
			descriptionList.Add(addedString);
	}
	
	private string GetCostString(Unit unit)
	{
		string costString = "";
		
		if (GetGoldCost(unit) != 0)
			costString = UISettings.cost + GetGoldCost(unit) + UISettings.gold;
		else if (GetManaCost(unit) != 0)
			costString = UISettings.Mana + GetManaCost(unit);
		
		return costString;
	} 
	
	protected virtual string GetDamageString(ActionData actionData)
	{
		return "";
	}
	
	protected virtual string GetRangeString(ActionData actionData)
	{
		string rangeString = UISettings.range;
		
		if (range == ActionRange.Melee)
			rangeString += UISettings.melee;
		else if (range == ActionRange.OnCaster)
			rangeString += UISettings.onCaster;
		else
			rangeString += (actionData.minAttackRange + 1) + "-" + actionData.attackRange;
		
		return rangeString;
	}
	
	private void SetEffectDescription()
	{
		if (actionEffect != null)
		{
			AddStringToRequiresList(GetEffectString());
			
			List<string> effectDescription = actionEffect.GetDescription();
		
			foreach (string descriptionString in effectDescription)
				AddStringToRequiresList(descriptionString);
		}
	}
	
	private string GetEffectString()
	{
		if (actionEffect.Name != "")
			return UISettings.Effect + actionEffect.Name;
		else
			return "";
	}
	
	private string GetRequiresString(Unit unit)
	{
		if (unit.player != null)
			return GetCurrentRequiresString(unit);
		else
			return GetDefaultRequiresString();
	}

	private string GetCurrentRequiresString(Unit unit)
	{
		string requiresString = "";
		
		if (IsNeededBuildingToUse(unit))
			requiresString = UISettings.needToBuild + _requiredBuilding.Name + " " + recuiredLevel + UISettings.lv;
		else if (IsNeededLevelToUse(unit))
			requiresString = UISettings.neededLevel + recuiredLevel;
		else if (IsNeedUpgradeToUse(unit))
			requiresString = UISettings.mustBuild + requiredUpgradeName;
		
		return requiresString;
	}

	private string GetDefaultRequiresString()
	{
		string requiresString = "";
		
		if (_requiredBuilding != null)
			requiresString = UISettings.needToBuild + _requiredBuilding.Name + " " + recuiredLevel + UISettings.lv;
		else if (recuiredLevel > 0)
			requiresString = UISettings.neededLevel + recuiredLevel;
		else if (_requiredUpgrade != null)
			requiresString = UISettings.mustBuild + requiredUpgradeName;
		
		return requiresString;
	}
	
	protected virtual void SetCurrentDescription(Unit unit) {}
	
	public virtual string GetRenderedText(Unit unit, GroundCell cell) { return ""; }
}