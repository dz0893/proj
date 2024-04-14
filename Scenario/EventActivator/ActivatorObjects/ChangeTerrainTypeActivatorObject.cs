using UnityEngine;

[CreateAssetMenu(menuName = "Events/Activator/TerrainTypeChanget")]
public class ChangeTerrainTypeActivatorObject : EventActivatorObject
{
	[SerializeField] private int _requiredCounterValue;
	[SerializeField] private TerrainType _oldType;
	[SerializeField] private TerrainType _newType;
	
	public int requiredCounterValue => _requiredCounterValue;
	public TerrainType oldType => _oldType;
	public TerrainType newType => _newType;
	
	public override EventActivator GetActivator()
	{
		return new ChangeTerrainActivator(this);
	}
} 
