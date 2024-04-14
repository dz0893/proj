using UnityEngine;

public class HealingPoint : UnmaterialObject
{
	[SerializeField] private int _healingValue;
	
	[SerializeField] private bool _isRepairingPoint;
	
	private MaterialObject obj => position.onCellObject;
	
	public override void StartTurn()
	{
		if (obj != null && obj.team == team)
		{
			if (!(obj.isMech ^ _isRepairingPoint))
				obj.RestoreHealth(_healingValue);
		}
	}
}
