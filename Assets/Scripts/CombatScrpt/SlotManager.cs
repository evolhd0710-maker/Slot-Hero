using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


public enum SlotOwner { Player, enemy }

public class SlotManager : MonoBehaviour
{

    public SlotOwner owner;
    public Enemy enemy;
    public GameObject[] slotImg;
    public GameObject slotPoint;
    //없어도 될것같으니 한번 시도해봅시다. 02.04기준 안해봄 
    public GameObject slotControl;
    public GameObject[] slotSet;
    public GameObject emptyPrefab;
    //슬롯 특정 부분만 보이도록 하는 스프라이트마스크
    public GameObject spriteMask;
    //슬롯 고정 효과 내기 위해 슬롯 위에 덮어두는 마스크
    public GameObject slotFixWindow;
    public SlotMask[] slotFixArr;

    public int[] slotValue;
    private int[,] slotTable;
    private bool[] rollDone;
    private Transform canvasTransform;
    public bool isRollEnd;

    private void Awake()
    {

    }
    IEnumerator Start()
    {
        //슬롯데이터 로딩될때까지 기다리는 코드 
        while (SlotDataReader.Instance == null || !SlotDataReader.Instance.IsReady)
        {
            yield return null;
        }
        //플레이어 슬롯
        if (owner == SlotOwner.Player)
        {
            int slotCount = 3;
            // slotset 은 각 릴을 의미 slotTalbe은 각 슬롯의 숫자 구성을 저장, slotaValue는 각 슬롯의 값을 저장한다.
            slotSet = new GameObject[slotCount];
            slotTable = new int[slotCount, 9];
            slotFixArr = new SlotMask[slotCount];
            slotValue = new int[] {0,0,0,0,0 };
            MakeSlot();
        }
        //적 슬롯
        else
        {
            int slotCount = 3;
            slotSet = new GameObject[slotCount];
            slotValue = new int[] { 0, 0, 0, 0, 0 };
            slotFixArr = new SlotMask[slotCount];
            slotTable = new int[slotCount, 9];
            MakeSlot();
        }
        canvasTransform = FindAnyObjectByType<Canvas>().transform;
        
    }


    void Update()
    {

    }

