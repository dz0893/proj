using UnityEngine;

public class CreateMission : Mission
{
	public NullObject objectPref { get; private set; }
	public int count { get; private set; }
	
	public CreateMission(CreateMissionObject missionObject, Player player)
	{
		this.missionObject = missionObject;
		objectPref = missionObject.objectPref;
		count = missionObject.count;
		this.player = player;

		InitTargets();
		
		NullObject.ObjectInited.AddListener(ObjectCreatedListener);
	}
	
	protected override bool CheckForEnded()
	{
		int currentCounter = 0;
		
		foreach (NullObject obj in player.objectList)
		{
			if (obj.Name.Equals(objectPref.Name))
				currentCounter++;
		}
		
		if (currentCounter >= count)
			return true;
		else
			return false;
	}

	private void ObjectCreatedListener(NullObject obj)
	{
		if (objectPref.Name.Equals(obj.Name))
			TryToEndMission();
	}
	
	public override void RemoveListener()
	{
		NullObject.ObjectInited.RemoveListener(ObjectCreatedListener);
	}
}
