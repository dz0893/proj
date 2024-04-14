using System.Collections.Generic;
using UnityEngine;

public class Unit : MaterialObject
{
	public Experience experience { get; protected set; }
	
	[SerializeField] private bool _canSurround = true;
	public override bool canSurround => _canSurround;

	[SerializeField] private int _counterAttackModifier = 2;
	public int counterAttackModifier => _counterAttackModifier;

	[SerializeField] private List<GlobalActionObject> _heroGlobalActionList;
	public List<GlobalActionObject> heroGlobalActionList => _heroGlobalActionList;
	
	[SerializeField] private UnitType _unitType;
	public UnitType unitType => _unitType;
	
	[SerializeField] private bool _goingToGraveAtDeath = true;
	public override bool goingToGraveAtDeath => _goingToGraveAtDeath;
	
	[SerializeField] protected bool _showingInUnitList = true;
	public bool showingInUnitList => _showingInUnitList;

	[SerializeField] protected bool _isMech;
	public override bool isMech => _isMech;
	
	[SerializeField] private bool _isUndead;
	public override bool isUndead => _isUndead;
	
	[SerializeField] private bool _isMovable = true;
	public override bool isMovable => _isMovable;
	
	[SerializeField] private bool _haveHealingTerrain;
	public bool haveHealingTerrain => _haveHealingTerrain;
	[SerializeField] private TerrainType _healingTerrain;
	public TerrainType healingTerrain => _healingTerrain;
	
	private MovingDistanceFinder movingDistanceFinder = new MovingDistanceFinder();
	
	[SerializeField] private AuraData _auraData;
	public AuraData auraData => _auraData;
	public Aura aura { get; set; }
	
	[SerializeField] private List<AbstractAction> _actionList;
	public List<AbstractAction> actionList => _actionList;
	
	[SerializeField] private AbstractAction _counterAttack;
	public AbstractAction counterAttack => _counterAttack;
	
	[SerializeField] private Move _moveAction;
	public Move moveAction => _moveAction;
	
	[SerializeField] private AbstractAction _startedTurnAction;
	public AbstractAction startedTurnAction => _startedTurnAction;
	
	[SerializeField] private AbstractAction _defaultAIAction;
	public AbstractAction defaultAIAction => _defaultAIAction;

	[SerializeField] private AbstractAction _onDeathAction;
	public AbstractAction onDeathAction => _onDeathAction;

	[SerializeField] private List<AudioClip> _actionVoiceLine;

	public List<ActionData> actionDataList { get; private set; }
	public ActionData counterAttackActionData { get; private set; }
	public ActionData moveActionData { get; private set; }
	public ActionData startedTurnActionData { get; private set; }
	public ActionData defaultAIActionData { get; private set; }

	public bool attackIsRecharget { get; set; } = true;
	
	public bool inAction { get; set; }
	
	public AbstractAction choosenAction { get; set; }
	
	public List<GroundCell> totalActionTargetList { get; private set; } = new List<GroundCell>();
	
	public List<GroundCell> areaOfAction { get; private set; } = new List<GroundCell>();
	public List<GroundCell> possibleTargetCells { get; private set; } = new List<GroundCell>();

	public List<GroundCell> moveActionTargetList { get; private set; } = new List<GroundCell>();
	public List<GroundCell> currentActionTargetList { get; private set; } = new List<GroundCell>();
	public List<GroundCell> road { get; private set; } = new List<GroundCell>();
	
	public List<CurrentEffect> activeEffectList { get; set; }
	
	public List<OnCellEffect> onCellEffectList { get; set; }
	
	public AIGoal aiGoal { get; private set; }

	public AudioClip GetActionAudioClip(AbstractAction action)
	{
		if (actionList.Contains(action) && _actionVoiceLine.Count != 0 && !TurnController.currentPlayerNotLocal)
			return _actionVoiceLine[actionList.IndexOf(action)];
		else
			return null;
	}

	public void AddActionToList(AbstractAction action)
	{
		if (!_actionList.Contains(action))
		{
			_actionList.Add(action);
			InitActionData();
		}
	}

	public void ChangeAction(AbstractAction oldAction, AbstractAction newAction, UnitActionType actionType)
	{
		if (actionType == UnitActionType.StartedTurnAction)
		{
			_startedTurnAction = newAction;
		}
		else if (actionType == UnitActionType.OnDeathAction)
		{
			_onDeathAction = newAction;
		}
		else
		{
			for (int i = 0; i < _actionList.Count; i++)
			{
				if (_actionList[i] == oldAction)
				{
					_actionList[i] = newAction;

					if (oldAction == _counterAttack)
						_counterAttack = newAction;

					if (oldAction == _moveAction)
						_moveAction = newAction as Move;
					
					break;
				}
			}
		}
		
		InitActionData();
	}

