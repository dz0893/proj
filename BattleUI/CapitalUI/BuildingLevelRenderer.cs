using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingLevelRenderer : MonoBehaviour
{
    [SerializeField] private List<BuildingLevelCellUI> _levelCellList;

    public void Render(BuildingData buildingData, Player player)
    {
        Clean();
        Init(buildingData, player);
    }

    private void Init(BuildingData buildingData, Player player)
    {
        for (int i = 0; i < buildingData.building.maxLevel; i++)
            _levelCellList[i].Render(player, buildingData, i);
    }

    private void Clean()
    {
        foreach (BuildingLevelCellUI cell in _levelCellList)
            cell.gameObject.SetActive(false);
    }
}
