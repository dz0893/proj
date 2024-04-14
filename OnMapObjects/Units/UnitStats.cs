public struct UnitStats
{
	public int maxHealth { get; private set; }
	public int maxMana { get; private set; }
	
	public int damage { get; private set; }
	public int physicalDefence { get; private set; }
	public int piercingDefence { get; private set; }
	public int magicDefence { get; private set; }
	public int siegeDefence { get; private set; }
	
	public int maxMovePoints { get; private set; }
	public int expForDestroing { get; private set; }
	
	public int attackRange { get; private set; }
	public int minAttackRange { get; private set; }
	
	public int restoreManaPerMeditate { get; private set; }
	public int healthRegen { get; private set; }
	public int manaRegen { get; private set; }
	
	public UnitStats(int maxHealth, int maxMana, int damage, int physicalDefence, int piercingDefence, int magicDefence, 
		int siegeDefence, int maxMovePoints, int expForDestroing, int attackRange, int minAttackRange, 
		int restoreManaPerMeditate, int healthRegen, int manaRegen)
	{
		this.maxHealth = maxHealth;
		this.maxMana = maxMana;
		this.damage = damage;
		this.physicalDefence = physicalDefence;
		this.piercingDefence = piercingDefence;
		this.magicDefence = magicDefence;
		this.siegeDefence = siegeDefence;
		this.maxMovePoints = maxMovePoints;
		this.expForDestroing = expForDestroing;
		this.attackRange = attackRange;
		this.minAttackRange = minAttackRange;
		this.restoreManaPerMeditate = restoreManaPerMeditate;
		this.healthRegen = healthRegen;
		this.manaRegen = manaRegen;
		
	//	SetBoards();
	}
	
	public UnitStats(UnitStats stat1, UnitStats stat2)
	{
		this.maxHealth = stat1.maxHealth + stat2.maxHealth;
		this.maxMana = stat1.maxMana + stat2.maxMana;
		this.damage = stat1.damage + stat2.damage;
		this.physicalDefence = stat1.physicalDefence + stat2.physicalDefence;
		this.piercingDefence = stat1.piercingDefence + stat2.piercingDefence;
		this.magicDefence = stat1.magicDefence + stat2.magicDefence;
		this.siegeDefence = stat1.siegeDefence + stat2.siegeDefence;
		this.maxMovePoints = stat1.maxMovePoints + stat2.maxMovePoints;
		this.expForDestroing = stat1.expForDestroing + stat2.expForDestroing;
		this.attackRange = stat1.attackRange + stat2.attackRange;
		this.minAttackRange = stat1.minAttackRange + stat2.minAttackRange;
		this.restoreManaPerMeditate = stat1.restoreManaPerMeditate + stat2.restoreManaPerMeditate;
		this.healthRegen = stat1.healthRegen + stat2.healthRegen;
		this.manaRegen = stat1.manaRegen + stat2.manaRegen;
		
		SetBoards();
	}
	
	public UnitStats(UnitStats stats)
	{
		this.maxHealth = stats.maxHealth;
		this.maxMana = stats.maxMana;
		this.damage = stats.damage;
		this.physicalDefence = stats.physicalDefence;
		this.piercingDefence = stats.piercingDefence;
		this.magicDefence = stats.magicDefence;
		this.siegeDefence = stats.siegeDefence;
		this.maxMovePoints = stats.maxMovePoints;
		this.expForDestroing = stats.expForDestroing;
		this.attackRange = stats.attackRange;
		this.minAttackRange = stats.minAttackRange;
		this.restoreManaPerMeditate = stats.restoreManaPerMeditate;
		this.healthRegen = stats.healthRegen;
		this.manaRegen = stats.manaRegen;
		
		SetBoards();
	}
	
	private void SetBoards()
	{
		SetDefenceBoards();
		SetAttackRangeBoards();
	}
	
	private void SetDefenceBoards()
	{
		if (this.physicalDefence > ObjectSettings.MAXDEFENCE)
			this.physicalDefence = ObjectSettings.MAXDEFENCE;
		if (this.physicalDefence < ObjectSettings.MINDEFENCE)
			this.physicalDefence = ObjectSettings.MINDEFENCE;
		
		if (this.piercingDefence > ObjectSettings.MAXDEFENCE)
			this.piercingDefence = ObjectSettings.MAXDEFENCE;
		if (this.piercingDefence < ObjectSettings.MINDEFENCE)
			this.piercingDefence = ObjectSettings.MINDEFENCE;
		
		if (this.magicDefence > ObjectSettings.MAXDEFENCE)
			this.magicDefence = ObjectSettings.MAXDEFENCE;
		if (this.magicDefence < ObjectSettings.MINDEFENCE)
			this.magicDefence = ObjectSettings.MINDEFENCE;
		
		if (this.siegeDefence > ObjectSettings.MAXDEFENCE)
			this.siegeDefence = ObjectSettings.MAXDEFENCE;
		if (this.siegeDefence < ObjectSettings.MINDEFENCE)
			this.siegeDefence = ObjectSettings.MINDEFENCE;
	}
	
	private void SetAttackRangeBoards()
	{
		if (attackRange <= minAttackRange)
			attackRange = minAttackRange + 1;
	}
}
