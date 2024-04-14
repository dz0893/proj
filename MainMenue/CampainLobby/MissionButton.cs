using UnityEngine;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour
{
	public string missionName { get; private set; }
	public string missionPrologue { get; private set; }
	public string missionGoal { get; private set; }
	public Sprite missionIcon { get; private set; }
	public BattleMap map { get; private set; }
	
	private Text nameText => transform.GetChild(0).GetComponent<Text>();
	private Scenario scenario => map.GetComponent<Scenario>();
	
	public void SetMission(CampainLobby lobby, int index)
	{
		map = lobby.currentMapList[index];
		
		missionName = scenario.Name;
		missionPrologue = scenario.prologue;
		missionGoal = scenario.goal;
		missionIcon = scenario.prologueIcon;
		
		nameText.text = missionName;
	}
	
	public void PushButton(CampainLobby lobby)
	{
		lobby.SelectMission(this);
	}
}
