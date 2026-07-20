using System.Collections.Generic;
using UnityEngine;

public class RelicDatabase : MonoBehaviour
{
    //싱글톤
    public static RelicDatabase instance { get; private set; }

    public Dictionary<int, RelicData> relics = new Dictionary<int, RelicData>();

    private void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    //유물 데이터베이스를 불러온다.
    public void Load()
    {
        TextAsset relicDataAsset = Resources.Load<TextAsset>("RelicDatabase");
        string[] lines = relicDataAsset.text.Split('\n');
        
        /*
         데이터베이스 로드 됐는지 확인 
        for(int i = 0; i < lines.Length; i++)
        {
            Debug.Log(lines[i]);
        }
        */
        for(int i = 3; i< lines.Length-1; i++)
        {
            string[] data = lines[i].Split(',');

            RelicData relic = new RelicData
            {
                num = data[0],
                name = data[1],
                trigger = data[2],
                order = data[3],
                effect = data[4],
                effectLevel = data[5]
            };
            /*
            각 필드 제대로 적용되었는지 확인
            Debug.Log(i.ToString() + " : " + relic.num + " : " + relic.name + " : " + relic.trigger + " : " + relic.order + " : " + relic.effect + " : " + relic.effectLevel);
            */
            relics.Add(int.Parse(relic.num), relic);

            
        }
        Debug.Log("Relic Loaded");
    }

    public RelicData GetRelicData(int id)
    {
        if(relics.TryGetValue(id, out RelicData data))
        {
            return data;
        }
        Debug.LogError("해당하는 아이템이 없음");
        return null;    
    }
}
