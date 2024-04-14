using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReviveHeroCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _reviveHeroButton;
	[SerializeField] private Text _nameText;
	[SerializeField] private Text _reviveHeroGoldCostText;
	[SerializeField] private Text _reviveHeroOreCostText;
	[SerializeField] private Text _reviveHeroLimitText;
	[SerializeField] private Image _heroIcon;

    private UnitInfo unitInfo = new UnitInfo();

    private Unit hero;

	public void Render(Player player, RecruitPoint recruitPoint)
	{
        hero = player.hero;

		if (hero != null)
		{
            unitInfo.Init(hero);
			_heroIcon.sprite = hero.icon;
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(false);
			return;
		}

		if (RecruitPointUI.CheckForReviveHero(recruitPoint))
		{
			_reviveHeroButton.interactable = true;
		}
		else
			_reviveHeroButton.interactable = false;
		
		if (recruitPoint.initer == player.capital)
			gameObject.SetActive(true);
		else
			gameObject.SetActive(false);
		
		_nameText.text = hero.Name;
		_reviveHeroGoldCostText.text = player.reviveHeroGoldCost.ToString();
		_reviveHeroOreCostText.text = player.reviveHeroOreCost.ToString();
		_reviveHeroLimitText.text = hero.leadershipCost.ToString();
	}

    public void OnPointerEnter(PointerEventData eventData)
	{
		ObjectInfoUI.writeInfo.Invoke(unitInfo);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		ObjectInfoUI.cleanInfo.Invoke();
	}
}
