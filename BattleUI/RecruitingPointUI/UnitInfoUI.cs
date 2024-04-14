using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    [SerializeField] private Image _idleSprite;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _manaText;
    [SerializeField] private Text _movePointsText;
    [SerializeField] private Text _limitText;
    [SerializeField] private Text _terrainKnowingText;
    [SerializeField] private Text _terrainHealingText;

    [SerializeField] private Text _physicalDefenceText;
    [SerializeField] private Text _piercingDefenceText;
    [SerializeField] private Text _magicalDefenceText;
    [SerializeField] private Text _siegeDefenceText;

    [SerializeField] private Text _descriptionText;

    [SerializeField] private Image _unitBackground;
    [SerializeField] private Image _defenceBackground;

    [SerializeField] private UnitActionListUI _skillList;
    
    [SerializeField] private Button _scrollLeftButton;
    [SerializeField] private Button _scrollRightButton;

    private int unitIndex;

    private void SetUnitIndex(Unit unit, RecruitPoint recruitPoint)
    {
        unitIndex = 0;

        if (unit == recruitPoint.player.hero)
        {
            unitIndex = recruitPoint.unitDataList.Count;
            return;
        }
        
        else
        {
            for (int i = 0; i < recruitPoint.unitDataList.Count; i++)
            {
                if (recruitPoint.unitDataList[i].unit.Name.Equals(unit.Name))
                {
                    unitIndex = i;
                    return;
                }
            }
        }
    }



    private void RenderScrollButtons(Unit unit, RecruitPoint recruitPoint)
    {
        _scrollLeftButton.interactable = false;
        _scrollRightButton.interactable = false;

        SetUnitIndex(unit, recruitPoint);

        if (unitIndex > 0)
        {
            _scrollLeftButton.interactable = true;
        }

        if (recruitPoint.initer != null && recruitPoint.initer == recruitPoint.player.capital)
        {
            if (unitIndex < recruitPoint.unitDataList.Count)
            {
                _scrollRightButton.interactable = true;
            }
        }
        else
        {
            if (unitIndex < recruitPoint.unitDataList.Count - 1)
            {
                _scrollRightButton.interactable = true;
            }
        }
    }

    public void PressLeftScrollButton()
    {
        unitIndex--;
        RecruitPointUI.openUnitInfoWithIndex(unitIndex);
    }

    public void PressRightScrollButton()
    {
        unitIndex++;
        RecruitPointUI.openUnitInfoWithIndex(unitIndex);
    }

    public void Render(Unit unit, Player player, RecruitPoint recruitPoint)
    {
        RenderScrollButtons(unit, recruitPoint);

        Vector4 color = UISettings.GetColor(player.race);
        UnitStats stats = unit.GetBasicStats();
        _idleSprite.sprite = unit.idleSprite;
        _nameText.text = unit.Name;
        _healthText.text = stats.maxHealth.ToString();
        _manaText.text = stats.maxMana.ToString();
        _movePointsText.text = stats.maxMovePoints.ToString();
        _limitText.text = unit.leadershipCost.ToString();

        _physicalDefenceText.text = stats.physicalDefence.ToString();
        _piercingDefenceText.text = stats.piercingDefence.ToString();
        _magicalDefenceText.text = stats.magicDefence.ToString();
        _siegeDefenceText.text = stats.siegeDefence.ToString();

        if (unit.description.Equals(""))
            _descriptionText.text = UISettings.DefaultUnitDescription;
        else
            _descriptionText.text = unit.description;
        
        _unitBackground.color = color;
        _defenceBackground.color = color;

        TerrainTextRender(unit);

        _skillList.Render(unit);
    }

    private void TerrainTextRender(Unit unit)
    {
        if (unit.haveHealingTerrain)
        {
            _terrainHealingText.gameObject.SetActive(true);
            _terrainHealingText.text = UISettings.TerrainHealing + GroundSettings.GetTerrainName(unit.healingTerrain);
        }
        else
        {
            _terrainHealingText.gameObject.SetActive(false);
        }

        if (unit.terrainKnowingList.Count > 0)
        {
            _terrainKnowingText.gameObject.SetActive(true);
            _terrainKnowingText.text = UISettings.TerrainKnowing;

            foreach (TerrainType terrainType in unit.terrainKnowingList)
            {
                _terrainKnowingText.text += GroundSettings.GetTerrainName(terrainType);

                if (unit.terrainKnowingList.IndexOf(terrainType) != unit.terrainKnowingList.Count - 1)
                    _terrainKnowingText.text += ", ";
            }
        }
        else
        {
            _terrainKnowingText.gameObject.SetActive(false);
        }
    }
}
