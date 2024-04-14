using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCameraField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Direction _direction;
	public Direction direction => _direction;
	
	[SerializeField] private CameraController _cameraController;
	
	public bool movingEnabled { get; private set; }
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		
		movingEnabled = true;
		
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		movingEnabled = false;
	}
}
