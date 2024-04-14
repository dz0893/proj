using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeListUI : MonoBehaviour
{
    [SerializeField] private Transform _container;
	[SerializeField] private List<UpgradeCell> _upgradeCellList;
	public List<UpgradeCell> upgradeCellList => _upgradeCellList;

    public void Render(Capital capital)
	{
		Clean();
		Init(capital);
	}
	
	private void Clean()
	{
		foreach (UpgradeCell cell in _upgradeCellList)
			cell.gameObject.SetActive(false);
	}
	
	private void Init(Capital capital)
	{
		for (int i = 0; i < capital.upgradeList.Count; i++)
		{
			_upgradeCellList[i].gameObject.SetActive(true);
			_upgradeCellList[i].Render(capital, capital.upgradeList[i]);
		}
	}
}
