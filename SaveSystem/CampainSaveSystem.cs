using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CampainSaveSystem : MonoBehaviour
{
    [SerializeField] private CampainLobby _campainLobby;
    [SerializeField] private List<Race> _campainList;
    public List<Race> campainList => _campainList;
    
    private List<BattleMap> humanMapList => _campainLobby.humanMapList;
    private List<BattleMap> orcMapList => _campainLobby.orcMapList;
    private List<BattleMap> elfMapList => _campainLobby.elfMapList;
    private List<BattleMap> undeadMapList => _campainLobby.undeadMapList;
    private List<BattleMap> dwarfMapList => _campainLobby.dwarfMapList;

    public StorySaveInfo storySaveInfo { get; private set; }

    private static string path;
    private string progressFileName = "prg";
    private DirectoryInfo dir;

    public delegate void CompleteMission(BattleMap map);
    public static CompleteMission completeMission;

    private void Start()
    {
        completeMission = RewriteMap;

        path = Directory.GetCurrentDirectory() + "/Cmp/";
        dir = new DirectoryInfo(@path);

        if (!dir.Exists)
			dir.Create();
        

        InitCompanyProgress();
    }

    public CampainSaveInfo GetCampainSaveInfo(Race race)
    {
        int indexOfCampain = _campainList.IndexOf(race);

        return storySaveInfo.campainSaveInfoList[indexOfCampain];
    }

    private void InitCompanyProgress()
    {
        if(HaveCompainProgress())
            LoadCampainProgress();
        else
            InitNewCampainProgress();
    }

    private bool HaveCompainProgress()
    {
        foreach (FileInfo file in dir.GetFiles())
        {
            if (file.Name.Equals(progressFileName))
                return true;
        }

        return false;
    }

    private void LoadCampainProgress()
    {
        storySaveInfo = JsonUtility.FromJson<StorySaveInfo>(File.ReadAllText(path + progressFileName));
    }

    private void SaveCampainProgress()
    {
        File.WriteAllText(path + progressFileName, JsonUtility.ToJson(storySaveInfo));
    }

    private void InitNewCampainProgress()
    {
        List<CampainSaveInfo> campainSaveInfoList = new List<CampainSaveInfo>();

        campainSaveInfoList.Add(new CampainSaveInfo(Race.Human, GetMapSaveInfoList(humanMapList)));
        campainSaveInfoList.Add(new CampainSaveInfo(Race.Orc, GetMapSaveInfoList(orcMapList)));
        campainSaveInfoList.Add(new CampainSaveInfo(Race.Elf, GetMapSaveInfoList(elfMapList)));
        campainSaveInfoList.Add(new CampainSaveInfo(Race.Undead, GetMapSaveInfoList(undeadMapList)));
        campainSaveInfoList.Add(new CampainSaveInfo(Race.Dwarf, GetMapSaveInfoList(dwarfMapList)));

        storySaveInfo = new StorySaveInfo(campainSaveInfoList);

        File.WriteAllText(path + progressFileName, JsonUtility.ToJson(storySaveInfo));
    }

    private List<CampainMapSaveInfo> GetMapSaveInfoList(List<BattleMap> mapList)
    {
        if (mapList.Count == null)
            return null;
        
        List<CampainMapSaveInfo> campainMapSaveInfoList = new List<CampainMapSaveInfo>();

        for (int i = 0; i < mapList.Count; i++)
        {
            campainMapSaveInfoList.Add(new CampainMapSaveInfo(mapList[i]));
        }

        return campainMapSaveInfoList;
    }

    private void RewriteMap(BattleMap map)
    {
        if (map.GetComponent<CampainMapSettings>() == null)
        {
            Debug.Log("ALARM NO MAPSETTINGS");
            return;
        }

        CampainMapSaveInfo mapSaveInfo = new CampainMapSaveInfo(map);
        Race race = map.GetComponent<CampainMapSettings>().race;
        CampainSaveInfo campainSaveInfo = storySaveInfo.campainSaveInfoList[_campainList.IndexOf(race)];

        campainSaveInfo.mapInfoList[mapSaveInfo.mapIndex] = mapSaveInfo;

        RewriteStorySaveInfo();
        SaveCampainProgress();
    }

    private void RewriteStorySaveInfo()
    {
        List<CampainSaveInfo> newCampainSaveInfoList = new List<CampainSaveInfo>();

        foreach (CampainSaveInfo saveInfo in storySaveInfo.campainSaveInfoList)
        {
            newCampainSaveInfoList.Add(GetCampainSaveInfo(saveInfo));
        }

        storySaveInfo = new StorySaveInfo(newCampainSaveInfoList);
    }

    private CampainSaveInfo GetCampainSaveInfo(CampainSaveInfo saveInfo)
    {
        return new CampainSaveInfo(saveInfo.race, saveInfo.mapInfoList);
    }

    public void SetHeroLevel(CampainMapSettings mapSettings)
    {
        Race race = mapSettings.race;

        int currentHeroLevel = 0;
        int currentExp = 0;

        if (mapSettings.mapIndex > 0)
        {
            currentHeroLevel = storySaveInfo.campainSaveInfoList[_campainList.IndexOf(race)].mapInfoList[mapSettings.mapIndex - 1].heroLevel;
            currentExp = storySaveInfo.campainSaveInfoList[_campainList.IndexOf(race)].mapInfoList[mapSettings.mapIndex - 1].heroExp;
        }

        Unit hero = BattleMap.instance.playerList[mapSettings.playerIndex].hero;

        if (hero != null)
        {
            hero.experience.IncreseLevelTo(currentHeroLevel);
            hero.experience.AddExp(currentExp);
        }
    }

    public int GetLastCampainIndex()
    {
        int campainIndex = 0;

        foreach (CampainSaveInfo saveInfo in storySaveInfo.campainSaveInfoList)
        {
            if (saveInfo.campainEnded)
                campainIndex++;
        }

        if (campainIndex >= campainList.Count)
            campainIndex = campainList.Count - 1;

        return campainIndex;
    }

    public int GetLastMissionIndex()
    {
        int mapIndex = 0;

        foreach (CampainMapSaveInfo saveInfo in storySaveInfo.campainSaveInfoList[GetLastCampainIndex()].mapInfoList)
        {
            if (saveInfo.isCompleted)
                mapIndex++;
        }

        if (mapIndex >= storySaveInfo.campainSaveInfoList[GetLastCampainIndex()].mapInfoList.Count)
            mapIndex = storySaveInfo.campainSaveInfoList[GetLastCampainIndex()].mapInfoList.Count;
        
        return mapIndex;
    }
}
