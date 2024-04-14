using System.Collections.Generic;

[System.Serializable]
public struct CampainSaveInfo
{
    public Race race;

    public List<CampainMapSaveInfo> mapInfoList;

    public bool campainEnded;

    public CampainSaveInfo(Race race, List<CampainMapSaveInfo> mapInfoList)
    {
        this.race = race;
        this.mapInfoList = mapInfoList;

        campainEnded = true;

        if (mapInfoList.Count == 0)
            campainEnded = false;
        
        foreach (CampainMapSaveInfo campainMapSaveInfo in mapInfoList)
        {
            if (!campainMapSaveInfo.isCompleted)
            {
                campainEnded = false;
                break;
            }
        }
    }
}
