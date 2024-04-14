using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TeamsRendererUI : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Image _playerCellPref;
    [SerializeField] private Image _versusCellPref;
    
    public delegate void RenderTeams();
    public static RenderTeams renderTeams;

    private void Start()
    {
        renderTeams = Render;
    }   

    private void Render()
    {
        Clean();
        Init(BattleMap.instance.turnController.playerList);
    }

    private void Clean()
    {
        foreach (Transform child in _container)
            Destroy(child.gameObject);
    }

    private void Init(List<Player> playerList)
    {
        List<Player> sortedPlayerList = playerList.OrderBy(o=>o.team).ToList();
        
        int currentTeam = sortedPlayerList[0].team;

        for (int i = 0; i < sortedPlayerList.Count; i++)
        {
            if (sortedPlayerList[i].isDefeated || sortedPlayerList[i].objectList.Count == 0)
                continue;
            
            if (currentTeam != sortedPlayerList[i].team)
            {
                Image versusText = Instantiate(_versusCellPref, _container);
            }

            Image playerCell = Instantiate(_playerCellPref, _container);
            playerCell.color = sortedPlayerList[i].color;
            currentTeam = sortedPlayerList[i].team;
        }
    }
}
