public class UnitInfo : ObjectInfo
{
	public Unit unit { get; private set; }
	
	public Experience experience => unit.experience;
	public UnitStats currentStats => unit.currentStats;
	public UnitStats basicStats => unit.basicStats;
	
	public int currentHealth => unit.currentHealth;
	public int currentMana => unit.currentMana;
	public int healthRegen => unit.currentStats.healthRegen;
	public int manaRegen => unit.currentStats.manaRegen;

	public int currentMovePoints => unit.currentMovePoints;
	
	public bool maxLeveled => experience.currentLevel >= experience.maxLevel;
	
	public override void Init(object obj)
	{
		unit = obj as Unit;
		
		objectName = unit.Name;
	}
}
