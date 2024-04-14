using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFinder
{
	public NullObject GetObjectWithPosition(Vector3 position, List<NullObject> objectList)
	{
		NullObject obj = null;
		
		foreach (NullObject currentObject in objectList)
		{
			if (currentObject.transform.position == position)
			{
				obj = currentObject;
				break;
			}
		}
		
		return obj;
	}
	
	public GroundCell GetCellWithPosition(Vector3 position, List<GroundCell> cellList)
	{
		GroundCell cell = null;
		
		foreach (GroundCell currentCell in cellList)
		{
			if (currentCell.transform.position == position)
			{
				cell = currentCell;
				break;
			}
		}
		
		return cell;
	}
}
