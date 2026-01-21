using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dictionary<int, RelicData> playerRelicData = new Dictionary<int, RelicData>();   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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
