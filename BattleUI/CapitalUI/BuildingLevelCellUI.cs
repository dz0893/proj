using UnityEngine;
using UnityEngine.UI;

public class BuildingLevelCellUI : MonoBehaviour
{

    [SerializeField] private Sprite _closedLevelIcon;
    [SerializeField] private Sprite _cantBuildIcon;
    [SerializeField] private Sprite _buildedLevelIcon;
    [SerializeField] private Sprite _blockedLevelIcon;

    [SerializeField] private Image _icon;
    [SerializeField] private Text _levelText;

    [SerializeField] private Image _blockIcon;

    public void Render(Player player, BuildingData buildingData, int currentCellIndex)
    {
        gameObject.SetActive(true);
        _levelText.text = (currentCellIndex + 1).ToString();
        
        if (buildingData.isBlocked && currentCellIndex >= buildingData.levelOfBlocking)
        {
            _blockIcon.sprite = _blockedLevelIcon;
            _blockIcon.gameObject.SetActive(true);
        }
        else if (buildingData.currentLevel < currentCellIndex)
        {
            _blockIcon.sprite = _closedLevelIcon;
            _blockIcon.gameObject.SetActive(true);
        }
        else if (buildingData.currentLevel > currentCellIndex)
        {
            _blockIcon.sprite = _buildedLevelIcon;
            _blockIcon.gameObject.SetActive(true);
        }
        else if (player.capital.turnEnded || player.gold < buildingData.upgradeGoldCost || player.ore < buildingData.upgradeOreCost)
        {
            _blockIcon.sprite = _cantBuildIcon;
            _blockIcon.gameObject.SetActive(true);
        }
        else
        {
            _blockIcon.gameObject.SetActive(false);
        }
    }
}
