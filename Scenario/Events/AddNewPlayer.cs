using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/AddPlayer")]
public class AddNewPlayer : MapEvent
{
	[SerializeField] private Capital _capital;
	[SerializeField] private Unit _hero;
	[SerializeField] private int _team;
	[SerializeField] private Race _race;
	[SerializeField] private bool _isAIPLayer = true;
	[SerializeField] private bool _isActive = true;
	
	public override void CurrentEventActivate()
	{
		Player player = new Player();
		
		player.race = _race;
		
		player.Init(_team);
		
		if (_isAIPLayer)
			player.SetAI(_isActive);
		
		BattleMap.instance.AddNewPlayer(player);
		TeamsRendererUI.renderTeams.Invoke();
	}
}
