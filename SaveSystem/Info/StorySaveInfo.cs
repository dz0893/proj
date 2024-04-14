using System.Collections.Generic;

[System.Serializable]
public struct StorySaveInfo
{
    public List<CampainSaveInfo> campainSaveInfoList;

    public StorySaveInfo(List<CampainSaveInfo> campainSaveInfoList)
    {
        this.campainSaveInfoList = new List<CampainSaveInfo>(campainSaveInfoList);
    }
}
