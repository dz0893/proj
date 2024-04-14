using UnityEngine;

public abstract class GlobalActionObject : MonoBehaviour
{
	[SerializeField] private AnimationClip _contactAnimation;

	public AnimationClip onCasterAnimation => null;
	public AnimationClip flyAnimation => null;
	public AnimationClip contactAnimation => _contactAnimation;

	[SerializeField] private AudioClip _contactSound;

	[SerializeField] private float _afterCastSoundDelay;
	public float afterCastSoundDelay => _afterCastSoundDelay;

	public AudioClip castSound => null;
	public AudioClip contactSound => _contactSound;
	
	[SerializeField] private Sprite _icon;
	public Sprite icon => _icon;
	
	[SerializeField] private string _name;
	public string Name => _name;
	
	[SerializeField] private string _description;
	public string description => _description;
	
	[SerializeField] private int _oreCost;
	public int oreCost => _oreCost;
	
	[SerializeField] private int _goldCost;
	public int goldCost => _goldCost;
	
	[SerializeField] private int _buildingLevel;
	public int buildingLevel => _buildingLevel;
	
	[SerializeField] private Building _requiredBuilding;
	public Building requiredBuilding => _requiredBuilding;
	
	[SerializeField] private GlobalActionRange _actionRange;
	public GlobalActionRange actionRange => _actionRange;
	
	[SerializeField] private int _distance;
	public int distance => _distance;
	
	[SerializeField] private int _actionValue;
	public int actionValue => _actionValue;
	
	[SerializeField] private bool _aiCanUse;
	public bool aiCanUse => _aiCanUse;

	[SerializeField] private bool _isAreaAction;
	public bool isAreaAction => _isAreaAction;
	
	public abstract GlobalAction GetGlobalAction();
}
