using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class MaterialObject : NullObject, IActionEffectTarget
{
	private DamageGetter damageGetter = new DamageGetter();
	
//	[SerializeField] private SpriteRenderer _flag;
//	[SerializeField] private Image _playerColor;
	
//	[SerializeField] private Image _healthBar;
	
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _maxMana;
	
	public int currentHealth { get; set; }
	public int currentMana { get; set; }
	
	[SerializeField] private int _attackRange;
	
	[SerializeField] private int _minAttackRange;
	
	[SerializeField] private int _maxMovePoints;
	[SerializeField] private MovingType _movingType;
	public MovingType movingType => _movingType;
	
	[SerializeField] private List<TerrainType> _terrainKnowingList = new List<TerrainType>();
	public List<TerrainType> terrainKnowingList => _terrainKnowingList;
	
	[SerializeField] private int _damage;
	
	[SerializeField] private int _physicalDefence;
	[SerializeField] private int _piercingDefence;
	[SerializeField] private int _magicDefence;
	[SerializeField] private int _siegeDefence;
	
	[SerializeField] private int _expForDestroing;
	
	[SerializeField] private int _restoreManaPerMeditate;
	[SerializeField] private int _healthRegen;
	[SerializeField] private int _manaRegen;
	
	public UnitStats basicStats { get; private set; }
	public UnitStats currentStats { get; private set; }
	private List<IStatChanger> chaneStatEffectList;
	
	public int currentMovePoints { get; set; }
	
	public abstract bool goingToGraveAtDeath { get; }
	public abstract bool isMech { get; }
	public abstract bool isUndead { get; }
	
	public bool canBeUpgraded { get; set; } = true;
	
	public static UnityEvent<GroundCell, MaterialObject> PositionWasSetted = new UnityEvent<GroundCell, MaterialObject>();
	
	public abstract bool isMovable { get; }

	public void ChangeBasicStats(UnitStats stats)
	{
		basicStats = new UnitStats(basicStats, stats);
		RewriteStats();
	}
	/*
	[ContextMenu("Decrease stats")]
	public void DecrStats()
	{
		_maxHealth /= 10;
		_maxMana /= 10;
		_damage /= 10;
		_healthRegen /= 10;
		_manaRegen /= 10;
	}
	*/
	public void AddStatChanger(IStatChanger statChanger)
	{
		if (chaneStatEffectList.Contains(statChanger))
			return;
		
		chaneStatEffectList.Add(statChanger);
		RewriteStats();
	}
	
	public void RemoveStatChanger(IStatChanger statChanger)
	{
		if (!chaneStatEffectList.Contains(statChanger))
			return;
		
		chaneStatEffectList.Remove(statChanger);
		RewriteStats();
	}
	
	private void RewriteStats()
	{
		int deltaMovePoints = currentStats.maxMovePoints - currentMovePoints;
		
		currentStats = new UnitStats(basicStats);
		
		foreach (IStatChanger changer in chaneStatEffectList)
			currentStats = new UnitStats(currentStats, changer.stats);
		
		if (currentMovePoints != 0)
			currentMovePoints = currentStats.maxMovePoints - deltaMovePoints;
	}
	
	protected override void LocalInit(GroundCell positionCell)
	{
		InitStats();
	}
	
	protected void InitStats()
	{
		basicStats = GetBasicStats();
		
		currentStats = new UnitStats(basicStats);
		
		chaneStatEffectList = new List<IStatChanger>();
		
		currentHealth = _maxHealth;
		currentMana = _maxMana;
		currentMovePoints = _maxMovePoints;
	}

	public UnitStats GetBasicStats()
	{
		return new UnitStats(_maxHealth, _maxMana, _damage, _physicalDefence, _piercingDefence, 
		_magicDefence, _siegeDefence, _maxMovePoints, _expForDestroing,_attackRange, _minAttackRange, 
		_restoreManaPerMeditate, _healthRegen, _manaRegen);
	}
	
	public int GetAttack(int damage, DamageType damageType)
	{
		int exp = GetDamage(damage, damageType);
		
		if (!isDead)
			StartCoroutine(AnimateDamage(damage, damageType));
		
		return exp;
	}
	
	public bool CheckDamageForLetality(int damage, DamageType damageType)
	{
		int totalDamage = damageGetter.GetDamage(this, damage, damageType);
		
		if (currentHealth > totalDamage)
			return false;
		else
			return true;
	}
	
	protected int GetDamage(int damage, DamageType damageType)
	{
		if (isDead)
			return 0;
		
		int totalDamage = damageGetter.GetDamage(this, damage, damageType);
		
		currentHealth -= totalDamage;
		
		if (currentHealth <= 0)
		{
			StartCoroutine(DeathIEnumerator());
		}
		else
		{
			PlayVoiceClip(_takingDamagePhrase);
		}

		AnimationController.write(transform.position, totalDamage.ToString(), Color.red);
		
		RefreshHealthBar();
			
		if (isDead)
			return currentStats.expForDestroing;
		else
			return 0;
	}
	
	public void RefreshHealthBar()
	{
		_objectRenderer.RenderHealthBar();
	}
	
	protected virtual IEnumerator AnimateDamage(int damage, DamageType damageType)
	{
		_objectRenderer.spriteRenderer.color = Color.red;
		yield return new WaitForSeconds(ActionSettings.ATTACKTIME);
		_objectRenderer.spriteRenderer.color = Color.white;
	}
	
	protected virtual IEnumerator AnimateHealing()
	{
		_objectRenderer.spriteRenderer.color = Color.green;
		yield return new WaitForSeconds(ActionSettings.ATTACKTIME);
		_objectRenderer.spriteRenderer.color = Color.white;
	}
	/*
	protected override void SetColor()
	{
		_playerColor.color = player.color;
	}
	*/

	public void AddHealth(int health)
	{
		currentHealth += health;
		RefreshHealthBar();
	}

	public virtual void RestoreHealth(int health)
	{
		if (currentHealth == currentStats.maxHealth)
			return;
		
		int restoredValue = health;
		
		if (currentStats.maxHealth - currentHealth < restoredValue)
			restoredValue = currentStats.maxHealth - currentHealth;
		
		if (restoredValue != 0)
		{
			AnimationController.write(transform.position, restoredValue.ToString(), Color.green);
			StartCoroutine(AnimateHealing());

			AddHealth(restoredValue);
		}
	}
	
	public void AddMana(int mana)
	{
		currentMana += mana;
	}

	public void RestoreMana(int mana)
	{
		if (currentMana == currentStats.maxMana)
			return;
		
		int restoredValue = mana;
		
		if (currentStats.maxMana - currentMana < restoredValue)
			restoredValue = currentStats.maxMana - currentMana;
		
		if (restoredValue > 0)
			AnimationController.write(transform.position, restoredValue.ToString(), Color.blue);
		
		AddMana(restoredValue);
	}
	
	public void WasteMana(int mana)
	{
		currentMana -= mana;
		
		if (currentMana < 0)
			currentMana = 0;
	}

	protected override void CleanPositionOfThisObject()
	{
		position.onCellObject = null;
	}
}
