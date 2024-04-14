using UnityEngine;

[CreateAssetMenu(menuName = "CapitalData/Upgrades/ChangeStats")]
public class ChangeStatsUpgradeObject : UpgradeObject
{
    [SerializeField] private  int _maxHealth;
	[SerializeField] private  int _maxMana;
	
	[SerializeField] private  int _damage;
	[SerializeField] private  int _physicalDefence;
	[SerializeField] private  int _piercingDefence;
	[SerializeField] private  int _magicDefence;
	[SerializeField] private  int _siegeDefence;
	
	[SerializeField] private  int _maxMovePoints;
	[SerializeField] private  int _expForDestroing;
	
	[SerializeField] private  int _attackRange;
	[SerializeField] private  int _minAttackRange;
	
	[SerializeField] private  int _restoreManaPerMeditate;
	[SerializeField] private  int _healthRegen;
	[SerializeField] private  int _manaRegen;

    public UnitStats unitStats => new UnitStats(_maxHealth, _maxMana, _damage, _physicalDefence, _piercingDefence, 
	_magicDefence, _siegeDefence, _maxMovePoints, _expForDestroing,_attackRange, _minAttackRange, 
	_restoreManaPerMeditate, _healthRegen, _manaRegen);

    public override Upgrade GetUpgrade()
    {
        return new ChangeStatsUpgrade(this);
    }
}
