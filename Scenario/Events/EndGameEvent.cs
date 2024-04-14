using UnityEngine;

[CreateAssetMenu(menuName = "Events/EventData/EndGame")]
public class EndGameEvent : MapEvent
{
	[SerializeField] private bool _isWin;
	
	public override void CurrentEventActivate()
	{
		PlayerUI.openGameResult.Invoke(_isWin);

		if (_isWin)
		{
			CampainSaveSystem.completeMission(BattleMap.instance);
		}
	}
}
