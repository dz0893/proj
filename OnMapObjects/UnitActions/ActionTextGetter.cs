public class ActionTextGetter
{
	private DamageGetter damageGetter = new DamageGetter();
	
	public string GetDamageText(MaterialObject obj, int damage, DamageType damageType)
	{
		string text = "";
		
		if (obj != null)
		{
			int localDamage = damageGetter.GetDamage(obj, damage, damageType);
			
			if (localDamage >= obj.currentHealth)
				text = UISettings.Kill;
			else
				text = localDamage.ToString();
		}
		
		return text;
	}
	
	public string GetHealingText(MaterialObject obj, int healingValue)
	{
		if (obj != null)
		{
			if (obj.currentStats.maxHealth - obj.currentHealth < healingValue)
				return (obj.currentStats.maxHealth - obj.currentHealth).ToString();
			else
				return healingValue.ToString();
		}
		else
			return "";
	}
	
	public string GetRestoreManaText(MaterialObject obj, int restoringValue)
	{
		if (obj != null)
		{
			if (obj.currentStats.maxMana - obj.currentMana < restoringValue)
				return (obj.currentStats.maxMana - obj.currentMana).ToString();
			else
				return restoringValue.ToString();
		}
		else
			return "";
	}
	
	public string GetWastedMana(MaterialObject obj, int wastedValue)
	{
		if (obj != null)
		{
			if (obj.currentMana < wastedValue)
				return obj.currentMana.ToString();
			else
				return wastedValue.ToString();
		}
		else
			return "";
	}
	
	public string GetExpAddingText(MaterialObject obj, int exp)
	{
		if (obj != null && obj is Unit)
		{
			Unit unit = obj as Unit;
			
			if (unit.experience.expToNextLevel - unit.experience.currentExp < exp
			&& unit.experience.currentLevel <= unit.experience.maxLevel)
				return UISettings.NextLevel;
			else
				return exp.ToString();
		}
		else
			return "";
	}
}
