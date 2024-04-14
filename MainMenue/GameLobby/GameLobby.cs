using UnityEngine;

public class GameLobby : MonoBehaviour
{
	[SerializeField] private PlayerList _playerList;
	[SerializeField] private MapPool _mapPool;
	
	[SerializeField] private Transform _background;
	[SerializeField] private Transform _mapContainer;
	public Transform mapContainer => _mapContainer;
	
	private bool simmetricalMap = true;
	
	public delegate void Render();
	public static Render render;

	private void Start()
	{
		render = RenderPlayerList;
	}
	public void Open()
	{
		_background.gameObject.SetActive(true);

		_mapPool.Init();
		RenderPlayerList();
	}

	public void Close()
	{
		_background.gameObject.SetActive(false);
	}

	private void RenderPlayerList()
	{
		_playerList.Render(_mapPool.selectedMap.countOfPlayers);
	}
	
	public void SwitchMapSimmetrical()
	{
		simmetricalMap = !simmetricalMap;
	}
	
	public void StartGame()
	{
		_playerList.InitPlayers();
		
		BattleMap map = Instantiate(_mapPool.selectedMap, _mapContainer);
		map.Init(_playerList.playerList, simmetricalMap);
		map.StartMatch();
		
		_background.gameObject.SetActive(false);
	}
}
