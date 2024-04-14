using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/ChangeTeam")]
public class ChangeTeamEvent : MapEvent
{
	[SerializeField] private int _changedTeamPlayer;
	[SerializeField] private int _team;
	
	public override void CurrentEventActivate()
	{
		BattleMap.instance.playerList[_changedTeamPlayer].SetTeam(_team);
		TeamsRendererUI.renderTeams.Invoke();
	}
}
