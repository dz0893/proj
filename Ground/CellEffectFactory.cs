using System.Collections.Generic;
using UnityEngine;

public class CellEffectFactory : MonoBehaviour
{
	public static CellEffectFactory instance { get; private set; }
	
	[SerializeField] private List<OnCellEffectData> _defaultEffectList;
	[SerializeField] private List<TerrainType> _terrainTypeList;
	
	private void Start()
	{
		instance = this;
	}
	
	public OnCellEffectData GetCurrentTerrainTypeEffect(TerrainType terrainType)
	{
		return _defaultEffectList[(int)terrainType];
	}
	
	public void CleanAllDefaultEffects(GroundCell cell)
	{
		for (int i = 0; i < cell.onCellEffectList.Count; i++)
		{
			if (_defaultEffectList.Contains(cell.onCellEffectList[i].data))
			{
				cell.onCellEffectList[i].Clean();
				i--;
			}
		}
	}
}
