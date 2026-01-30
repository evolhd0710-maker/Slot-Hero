using JetBrains.Annotations;
using System.Linq;
using UnityEngine;

public class PlayerDataReader : MonoBehaviour
{
    public static PlayerDataReader Instance { get; private set; }
    public PlayerData playerData;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        playerData = new PlayerData();
        TextAsset playerDataFile = Resources.Load<TextAsset>("PlayerData");
        string[] row = playerDataFile.text.Split('\n');
        string[] strings = row[2].Split(',');
        
        playerData.name = strings[0];
        playerData.hp = int.Parse(strings[1]);
        playerData.weapon = int.Parse(strings[2]);
        playerData.relics = strings[3].Split('/').Select(int.Parse).ToArray();
        playerData.slotCount = int.Parse(strings[4]);
        playerData.slotNums = new int[playerData.slotCount];
        for(int i = 0; i < playerData.slotCount; i++)
        {
            playerData.slotNums[i] = int.Parse(strings[i + 5]);
        }
        print("PlayerData Loaded");
    }
}
