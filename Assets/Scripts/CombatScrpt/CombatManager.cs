using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{

    public GameManager gameManager;
    public Animator textAnimator;
    int[] slotNum = {1, 1, 1}; // 플레이어가 가진 슬롯 표현하기 위한 변수 일단 지금은 내가 입력했음 나중에 플레이어 데이터에서 직접 받아와야 함 
    //플레이어 관련 변수 
    RelicData[] playerRelics;

    //적 관련 변수 
    GameObject enemy;

    //UI
    public Text relicText, phaseText; //phase text 는 임시 
    public Button button1, button2, rollButton;
    private bool isWaitingUserInput, skillPressed;
    
    public SlotManager slotManager;
    void Start()
    {
        //아래 두 줄은 유물 발생을 테스트하기 위해서 임시로 넣은 코드임
        gameManager.AchieveRelic(1);
        gameManager.AchieveRelic(2);
        playerRelics = gameManager.GetRelicDatas();
        slotManager = FindAnyObjectByType<SlotManager>();
        StartCoroutine(TurnStarter());
    }


    void Update()
    {
        
    }
    


    IEnumerator TurnStarter()
    {
        yield return StartCoroutine(PreTurn());
        yield return StartCoroutine(Turn());
        yield return StartCoroutine(PostTurn());
    }
    //전투 시작 시 처리하는 요소 : 유물 효과, 적 스킬 선택 
    IEnumerator PreTurn()
    {
        // 턴 시작 페이즈 : 1. 유물 효과 발동 2. 몬스터 스킬 지정. 3. 유저 스킬 선택. 
        //유물 효과 발동
        StartCoroutine(Relics());
        //몬스터 스킬 지정
        EnemyMoveAlloc();
        //유저 입력 대기 
        WaitUserInput();
        yield return new WaitUntil(() => skillPressed);
        EndUserInput();
        yield return null;
    }
    IEnumerator Roll()
    {
        slotManager.RollFunc();
        yield return null;
    }
    //직접 전투 
    IEnumerator Turn()
    {

        yield return null;
    }

    //전투 후처리 
    IEnumerator PostTurn()
    {
        yield return null;
    }


    //유물 처리
    IEnumerator Relics()
    {
        relicText.gameObject.SetActive(true);
        //현재는 유물이 텍스트만 띄우고 있지만 추후 유물의 실제 효과도 적용해야 함
        foreach(RelicData i in playerRelics)
        {
            relicText.text = "Relic [" + i.num + "] Activated";
            textAnimator.Play("Relic Text", 0, 0f);
            yield return new WaitForSeconds(1f);
        }
        relicText.gameObject.SetActive(false);
    }

    //몬스터 스킬 지정
    void EnemyMoveAlloc()
    {

    }

    void WaitUserInput()
    {
        isWaitingUserInput = true;
        skillPressed = false;

        rollButton.interactable = true;
        button1.interactable = true;
        button2.interactable = true;
    }

    void EndUserInput()
    {
        isWaitingUserInput = false;

        rollButton.interactable = false;
        button1.interactable = false;
        button2.interactable = false;
    }

    void OnClickSkill()
    {
        if (!isWaitingUserInput) return;
            skillPressed = true;
    }
}


