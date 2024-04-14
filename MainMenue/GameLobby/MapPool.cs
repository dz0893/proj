using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapPool : MonoBehaviour
{
	[SerializeField] private List<BattleMap> _mapList;
	public List<BattleMap> mapList => _mapList;

	[SerializeField] private MapCell _cellPref;
	[SerializeField] private Transform _container;
	
	private List<MapCell> cellList;
	
	public delegate void SelectMap(MapCell mapCell);
	public static SelectMap selectMap;
	
	public BattleMap selectedMap { get; private set; }
	
	private void Start()
	{
		selectMap = Select;
	}
	
	public void Init()
	{
		Clean();
		Render();
		Select(cellList[0]);
	}
	
	private void Render()
	{
		foreach (BattleMap map in _mapList)
		{
			MapCell cell = Instantiate(_cellPref, _container);
			cell.Init(map);
			cellList.Add(cell);
		}
	}
	
	private void Clean()
	{
		cellList = new List<MapCell>();
		
		foreach (Transform child in _container)
			Destroy(child.gameObject);
	}
	
	private void Select(MapCell mapCell)
	{
		foreach (MapCell cell in cellList)
			cell.Deselect();
		
		mapCell.Select();
		selectedMap = mapCell.map;
	}
}
