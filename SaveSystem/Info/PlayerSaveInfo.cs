using UnityEngine;

[System.Serializable]
public struct PlayerSaveInfo
{
	public bool isDefeated;
	public bool isAIPlayer;
	public bool aiIsActive;
	public bool notNullPlayer;
	public bool heroIsDead;
	public UnitSaveInfo hero;
	public int gold;
	public int ore;
	public int team;
	public int countOfHeroReviving;
	public int maxUnitLimit;
	public int colorIndex;
	public int race;
	
	public PlayerSaveInfo(Player player)
	{
		isDefeated = player.isDefeated;
		isAIPlayer = player.isAIPlayer;
		
		if (isAIPlayer)
			aiIsActive = player.aiPlayer.active;
		else
			aiIsActive = false;
		
		notNullPlayer = true;
		
		if (player.hero != null)
		{
			heroIsDead = player.hero.isDead;
			hero = new UnitSaveInfo(player.hero);
		}
		else
		{
			heroIsDead = true;
			hero = new UnitSaveInfo();
		}
		
		gold = player.gold;
		ore = player.ore;
		team = player.team;
		countOfHeroReviving = player.countOfHeroReviving;
		maxUnitLimit = player.maxUnitLimit;
			
		colorIndex = MapSettings.colors.IndexOf(player.color);
		race = (int)player.race;
	}
}
