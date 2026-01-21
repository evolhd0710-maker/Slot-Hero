using System.Collections.Generic;
using UnityEngine;


/*
 *  슬롯 데이터 읽어오는 스크립트 
 *  지금은 슬롯 데이터 상의 순서가 이름으로 설정되어 있어서 int key 를 통해 원하는 번호의 슬롯에 접근할 수 있음
 *  slots[i] 는 슬롯 데이터 테이블의 i 번 슬롯을 의미한다/
 *  어떤 플레이어의 slotnums 가 1, 2, 3 이면 이 데이터 상에서 1번 2번 3번 슬롯을 보유하고 있는 것. 이 구현이 맞는지 모르겠음 이건 기획한테 확인할 필요가 있을 듯
 * 
 */
public class SlotDataReader : MonoBehaviour
{   
    //누가 접근할지 몰라서 일단 싱글톤으로 설정해놨는데 지금은 CombatManager만 접근하기 때문에 추후 직접 참조로 전환할 여지 있음
    public static SlotDataReader Instance { get; private set; }
    //임시로 2 나중에 슬롯 추가할 수 있는 변수 설정합시다.
    public SlotData[] slots;
    public Dictionary<int , SlotData> data;

    private void Awake()
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

    void Update()
    {
        
    }

    public void Load()
    {
        if (data == null) data = new Dictionary<int, SlotData>();
        TextAsset slotData = Resources.Load<TextAsset>("SlotData");
        string[] row = slotData.text.Split('\n');
        string[] column;
        slots = new SlotData[row.Length -5]; //위쪽 인덱스 4개 + 마지막줄 에 \n 하나 있어서 그것까지 5개 
        print("row length " + row.Length);
        for (int i = 0; i < slots.Length; i++)
        {
            //각 슬롯 요소 초기화. 
            slots[i] = new SlotData();
            slots[i].slotIndex = new int[9];
            column = row[i + 4].Split(',');
            for(int j = 0; j < 9; j++)
            {
                print(int.Parse(column[j + 2]));
                slots[i].slotIndex[j] = int.Parse(column[j + 2]);
            }
            data.Add(i + 1, slots[i]);
        }
        
        print("Slot Loaded");
        /*
         *로드된 슬롯 확인해보려고 임시로 추가함
        for (int i = 0; i < slots.Length; i++)
        {
            print("slot : " + i);
            for(int j = 0; j <slots[i].slotIndex.Length; j++)
            {
                print(slots[i].slotIndex[j]); 
            }

        }
        */
    }
}
