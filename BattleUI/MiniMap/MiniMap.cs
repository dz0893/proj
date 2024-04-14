using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
	[SerializeField] private GameObject _back;
	[SerializeField] private OnMapGroundCell _cellPref;
	[SerializeField] private GameObject _targetMarkerPref;
	[SerializeField] private Transform _container;
	[SerializeField] private Transform _targetMarkerContainer;
	
	[SerializeField] private Transform _topLeftAnchor;
	[SerializeField] private Transform _botRightAnchor;
	
	private OnMapGroundCell topPosition;
	private OnMapGroundCell bottomPosition;
	private OnMapGroundCell leftPosition;
	private OnMapGroundCell rightPosition;
	
	private List<OnMapGroundCell> cellList = new List<OnMapGroundCell>();
	private List<GameObject> markersList = new List<GameObject>();
	
	public delegate void InitMap(BattleMap map);
	public delegate void RenderMap();
	public delegate void CleanMiniMap();
	
	public static InitMap init;
	public static RenderMap render;
	public static CleanMiniMap cleanMiniMap;
	
	private void Awake()
	{
		init = Init;
		render = Render;
		cleanMiniMap = Clean;
	}
	
	private void Init(BattleMap map)
	{
		Clean();
		CreateCells(map);
		SetMapTransform();

		TeamsRendererUI.renderTeams.Invoke();
	}

	public void SetTargets(BattleMap map)
	{

	}
	
	private void Clean()
	{
		cellList = new List<OnMapGroundCell>();
		_container.localScale = new Vector3(1,1,1);

		foreach(Transform child in _container)
			Destroy(child.gameObject);
	}
	
	private void CreateCells(BattleMap map)
	{
		foreach (GroundCell cell in map.mapCellList)
		{
			OnMapGroundCell onMapCell = Instantiate(_cellPref, _container);
			onMapCell.Init(cell);
			cellList.Add(onMapCell);
		}
	}
	
	public void Render()
	{
		foreach (OnMapGroundCell cell in cellList)
			cell.Render();
			
		RenderTargetMarkers(GetTargets(TurnController.lastNotComputerPlayer));

		FixFucktardetBug();

		BattleMap.instance.RenderObjectsState();
	}
	
	private void FixFucktardetBug()
	{
		foreach (NullObject obj in BattleMap.instance.objectList)
		{
			if (obj is Unit && !obj.isDead)
				obj.SetNewPosition(obj.position);
		}
	}
	
	private void SetMapTransform()
	{
		SetBoards();
		CentraiteMap();
		SetScale();
	}
	
	private void SetBoards()
	{
		topPosition = cellList[0];
		bottomPosition = cellList[0];
		leftPosition = cellList[0];
		rightPosition = cellList[0];
		
		foreach (OnMapGroundCell cell in cellList)
		{
			if (cell.transform.position.y > topPosition.transform.position.y)
				topPosition = cell;
			if (cell.transform.position.y < bottomPosition.transform.position.y)
				bottomPosition = cell;
			if (cell.transform.position.x < leftPosition.transform.position.x)
				leftPosition = cell;
			if (cell.transform.position.x > rightPosition.transform.position.x)
				rightPosition = cell;
		}
	}
	
	private void CentraiteMap()
	{
		float deltaX = (rightPosition.cell.transform.position.x + leftPosition.cell.transform.position.x) / 2;
		float deltaY = (topPosition.cell.transform.position.y + bottomPosition.cell.transform.position.y) / 2;
		
		foreach (OnMapGroundCell cell in cellList)
		{
			cell.transform.localPosition = new Vector3(cell.transform.localPosition.x - deltaX,
								cell.transform.localPosition.y - deltaY,
								cell.transform.localPosition.z);
		}
	}
	
	private void SetScale()
	{
		_container.localScale = new Vector3(1,1,1);
		
		float scaleY = _botRightAnchor.localPosition.y / bottomPosition.transform.localPosition.y;
		float scaleX = _topLeftAnchor.localPosition.x / leftPosition.transform.localPosition.x;
		
		if (scaleX > scaleY)
			_container.localScale = new Vector3(_container.localScale.x * scaleY, _container.localScale.y * scaleY, 1);
		
		else
			_container.localScale = new Vector3(_container.localScale.x * scaleX, _container.localScale.y * scaleX, 1);
		
		_targetMarkerContainer.localScale = _container.localScale;
	}
	
	public void PushOnOffButton()
	{
		_back.SetActive(!_back.activeSelf);
	}

	private List<GroundCell> GetTargets(Player player)
	{
		List<GroundCell> targetCellList = new List<GroundCell>();

		foreach (Mission mission in player.missionList)
		{
			foreach (GroundCell cell in mission.GetTargetList())
				targetCellList.Add(cell);
		}

		return targetCellList;
	}

	private void RenderTargetMarkers(List<GroundCell> targetCellList)
	{
		foreach (GameObject marker in markersList)
			Destroy(marker);

		foreach (OnMapGroundCell cell in cellList)
		{
			if (targetCellList.Contains(cell.cell))
			{
				InitMarker(cell);
			}
		}
	}

	private void InitMarker(OnMapGroundCell cell)
	{
		GameObject marker = Instantiate(_targetMarkerPref, _targetMarkerContainer);

		markersList.Add(marker);
		marker.transform.localPosition = cell.transform.localPosition;
	//	marker.transform.localScale = cell.transform.localScale;
	}
}