	public void InitActionData()
	{
		actionDataList = new List<ActionData>();

		foreach (AbstractAction action in _actionList)
			actionDataList.Add(action.GetActionData(this));
		
		if (_counterAttack != null)
			counterAttackActionData = _counterAttack.GetActionData(this);
		
		if (_moveAction != null)
			moveActionData = _moveAction.GetActionData(this);
		
		if (_startedTurnAction != null)
			startedTurnActionData = _startedTurnAction.GetActionData(this);

		if (_defaultAIAction != null)
			defaultAIActionData = _defaultAIAction.GetActionData(this);
	}

	public ActionData GetActionData(AbstractAction action)
	{
		ActionData actionData = null;

		for (int i = 0; i < _actionList.Count; i++)
		{
			if (_actionList[i] == action)
				actionData = actionDataList[i];
		}

		if (_counterAttack == action)
			actionData = counterAttackActionData;
		
		if (_startedTurnAction == action)
			actionData = startedTurnActionData;

		return actionData;
	}

	private void SetGoal()
	{
		if (player?.aiPlayer?.goals.Count > 0)
		{
			foreach (AIGoal goal in player.aiPlayer.goals)
			{
				if (goal.currentUnit.Name.Equals(Name))
				{
					aiGoal = goal;
					break;
				}
			}
		}
	}
	
	public override void StartTurn()
	{
		turnEnded = false;
		currentMovePoints = currentStats.maxMovePoints;
		int regen = currentStats.healthRegen;
		
		if (_haveHealingTerrain && position.terrainType == _healingTerrain)
			regen += GroundSettings.TERRAINREGEN;
		
		RestoreHealth(regen);
		
		if (currentStats.maxMana != 0)
			RestoreMana(currentStats.manaRegen);
		
		ActivateStartedTurnAction();
	}

	public void ActivateStartedTurnAction()
	{
		if (_startedTurnAction != null)
		{
			StartCoroutine(_startedTurnAction.MakeAction(this, position));
		}
	}
	
	public override void EndTurn()
	{
		turnEnded = true;
	}
	
	private void SetMoveDistance()
	{
		if (_moveAction != null)
			moveActionTargetList = _moveAction.GetDistance(this);
		
		else
			moveActionTargetList = new List<GroundCell>();
	}
	
	public void SetRoadTo(GroundCell goal)
	{
		road = moveAction.GetRoadToCell(this, goal);
	}
	
	public void SetMapEventMovingRoad(List<GroundCell> road)
	{
		this.road = new List<GroundCell>();

		foreach (GroundCell cell in road)
			this.road.Add(cell);
	}

	public void SetAreaOfActions()
	{
		areaOfAction = new List<GroundCell>();

		if (choosenAction != moveAction)
			areaOfAction = choosenAction.GetAreaOfAction(this);
	}

	private void SetAttackDistance()
	{
		currentActionTargetList = choosenAction.GetDistance(this);
	}
	
	public void SetRangeOfAction()
	{
		totalActionTargetList = new List<GroundCell>();
		moveActionTargetList = new List<GroundCell>();
		currentActionTargetList = new List<GroundCell>();
		
		if (choosenAction != moveAction)
			SetAttackDistance();
		else
			currentActionTargetList = new List<GroundCell>();
		
		if (choosenAction.needToRenderRoad || currentActionTargetList.Count == 0)
			SetMoveDistance();
		
		foreach(GroundCell cell in moveActionTargetList)
			totalActionTargetList.Add(cell);
		
		foreach(GroundCell cell in currentActionTargetList)
			totalActionTargetList.Add(cell);
	}
	
	public void RenderUnitRoad(GroundCell goal)
	{
		if (turnEnded || !isMovable || !choosenAction.needToRenderRoad || choosenAction.range == ActionRange.Ranged 
		&& (goal.onCellObject != null || choosenAction is IAreaAction || choosenAction.actionType == ActionType.OnDeadUnit))
		{
			return;
		}
		
		if (totalActionTargetList.Contains(goal) && goal != position)
		{
			SetRoadTo(goal);

			if (road.Count != 0)
			{
				foreach (GroundCell cell in road)
					cell.OnSelecter(ActionType.Moving, false);
			}
		}
	}
	
	protected override void LocalInit(GroundCell positionCell)
	{
		InitStats();
		
		activeEffectList = new List<CurrentEffect>();
		onCellEffectList = new List<OnCellEffect>();
		
		turnEnded = true;
		
		InitInfo(new UnitInfo());
		
		experience = GetComponent<Experience>();
		experience.Init();
		
		if (experience.maxLevel > 0)
			player.unitsWithExperience.Add(this);

		if (_auraData != null)
			_auraData.ActivateAura(this, positionCell);

		InitActionData();
		SetGoal();
	}
	
	protected override void LocalSelect()
	{
		UnitListUI.selectUnit.Invoke(this);

		if (!turnEnded && actionList.Count != 0 && !TurnController.currentPlayerNotLocal)
		{
			if (actionList[0].ChekActionForActive(this))
				ChooseAction(actionList[0]);
			
			else 
				ChooseAction(_moveAction);
		}
	}
	