    void MakeSlot()
    {

        //슬롯 변경되기 전, 슬롯데이터에서 슬롯 생성하는 코드
        if(GameManager.Instance.playerSlotTable == null)
        {
            for (int i = 0; i < slotSet.Length; i++)
            {
                slotSet[i] = Instantiate(emptyPrefab, slotPoint.transform.position + new Vector3(i, 0, 0), Quaternion.identity, slotControl.transform);
            }
            for (int i = 0; i < slotSet.Length; i++)
            {
                //슬롯마다 한칸씩 밀기 위한 offset
                int slotPositionOffset = 0;
                int slotId;
                if (owner == SlotOwner.Player)
                {
                    slotId = PlayerDataReader.Instance.playerData.slotReelName[i];
                }
                else
                {
                    slotId = enemy.slotNums[i];
                }
                //플레이어 슬롯 ID 받아와서 해당하는 슬롯 만든다.
                int num = 0;
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
                            slotFixArr[i] = Instantiate(slotFixWindow, slotPoint.transform.position + new Vector3(i, slotPositionOffset, 0), Quaternion.identity, canvasTransform).GetComponent<SlotMask>();
                            slotFixArr[i].index = i;
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
        //플레이어가 슬롯 변경한 후의 슬롯 불러오는 코드 
        else
        {
            slotTable = GameManager.Instance.playerSlotTable;
            for (int i = 0; i < slotSet.Length; i++)
            {
                slotSet[i] = Instantiate(emptyPrefab, slotPoint.transform.position + new Vector3(i, 0, 0), Quaternion.identity, slotControl.transform);
            }
            for (int i = 0; i < slotSet.Length; i++)
            {
                //슬롯마다 한칸씩 밀기 위한 offset
                int slotPositionOffset = 0;
                int slotId;
                GameObject tmpSlot = null;
                for (int j = 0; j < 9; j++)
                {
                        if (tmpSlot == null)
                        {
                            tmpSlot = slotImg[slotTable[i,j]];
                            Instantiate(spriteMask, slotPoint.transform.position + new Vector3(i, slotPositionOffset, 0), Quaternion.identity, canvasTransform);
                            slotFixArr[i] = Instantiate(slotFixWindow, slotPoint.transform.position + new Vector3(i, slotPositionOffset, 0), Quaternion.identity, canvasTransform).GetComponent<SlotMask>();
                            slotFixArr[i].index = i;
                        }
                        Instantiate(slotImg[slotTable[i, j]], slotPoint.transform.position + new Vector3(i, slotPositionOffset++, 0), Quaternion.identity, slotSet[i].transform);
                }
                //슬롯 회전 효과 주려면 시작과 끝이 동일해야함 그러기 위해서 처음 생성된 슬롯을 tmp 슬롯에 저장해서 마지막에 생성해준다.
                if (tmpSlot != null)
                {
                    Instantiate(tmpSlot, slotPoint.transform.position + new Vector3(i, slotPositionOffset++, 0), Quaternion.identity, slotSet[i].transform);
                }
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


        Vector3 finalPos = slotSet[index].transform.position;
        finalPos.y = Mathf.Round(finalPos.y); // 소수점을 반올림해서 정수로 만듦
        slotSet[index].transform.position = finalPos;


        // 위치에 따른 값 대입해줘야함 
        ReturnValue(index);

        rollDone[index] = true;

    }

    void MoveSlot(int index)
    {
        var pos = slotSet[index].transform.position;

        if (pos.y <= -9)
            pos.y = 0;
        else
            pos.y -= 0.25f;
        slotSet[index].transform.position = pos;
    }

    IEnumerator RollAll()
    {
        isRollEnd = false;
        int n = slotSet.Length;              // 보통 3
        rollDone = new bool[n];

        for (int i = 0; i < n; i++)
        {
            if (!slotFixArr[i].selected)
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
            slotFixArr[i].ResetMask();
        isRollEnd = true;
    }

    public void RollFunc()
    {
        StartCoroutine(RollAll());
    }

    public void ReturnValue(int num)
    {
        if (slotSet[num].transform.position.y == 0 || slotSet[num].transform.position.y == -9)
            slotValue[num] = slotTable[num, 0];
        else if (slotSet[num].transform.position.y == -1)
            slotValue[num] = slotTable[num, 1];
        else if (slotSet[num].transform.position.y == -2)
            slotValue[num] = slotTable[num, 2];
        else if (slotSet[num].transform.position.y == -3)
            slotValue[num] = slotTable[num, 3];
        else if (slotSet[num].transform.position.y == -4)
            slotValue[num] = slotTable[num, 4];
        else if (slotSet[num].transform.position.y == -5)
            slotValue[num] = slotTable[num, 5];
        else if (slotSet[num].transform.position.y == -6)
            slotValue[num] = slotTable[num, 6];
        else if (slotSet[num].transform.position.y == -7)
            slotValue[num] = slotTable[num, 7];
        else if (slotSet[num].transform.position.y == -8)
            slotValue[num] = slotTable[num, 8];
    }



    void printArr(int[,] grid)
    {
        // 문자열 보간을 이용해 표 형태로 만들기
        string result = "--- 3x9 Matrix ---\n";
        for (int i = 0; i < grid.GetLength(0); i++) // 행(3)
        {
            for (int j = 0; j < grid.GetLength(1); j++) // 열(9)
            {
                result += grid[i, j] + "\t"; // 탭으로 간격 맞추기
            }
            result += "\n"; // 줄바꿈
        }

        Debug.Log(result);
    }

}
