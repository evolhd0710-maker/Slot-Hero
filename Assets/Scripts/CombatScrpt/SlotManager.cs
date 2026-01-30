using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SlotManager : MonoBehaviour
{
    public GameObject[] slotImg;
    public GameObject slotPoint;
    public GameObject slotControl;
    public GameObject[] slotSet;
    public GameObject emptyPrefab;
    public GameObject spriteMask;
    public GameObject slotMask;
    public SlotMask[] slotMaskArr;
    public int[] slotValue;
    private int[,] slotTable;
    private bool[] rollDone;
    private Transform canvasTransform;
    public bool isRollEnd;

    private void Awake()
    {
        
    }
    void Start()
    {
        canvasTransform = FindAnyObjectByType<Canvas>().transform;
        int slotCount = PlayerDataReader.Instance.playerData.slotCount;
        // slotset 은 각 릴을 의미 slotTalbe은 각 슬롯의 숫자 구성을 저장, slotaValue는 각 슬롯의 값을 저장한다.
        slotSet = new GameObject[slotCount];
        slotTable = new int[slotCount, 9];
        slotMaskArr = new SlotMask[slotCount];
        slotValue = new int[slotSet.Length];
        MakeSlot();
    }


    void Update()
    {
        
    }

    void MakeSlot()
    {
        for (int i = 0; i < slotSet.Length; i++)
        {
            slotSet[i] = Instantiate(emptyPrefab, slotPoint.transform.position + new Vector3(i, 0, 0), Quaternion.identity, slotControl.transform);
        }
        for (int i = 0; i < slotSet.Length; i++)
        {

            //슬롯테이블 채우기 위한 숫자
            int num = 0;
            //슬롯마다 한칸씩 밀기 위한 offset
            int slotPositionOffset = 0;
            int slotId = PlayerDataReader.Instance.playerData.slotNums[i]; //플레이어 슬롯 ID 받아와서 해당하는 슬롯 만든다.
            GameObject tmpSlot = null;
            for (int j = 0; j < 9; j++)
            {

                for (int k = 0; k < SlotDataReader.Instance.slots[slotId].slotIndex[j]; k++)
                {
                    slotTable[i, num++] = j + 1;
                    if (tmpSlot == null)
                    {
                        tmpSlot = slotImg[j];
                        Instantiate(spriteMask, slotPoint.transform.position + new Vector3(i, slotPositionOffset, 0), Quaternion.identity, canvasTransform);
                        slotMaskArr[i] = Instantiate(slotMask, slotPoint.transform.position + new Vector3(i, slotPositionOffset, 0), Quaternion.identity, canvasTransform).GetComponent<SlotMask>();
                        slotMaskArr[i].index = i;
                    }
                    Instantiate(slotImg[j], slotPoint.transform.position + new Vector3(i, slotPositionOffset++, 0), Quaternion.identity, slotSet[i].transform);
                }
            }
            //슬롯 회전 효과 주려면 시작과 끝이 동일해야함 그러기 위해서 처음 생성된 슬롯을 tmp 슬롯에 저장해서 마지막에 생성해준다.
            if (tmpSlot != null)
            {
                Instantiate(tmpSlot, slotPoint.transform.position + new Vector3(i, slotPositionOffset++, 0), Quaternion.identity, slotSet[i].transform);
            }
        }
    }

    IEnumerator Roll(int index)
    {
        int minTime = 40;
        int randTime = Random.Range(0, 30);
        randTime = (randTime + 3) / 4 * 4;

        float waitTime = 0.05f;

        for (int i = 0; i < minTime; i++)
        {
            MoveSlot(index);
            yield return new WaitForSeconds(waitTime);
        }

        for (int i = 0; i < randTime; i++)
        {
            waitTime = Mathf.Lerp(0.05f, 0.2f, (float)i / randTime);
            MoveSlot(index);
            yield return new WaitForSeconds(waitTime);
        }

        // 위치에 따른 값 대입해줘야함 
        ReturnValue(index);

        rollDone[index] = true;

    }

    void MoveSlot(int index)
    {
        var pos = slotSet[index].transform.position;

        if (pos.y <= -6.0f)
            pos.y = 2.75f;
        else
            pos.y -= 0.25f;
        slotSet[index].transform.position = pos;
    }

    IEnumerator RollAll()
    {
        isRollEnd = false;  
        int n = slotSet.Length;              // 보통 3
        rollDone = new bool[n];

        for (int i = 0; i < n; i++)
        {
            if (!slotMaskArr[i].selected)
                StartCoroutine(Roll(i));
            else
                rollDone[i] = true;
        }
            
            

        yield return new WaitUntil(() =>
        {
            for (int i = 0; i < n; i++)
                if (!rollDone[i]) return false;
            return true;
        });
        for (int i = 0; i < n; i++)
            slotMaskArr[i].ResetMask();
        isRollEnd = true;
    }

    public void RollFunc()
    {
        StartCoroutine(RollAll());
    }

    public void ReturnValue(int num)
    {
        if (slotSet[num].transform.position.y == 3 || slotSet[num].transform.position.y == -6)
            slotValue[num] = slotTable[num, 0];
        else if (slotSet[num].transform.position.y == 2)
            slotValue[num] = slotTable[num, 1];
        else if (slotSet[num].transform.position.y == 1)
            slotValue[num] = slotTable[num, 2];
        else if (slotSet[num].transform.position.y == 0)
            slotValue[num] = slotTable[num, 3];
        else if (slotSet[num].transform.position.y == -1)
            slotValue[num] = slotTable[num, 4];
        else if (slotSet[num].transform.position.y == -2)
            slotValue[num] = slotTable[num, 5];
        else if (slotSet[num].transform.position.y == -3)
            slotValue[num] = slotTable[num, 6];
        else if (slotSet[num].transform.position.y == -4)
            slotValue[num] = slotTable[num, 7];
        else if (slotSet[num].transform.position.y == -5)
            slotValue[num] = slotTable[num, 8];
    }



    
}
