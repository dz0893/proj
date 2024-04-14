using System;

public class DamageGetter
{
	public int GetDamage(MaterialObject obj, int damage, DamageType damageType)
	{
		int totalDamage;
		int currentDefence = 0;
		int minDamage;// = damage / 2 + damage % 2;
		
		if (damageType == DamageType.Physical)
			currentDefence = obj.currentStats.physicalDefence;
		else if (damageType == DamageType.Piercing)
			currentDefence = obj.currentStats.piercingDefence;
		else if (damageType == DamageType.Magical)
			currentDefence = obj.currentStats.magicDefence;
		else if (damageType == DamageType.Siege)
			currentDefence = obj.currentStats.siegeDefence;
		
		if (currentDefence < 0)
			currentDefence = 0;

		/*
		if (currentDefence > 90)
			currentDefence = 90;
		
		totalDamage = (int)Math.Round((damage * (1f - currentDefence / 100f)));
		*/

		totalDamage = damage - currentDefence;

		if (obj is Unit)
			minDamage = damage / 2 + damage % 2;
		else
			minDamage = damage / 3;

		if (totalDamage < minDamage)
			totalDamage = minDamage;

		if (totalDamage <= 0)
			totalDamage = ObjectSettings.minDamage;
		
		if (totalDamage > obj.currentHealth)
			totalDamage = obj.currentHealth;
		
		return totalDamage;
	}
}