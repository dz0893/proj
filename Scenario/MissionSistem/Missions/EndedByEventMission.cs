public class EndedByEventMission : Mission
{
	public EndedByEventMission(EndedByEventMissionObject missionObject, Player player)
	{
		this.missionObject = missionObject;
		this.player = player;
		InitTargets();
	}
	
	protected override bool CheckForEnded()
	{
		return false;
	}
	
	public override void RemoveListener() {}
}
