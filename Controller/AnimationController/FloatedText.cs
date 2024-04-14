using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatedText : MonoBehaviour
{
	[SerializeField] private Text _text;
	
	[SerializeField] private float _dy;
	[SerializeField] private float _dx;
	
	[SerializeField] private float _stringOffset;
	
	private static List<Vector3> positionList = new List<Vector3>();
	
	public void Play(Vector3 position, string text, Color color)
	{
		_text.text = text;
		_text.color = color;
		
		StartCoroutine(PlayLiveTime(position, color));
	}
	
	private float GetDXByColor(Color color)
	{
		if (color == Color.blue)
			return _dx;
		else if (color == Color.green)
			return -_dx;
		else
			return 0;
	}
	
	private IEnumerator PlayLiveTime(Vector3 position, Color color)
	{
		positionList.Add(position);
		float progress = 0;
		
		Vector3 startedPosition = position;
		startedPosition.x = startedPosition.x + GetDXByColor(color);
		
		startedPosition.y = position.y + GetPosCounter(position) * _stringOffset;
		
		Vector3 moveProgress = startedPosition;
		
		while (progress < 1)
		{
			progress += Time.deltaTime / ActionSettings.FLOATEDTEXTLIVETIME;
			
			moveProgress.y = startedPosition.y + _dy * progress;
			
			transform.position = moveProgress;
			yield return null;
		}
		
		positionList.Remove(position);
		Destroy(this.gameObject);
	}
	
	private int GetPosCounter(Vector3 position)
	{
		int counter = 0;
		
		foreach (Vector3 pos in positionList)
		{
			if (position == pos)
				counter++;
		}
		
		return counter;
	}
}