	public void ChooseAction(AbstractAction action)
	{
		choosenAction = action;
		
		map.ClearMapCash();
		SetAreaOfActions();
		SetRangeOfAction();
		RenderActionDistance();
	}
	
	public void RenderActionDistance()
	{
		foreach (GroundCell cell in moveActionTargetList)
			cell.OnSelecter(ActionType.Moving, true);
		
		foreach (GroundCell cell in currentActionTargetList)
			cell.OnSelecter(choosenAction.actionType, false);

		foreach (GroundCell cell in areaOfAction)
			cell.OnPossibleTargetSelecter();
	}
	
	public override void LocalDeath()
	{
		if (!isMech && _goingToGraveAtDeath)
			position.AddUnitToGrave(this);
		
	//	if (_onDeathEffect != null)
	//		_onDeathEffect.Activate(this);
		
		if (_onDeathAction != null)
		{
			StartCoroutine(_onDeathAction.MakeAction(this, position));
		}

		Remove();
		
		while (activeEffectList.Count > 0)
		{
			activeEffectList[0].Clean();
		}
	}
	
	public void RemoveFromGame()
	{
		Remove();
		
		map.objectList.Remove(this);
		
		if (player != null)
			player.objectList.Remove(this);
		
		isDead = true;
		
		gameObject.SetActive(false);
	}
	
	private void Remove()
	{
		isDead = true;
		
		if (player != null)
			player.currentUnitLimit -= leadershipCost;
		
		if (aura != null)
			aura.CleanArea();
		
		currentHealth = 0;
		
		position.onCellObject = null;
		
		PlayerUI.refreshPlayerInfo.Invoke(TurnController.lastNotComputerPlayer);
	}
	
	public void Revive(Player revivingPlayer, GroundCell pos)
	{
		gameObject.SetActive(true);
		
		RestoreHealth(currentStats.maxHealth);
		
		map.objectList.Add(this);
		revivingPlayer.objectList.Add(this);
		player = revivingPlayer;
		team = revivingPlayer.team;
		_objectRenderer.SetColor();
		SetNewPosition(pos);
		
		player.currentUnitLimit += leadershipCost;
		
		isDead = false;
		EndTurn();
	}
	
	public void DotAllEffects(bool endedTurnDot)
	{
		DotAllActionEffects(endedTurnDot);
		
		if (!endedTurnDot)
			RefreshOnCellEffectList(true);
	}
	private void DotAllActionEffects(bool endedTurnDot)
	{
		for (int i = 0; i < activeEffectList.Count; i++)
		{
			CurrentEffect effect = activeEffectList[i];
			
			if (effect.actionEffect.dotAtEndOfTurn == endedTurnDot)
			{
				effect.TurnEffect();
				
				if (effect.isCleaned)
					i--;
			}
		}
	}
	
	public override void ActivateTurnEndedAction()
	{
		foreach (AbstractAction action in actionList)
		{
			if (action.autoActivateAtTurnEnded && action.ChekActionForActive(this))
			{
				choosenAction = action;
				SetRangeOfAction();

				if (currentActionTargetList.Count != 0)
				StartCoroutine(action.MakeAction(this, currentActionTargetList[0]));
				break;
			}
		}
	}
	
	public override void SetNewPosition(GroundCell newPosition)
	{
		if (aura != null)
			aura.CleanArea();
			
		if (position != null)	
			position.onCellObject = null;
		
		newPosition.onCellObject = this;
		position = newPosition;
		
		Vector3 pos = new Vector3(newPosition.transform.position.x, 
						newPosition.transform.position.y, 
						GroundSettings.OBJECTZPOSITION);
		
		transform.position = pos;
		
		RefreshOnCellEffectList(true);
		
		if (aura != null)
			aura.SetArea(position);
		
		PositionWasSetted.Invoke(position, this);
		IconSetter.setEffects.Invoke(this);
	}
	
	public void RefreshOnCellEffectList(bool startedTurnRefresh)
	{
		if (isDead)
			return;
		
		while (onCellEffectList.Count > 0)
			onCellEffectList[0].CleanObject(this);
		
		foreach (OnCellEffect effect in position.onCellEffectList)
		{
			if (startedTurnRefresh || !effect.refreshedOnStartTurnOnly)
				effect.SetEffectOnObject();
		}
	}

	public void PlayActionCastSound(AbstractAction action)
	{
		if (action != null && action.castSound != null)// && !objectRenderer.voiceSource.isPlaying)
			objectRenderer.PlayActionSound(action);
	}

	public override void RenderTurnState()
	{
		if (turnEnded)
			objectRenderer.turnStateRenderer.color = new Vector4(0,0,0,0);

		else if (currentMovePoints >= currentStats.maxMovePoints)
			objectRenderer.turnStateRenderer.color = player.color;
			
		else
			objectRenderer.turnStateRenderer.color = player.secondColor;
	}
}
