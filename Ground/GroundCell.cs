using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GroundCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler// IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public List<OnCellEffect> onCellEffectList { get; set; } = new List<OnCellEffect>();
	
	public OnCellEffectData terrainEffect => CellEffectFactory.instance.GetCurrentTerrainTypeEffect(terrainType);
	
	[SerializeField] private ActionTargetCell _targetCell;
	public ActionTargetCell targetCell => _targetCell;
	
	[SerializeField] private int _index;
	public int index => _index;
	
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private SpriteRenderer _selecter;
	
	[SerializeField] private bool _isRandomCell;
	public bool isRandomCell => _isRandomCell;
	
	[SerializeField] private bool _isDepositCell;
	public bool isDepositCell => _isDepositCell;
	
	[SerializeField] private List<TerrainType> _terrainTypeList;
	public List<TerrainType> terrainTypeList => _terrainTypeList;
	
	[SerializeField] private TerrainType _defaultTerrainType;
	public TerrainType defaultTerrainType => _defaultTerrainType;
	
	[SerializeField] private int _basicOreValue;
	public int currentOreValue { get; set; }
	
	public TerrainType terrainType { get; private set; }
	
	public string Name { get; private set; }
	public int movingCost { get; private set; }
	public MovingType movingType { get; private set; }
	public bool canBeTerraformated { get; private set; }
	
	public MaterialObject onCellObject { get; set; }
	public UnmaterialObject unmaterialOnCellObject { get; set; }
	
	public List<Unit> grave { get; private set; } = new List<Unit>();
	
	public GroundCell previousCell { get; set; }
	
	public Vector3Int cellPosition { get; private set; }
	
	public int totalMovingCost { get; set; }
	
	protected Tilemap tilemap;
	
	public List<GroundCell> closestCellList { get; private set; } = new List<GroundCell>();
	
	private GroundInfo info = new GroundInfo();
	
	public bool isNegativeEffected { get; private set; }

	private static bool mouseIsBloked;
	private float mouseClickDelay = 0.1f;
	
	public static UnityEvent<TerrainType, TerrainType> TerrainTypeWasChanged = new UnityEvent<TerrainType, TerrainType>();
	
	public void OnPointerClick (PointerEventData eventData)
	{
		if (!mouseIsBloked)
			StartCoroutine(Click(eventData));
	}

	private IEnumerator Click(PointerEventData eventData)
	{
		if (PlayerUI.inputIsLocked)
			yield break;

		mouseIsBloked = true;

		if (eventData.button == PointerEventData.InputButton.Left && TurnController.currentPlayer.aiPlayer == null)
		{
			if (MapController.selectedGlobalAction != null && MapController.selectedGlobalAction.areaList.Contains(this))
			{
				MapController.global.Invoke(this);
			}
			else if (MapController.selectedUnit == null)
			{
				MapController.select.Invoke(this);

				if (onCellObject != null)
					onCellObject.PlayIdleVoiceLine();
			}	
			else if (MapController.selectedUnit.totalActionTargetList.Contains(this) && !MapController.selectedUnit.turnEnded)
			{
				MapController.action.Invoke(this);
			}
			else if (MapController.selectedObjectCell == this)
			{
				CameraController.setCameraPosition.Invoke(transform.position);
				
				if (onCellObject != null)
					onCellObject.PlayIdleVoiceLineWithIncrementingIndex();
			}
			else if (onCellObject != null || unmaterialOnCellObject != null)
			{
				MapController.select.Invoke(this);
				
				if (onCellObject != null)
					onCellObject.PlayIdleVoiceLine();
			}
		}
		
		else if (eventData.button == PointerEventData.InputButton.Right)
			MapController.clear.Invoke();

		yield return new WaitForSeconds(mouseClickDelay);
		mouseIsBloked = false;
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!MapController.controllerIsBlocked)
			BattleMap.renderRoad.Invoke(this);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		if (!MapController.controllerIsBlocked)
			BattleMap.cleanRoad.Invoke();
	}
	
	public void OnPointerDown (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			ObjectInfoUI.writeInfo.Invoke(GetObjectInfo());
		}
	}
	
	public void OnPointerUp (PointerEventData eventData)
	{
		ObjectInfoUI.cleanInfo.Invoke();
	}
	
	private ObjectInfo GetObjectInfo()
	{
		if (onCellObject != null)
			return onCellObject.info;
		else if (unmaterialOnCellObject != null)
			return unmaterialOnCellObject.info;
		else
			return info;
	}
	
	public void Init()
	{
		tilemap = BattleMap.instance.groundMap;
		
		CleanGrave();
		
		cellPosition = tilemap.WorldToCell(transform.position);
	}
	
	public void SetTerrainType(IGround ground)
	{
		TerrainType oldTerrainType = terrainType;
		
		InitTerrainType(ground);
		SetOreValue();
		
		if (unmaterialOnCellObject != null && unmaterialOnCellObject.destroedByTerramorfing)
			unmaterialOnCellObject.Death();
		
		TerrainTypeWasChanged.Invoke(oldTerrainType, terrainType);
	}
	
	private void SetOreValue()
	{
		if (terrainType == TerrainType.GoldDeposit)
		{
			currentOreValue = GroundSettings.maxGoldValue;
		}
		else if (terrainType == TerrainType.OreDeposit)
		{
			currentOreValue = GroundSettings.maxOreValue;
		}
		if (_basicOreValue != 0)
		{
			currentOreValue = _basicOreValue;
		}
	}
	
	public void InitTerrainType(IGround ground)
	{
		Name = ground.Name;
		movingCost = ground.movingCost;
		movingType = ground.movingType;
		terrainType = ground.terrainType;
		isNegativeEffected = ground.isNegativeEffected;
		canBeTerraformated = ground.canBeTerraformated;
		
		CleanGrave();
		
		info.Init(this);
		
		CellEffectFactory.instance.CleanAllDefaultEffects(this);
		
		if (terrainEffect != null)
			terrainEffect.Set(this);
		
		_spriteRenderer.sprite = RandomGroundCellGetter.instance.GetTerrainSprite(terrainType);

		if (ground.terrainType != TerrainType.Water)
			_spriteRenderer.flipX = RandomGroundCellGetter.instance.GetFlipState();
		else
			_spriteRenderer.flipX = false;
	}

	public void AddUnitToGrave(Unit unit)
	{
		grave.Add(unit);
		RenderGrave();
	}

	public void RemoveUnitFromGrave(Unit unit)
	{
		if (grave.Contains(unit))
			grave.Remove(unit);
		
		RenderGrave();
	}
	
	public void CleanGrave()
	{
		grave = new List<Unit>();
		RenderGrave();
	}

	private void RenderGrave()
	{
		if (grave.Count == 0)
		{
			targetCell.OffGraveIcon();
		}
		else
		{
			targetCell.OnGraveIcon();
		}
	}

	public void ClearCurrentEffect(OnCellEffectData effectData, int team)
	{
		foreach (OnCellEffect effect in onCellEffectList)
		{
			if (effect.data == effectData && effect.team == team)
			{
				effect.Clean();
				break;
			}
		}
		
		if (onCellObject != null && onCellObject is Unit)
		{
			Unit unit = onCellObject as Unit;
			unit.RefreshOnCellEffectList(false);
		}
	}
	
	public void CLearCellCash()
	{
		previousCell = null;
		totalMovingCost = 0;
		
		OffSelecter();
		OffPossibleTargetSelecter();
	}
	
	public void SetPreviousCell(GroundCell cell)
	{
		if (closestCellList.Contains(cell))
			previousCell = cell;
		else
			Debug.Log("ALARM!!! WRONG CELL!");
	}
	
	public void AddCellToClosestCellList(GroundCell groundCell)
	{
		if (!closestCellList.Contains(groundCell))
			closestCellList.Add(groundCell);
	}
	
	public void OffSelecter()
	{
		_selecter.gameObject.SetActive(false);
	}
	
	public void OnSelecter(ActionType actionType, bool isMoveDistanceSelecter)
	{
		_selecter.gameObject.SetActive(true);
		
		if (isMoveDistanceSelecter)
			_selecter.color = new Vector4(1,1,1,0.3f);
			
		else if (actionType == ActionType.Moving)
			_selecter.color = new Vector4(0,0,1,0.3f);
		
		else if (actionType == ActionType.Offensive)
			_selecter.color = new Vector4(1,0,0,0.3f);
		
		else
			_selecter.color = new Vector4(0,1,0,0.3f);
	}

	public void OffPossibleTargetSelecter()
	{
		targetCell.possibleTargetSelecter.gameObject.SetActive(false);
	}

	public void OnPossibleTargetSelecter()
	{
		targetCell.possibleTargetSelecter.gameObject.SetActive(true);
	}
}
