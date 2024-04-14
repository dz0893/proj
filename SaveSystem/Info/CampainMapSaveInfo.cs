using UnityEngine;

[System.Serializable]
public struct CampainMapSaveInfo
{
    public int mapIndex;
    public int heroLevel;
    public int heroExp;
    public bool isCompleted;

    public CampainMapSaveInfo(BattleMap map)
    {
        if (BattleMap.instance != map)
        {
            mapIndex = 404;
            heroLevel = 0;
            heroExp = 0;
            isCompleted = false;
        }
        else
        {
            CampainMapSettings mapSettings = map.GetComponent<CampainMapSettings>();
            Experience experience = map.playerList[mapSettings.playerIndex].hero.experience;

            mapIndex = mapSettings.mapIndex;
            
            heroLevel = experience.currentLevel;

            heroExp = 0;

            if (heroLevel < mapSettings.maxHeroLevel)
                heroExp = experience.currentExp;

            isCompleted = true;
        }
    }
}
