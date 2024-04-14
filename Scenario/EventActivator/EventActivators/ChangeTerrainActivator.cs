using UnityEngine;

public class ChangeTerrainActivator : EventActivator
{
	private TerrainType oldType;
	private TerrainType newType;
	
	private int requiredCounterValue;
	private int counter;
	
	public ChangeTerrainActivator(ChangeTerrainTypeActivatorObject activatorObject)
	{
		SetDefaultActivatorData(activatorObject);
		oldType = activatorObject.oldType;
		newType = activatorObject.newType;
		requiredCounterValue = activatorObject.requiredCounterValue;
		
		GroundCell.TerrainTypeWasChanged.AddListener(CheckCellForCorrectTypes);
	}
	
	private void CheckCellForCorrectTypes(TerrainType oldType, TerrainType newType)
	{
		if (this.oldType == oldType && this.newType == newType)
			counter++;
		
		TryToActivateEvent();
	}
	
	protected override bool CheckForActiveEvent()
	{
		if (counter == requiredCounterValue)
			return true;
		else
			return false;
	}
	
	public override void RemoveListener()
	{
		GroundCell.TerrainTypeWasChanged.RemoveListener(CheckCellForCorrectTypes);
	}
}
