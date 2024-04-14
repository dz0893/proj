using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
	[SerializeField] private List<BattleMap> _mapList;
	[SerializeField] private List<NullObject> _objectList;
	[SerializeField] private List<ActionEffect> _actionEffectList;
	
	public static DataBase instance { get; private set; }
	
	private void Start()
	{
		instance = this;
	}
	
	public BattleMap GetMap(string name)
	{
		foreach (BattleMap map in _mapList)
		{
			if (name.Equals(map.Name))
				return map;
		}
		Debug.Log("NoMap " + name);
		return null;
	}
	
	public NullObject GetObject(int index)
	{
		foreach (NullObject obj in _objectList)
		{
			if (index == obj.index)
				return obj;
		}
		Debug.Log("NoObj " + index);
		return null;
	}
	
	public ActionEffect GetActionEffect(int index)
	{
		foreach (ActionEffect effect in _actionEffectList)
		{
			if (index == effect.index)
				return effect;
		}
		Debug.Log("NoEffect " + index);
		return null;
	}
}
