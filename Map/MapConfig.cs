using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConfig : MonoBehaviour
{
	/*[ContextMenu("scale map")]
	public void Scale()
	{
		foreach (Transform child in transform)
		{
			child.position = new Vector3(child.position.x * 1.4f, child.position.y * 1.4f, child.position.z);
		}
	}*/

	[ContextMenu("Cutting")]
	public void Cut()
	{
		/*
		foreach (Transform child in transform)
		{
			if (child.position.x >= 0 && child.position.x < 2)
				DestroyImmediate(child.gameObject);
			//	child.position = new Vector3(child.position.x, child.position.y - 6.3f, child.position.z);
		}
		*/
		foreach (Transform child in transform)
		{
			if (child.position.x < 2)
				child.position = new Vector3(child.position.x + 1.218f, child.position.y, child.position.z);
		}
		
	}
}
