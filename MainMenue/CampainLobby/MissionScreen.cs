using UnityEngine;
using UnityEngine.UI;

public class MissionScreen : MonoBehaviour
{
    [SerializeField] private CampainLobby _campainLobby;

    [SerializeField] private Text _mapName;
	[SerializeField] private Text _mapDescription;
	[SerializeField] private Image _mapIcon;

    [SerializeField] private GameObject _startGameButton;
    [SerializeField] private GameObject _nextMissionButton;

    public void RenderPrologue(Scenario scenario)
    {
        _mapName.text = scenario.Name;
        _mapDescription.text = scenario.prologue;
        _mapIcon.sprite = scenario.prologueIcon;

        _startGameButton.SetActive(true);
        _nextMissionButton.SetActive(false);
    }

    public void RenderEpilogue(Scenario scenario)
    {
        _mapName.text = "";
        _mapDescription.text = scenario.epilogue;
        _mapIcon.sprite = scenario.epilogueIcon;

        _startGameButton.SetActive(false);

        if (_campainLobby.totalMapList.Count > _campainLobby.totalMapList.IndexOf(_campainLobby.currentMap))
            _nextMissionButton.SetActive(true);
        else
            _nextMissionButton.SetActive(false);
    }
}
