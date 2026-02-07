using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Dictionary<int, RelicData> playerRelicData = new Dictionary<int, RelicData>();
    public List<WeaponData> weaponDatas = new List<WeaponData>();
    public int playerCurrentHp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }

    public RelicData[] GetRelicDatas()
    {
        RelicData[] data = new RelicData[playerRelicData.Count];
        foreach(int i in playerRelicData.Keys)
        {
            data[i] = playerRelicData[i];
        }
        return data;    
    }

    public void AchieveRelic(int key)
    {
        playerRelicData.Add(playerRelicData.Count, RelicDatabase.instance.relics[key]);
    }
}
