using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    //전투 상태
    public enum BattleState
    {
        Idle,
        StartPhase,
        RollPhase,
        CombatPhase
    }
    private BattleState currentState;
    int[] slotNum = {1, 1, 1}; // 플레이어가 가진 슬롯 표현하기 위한 변수 일단 지금은 내가 입력했음 나중에 플레이어 데이터에서 직접 받아와야 함 
    //플레이어 관련 변수 
    Player player;
    RelicData[] playerRelics;
    private int rerollCount;
    public SkillData selectedSkill;
    public WeaponData currentWeapon;
    int acc; //결투력
    //적 관련 변수 
    Enemy enemy;

    //UI
    public Text relicText, phaseText; //phase text 는 임시 
    public Button button1, button2, rollButton;
    private bool isWaitingUserInput, skillPressed, rollPressed;
    public Animator textAnimator;
    //다른 매니저
    public SlotManager slotManager;
    public GameManager gameManager;
    void Start()
    {
        //아래 두 줄은 유물 발생을 테스트하기 위해서 임시로 넣은 코드임
        gameManager.AchieveRelic(1);
        gameManager.AchieveRelic(2);
        playerRelics = gameManager.GetRelicDatas();
        slotManager = FindAnyObjectByType<SlotManager>();
        rerollCount = 1;
        // 1 :PreTurn 2: Turn 3 : PostTurn
        currentState = BattleState.Idle;
        StartCoroutine(TurnStarter());
    }


    void Update()
    {
        
    }
    


    IEnumerator TurnStarter()
    {
        yield return StartCoroutine(StartPhase());
        yield return StartCoroutine(RollPhase());
        yield return StartCoroutine(CombatPhase());
    }
    //전투 시작 전 처리하는 요소 : 유물 효과, 적 스킬 선택 
    IEnumerator StartPhase()
    {
        // 턴 시작 페이즈 : 1. 유물 효과 발동 2. 몬스터 스킬 지정. 3. 유저 스킬 선택. 
        currentState = BattleState.StartPhase;
        //유물 효과 발동
        StartCoroutine(Relics());
        //몬스터 스킬 지정
        EnemyMoveAlloc();
        //유저 입력 대기 
        WaitUserInput();
        yield return new WaitUntil(() => skillPressed);
        EndUserInput();
        skillPressed = false;
    }

    //스킬 선택 후 슬롯 회전 
    IEnumerator RollPhase()
    {
        //슬롯회전페이즈 : 1.굴림 2. 재굴림 3.슬롯 요소 발동 (3은 아직 미구현)
        currentState = BattleState.RollPhase;
        StartCoroutine(Roll());
        yield return new WaitUntil(() => slotManager.isRollEnd);
        WaitUserInput();
        yield return new WaitUntil(() => rollPressed);
        EndUserInput();
        yield return new WaitUntil(() => slotManager.isRollEnd);
        rollPressed = false;
        acc = slotManager.slotValue.Sum(); 
        print("결투력 : " + acc);
    }
    //전투 계산
    IEnumerator CombatPhase()
    {
        currentState = BattleState.CombatPhase;
        yield return null;
    }
    //preturn 함수 
    //유물 발동
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
        if (currentState == BattleState.StartPhase)
        {
            button1.interactable = true;
            button2.interactable = true;
        }
        else if (currentState == BattleState.RollPhase)
            rollButton.interactable = true;
    }
    void EndUserInput()
    {
        isWaitingUserInput = false;

        rollButton.interactable = false;
        button1.interactable = false;
        button2.interactable = false;
    }
    public void UserSkillInput()
    {
        skillPressed = true;
    }

    public void UserRollInput()
    {
        rollPressed = true;
    }

    public void WeaponSelect()
    {

    }
    //Turn 함수
    IEnumerator Roll()
    {
        slotManager.RollFunc();
        yield return null;
    }

    public void EquipWeapon(WeaponData weapon)
    {
        currentWeapon = weapon;
        button1.GetComponent<SkillButton>().mySkillData = weapon.skills[0];
        button2.GetComponent<SkillButton>().mySkillData = weapon.skills[1];
    }
}


