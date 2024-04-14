using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private List<MoveCameraField> _movingFields;
	
	private BattleMap map;
	
	private Vector3 movingVector;
	
	private bool cameraEnabled;
	
	[SerializeField] private float _speed;
	
	[SerializeField] private Transform _mapContainer;

	private float maxScale = 2f;
	private float minScale = 0.5f;

	private bool isUnitFollowed;
	private bool mouseMovingEnabled;
	private bool keyboardMovingEnabled;
	private bool movingEnabled;
	
	private float followingUnitTime;
	
	public delegate void SetCameraPosition(Vector3 position);
	public delegate void InitCamera();
	public delegate void OffCameraController();
	
	public static SetCameraPosition setCameraPosition;
	public static InitCamera init;
	public static OffCameraController offCameraController;
	
	private Vector3 mousePosition;

	private void Start()
	{
		init = Init;
		offCameraController = DeasableCamera;
	}
	
	private void Init()
	{
		setCameraPosition = SetPosition;
		map = BattleMap.instance;
		cameraEnabled = true;
	}

	private void DeasableCamera()
	{
		cameraEnabled = false;
	}
	
	private void FixedUpdate()
	{
		//MouseMiddleButtonCameraController();
		if (!cameraEnabled)
			return;
		
		CheckDirectionAndSetSpeed();
		
		if (movingEnabled && !isUnitFollowed && !PlayerUI.inputIsLocked)
			transform.position += movingVector;
		
		// menu controller
		if (Input.GetKeyDown(KeyCode.Escape))
			MatchMenue.switchMenuState.Invoke();
	}
	
	public void ScaleUpMapContainer()
	{
		if (_mapContainer.localScale.x < maxScale)
		{
			Vector3 newScale = new Vector3(_mapContainer.localScale.x + 0.1f, _mapContainer.localScale.y + 0.1f, _mapContainer.localScale.z);
			_mapContainer.localScale = newScale;

			GameOptions.saveNewMapScale.Invoke();
		}
	}
	public void ScaleDownMapContainer()
	{
		if (_mapContainer.localScale.x > minScale)
		{
			Vector3 newScale = new Vector3(_mapContainer.localScale.x - 0.1f, _mapContainer.localScale.y - 0.1f, _mapContainer.localScale.z);
			_mapContainer.localScale = newScale;

			GameOptions.saveNewMapScale.Invoke();
		}
	}
	/*private void MouseMiddleButtonInput()
	{
		if (Input.GetMouseButtonDown(2))
		{
			mousePosition = Input.mousePosition;
		}
		if (Input.GetMouseButton(2))
		{
			Vector3 pos = mousePosition - Input.mousePosition;
			
			if (pos.x > 0)
			{
				movingVector.x = -0.0001f * pos.x;
				movingEnabled = true;
			}
			else if (pos.x < 0)
			{
				movingVector.x = 0.0001f * pos.x;
				movingEnabled = true;
			}
			else
				movingVector.x = 0;
				
			if (pos.y > 0)
			{
				movingVector.y = -0.0001f * pos.y;
				movingEnabled = true;
			}
			else if (pos.y < 0)
			{
				movingVector.y = 0.0001f * pos.y;
				movingEnabled = true;
			}
			else
				movingVector.x = 0;
			
		}
	}*/
	
	private void SetPosition(Vector3 position)
	{
		transform.position = new Vector3(position.x, position.y, transform.position.z);
	}
	
	private void CheckDirectionAndSetSpeed()
	{
		if (!mouseMovingEnabled)
			KeyboardInput();
		
		if (!keyboardMovingEnabled)
			MousePositionInput();
		
		SetMovingState();
		
	//	if (!movingEnabled)
	//		MouseMiddleButtonInput();
	}
	
	private void SetMovingState()
	{
		if (keyboardMovingEnabled || mouseMovingEnabled)
			movingEnabled = true;
		else
			movingEnabled = false;
	}
	
	private void MousePositionInput()
	{
		mouseMovingEnabled = false;
		
		foreach (MoveCameraField field in _movingFields)
		{
			if (field.movingEnabled && !CameraOnBoard(field.direction))
			{
				SetMovingVector(field.direction);
				mouseMovingEnabled = true;
				break;
			}
		}
	}
	
	private void KeyboardInput()
	{
		keyboardMovingEnabled = false;
		
		if (Input.GetKey(KeyCode.W) && map.topMapBoard.transform.position.y >= transform.position.y)
		{
			movingVector.y = _speed;
			keyboardMovingEnabled = true;
		}
		else if (Input.GetKey(KeyCode.S) && map.bottomMapBoard.transform.position.y <= transform.position.y)
		{
			movingVector.y = -_speed;
			keyboardMovingEnabled = true;
		}
		
		else
			movingVector.y = 0;
		
		if (Input.GetKey(KeyCode.A) && map.leftMapBoard.transform.position.x <= transform.position.x)
		{
			movingVector.x = -_speed;
			keyboardMovingEnabled = true;
		}
		else if (Input.GetKey(KeyCode.D) && map.rightMapBoard.transform.position.x >= transform.position.x)
		{
			movingVector.x = _speed;
			keyboardMovingEnabled = true;
		}
		
		else
			movingVector.x = 0;
	}
	
	public void SetMovingVector(Direction direction)
	{
		switch (direction)
		{
			case Direction.Up:
				movingVector = new Vector3(0,_speed,0);
				break;
			case Direction.Down:
				movingVector = new Vector3(0,-_speed,0);
				break;
			case Direction.Left:
				movingVector = new Vector3(-_speed,0,0);
				break;
			case Direction.Right:
				movingVector = new Vector3(_speed,0,0);
				break;
		}
	}
	
	public bool CameraOnBoard(Direction direction)
	{
		bool CameraOnBoard = false;
		
		switch (direction)
		{
			case Direction.Up:
			{
				if (map.topMapBoard.transform.position.y <= transform.position.y)
					CameraOnBoard = true;
				break;
			}
			case Direction.Down:
			{
				if (map.bottomMapBoard.transform.position.y >= transform.position.y)
					CameraOnBoard = true;
				break;
			}
			case Direction.Left:
			{
				if (map.leftMapBoard.transform.position.x >= transform.position.x)
					CameraOnBoard = true;
				break;
			}
			case Direction.Right:
			{
				if (map.rightMapBoard.transform.position.x <= transform.position.x)
					CameraOnBoard = true;
				break;
			}
		}
		
		return CameraOnBoard;
	}
}
