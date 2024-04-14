using System.Collections.Generic;

public class GlobalActionInfo : ObjectInfo
{
	public GlobalAction action { get; private set; }
	
	public string currentActivatingWarningText => action.currentActivatingWarningText;
	
	public List<string> descriptionList => action.descriptionList;
	
	public bool enoughtResources => action.CheckResourses(TurnController.lastNotComputerPlayer);
	public bool heroIsAlive => (action.actionRange != GlobalActionRange.OnHero || !TurnController.lastNotComputerPlayer.hero.isDead);
	public bool requiredStructureIsBuilded => action.CheckBuildings(TurnController.lastNotComputerPlayer);
	public bool usedAtThisTurn => action.usedAtThisTurn;
	
	public override void Init(object obj)
	{
		action = obj as GlobalAction;
		
		objectName = action.Name;
	}
}
