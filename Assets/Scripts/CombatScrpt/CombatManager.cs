using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

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
    private bool isTurnEnd;
    //플레이어 관련 변수 
    public Player player;
    RelicData[] playerRelics;
    private int rerollCount;
    public SkillData  playerSelectedSkill;
    private WeaponData currentWeapon;
    int cp;
    int pNum;
    public Animator playerAnimator;
    //적 관련 변수 
    public Enemy enemy;
    int ecp;
    int eNum;
    public SkillData enemySelectedSkill;
    public Animator enemyAnimator;

    //UI
    public Text relicText, phaseText; //phase text 는 임시 
    public Button button1, button2, rollButton;
    private bool isWaitingUserInput, skillPressed, rollPressed;
    public Animator textAnimator;
    public Hp playerHp, enemyHp;
    //다른 매니저
    public SlotManager slotManager;
    public SlotManager enemySlotManager;
    public GameManager gameManager;
    void Start()
    {
 
        //아래 두 줄은 유물 발생을 테스트하기 위해서 임시로 넣은 코드임
        gameManager.AchieveRelic(1);
        gameManager.AchieveRelic(2);
        playerRelics = gameManager.GetRelicDatas();
        rerollCount = 1;
        currentState = BattleState.Idle;
        player.Setup();
        enemy.Setup();
        UpdateHp();
        isTurnEnd = false;
        StartCoroutine(TurnStarter());

    }


    void Update()
    {
        
    }



    IEnumerator TurnStarter()
    {
        while (player.Health > 0 && enemy.Health > 0)
        {
            yield return StartCoroutine(StartPhase());
            yield return StartCoroutine(RollPhase());
            yield return StartCoroutine(CombatPhase());

            yield return new WaitForSeconds(1.0f);

            Debug.Log("턴 종료.");
        }

        BattleResult();
    }
    //전투 시작 전 처리하는 요소 : 유물 효과, 적 스킬 선택 
    IEnumerator StartPhase()
    {
        isTurnEnd = false;
        // 턴 시작 페이즈 : 1. 유물 효과 발동 2. 몬스터 스킬 지정. 3. 유저 스킬 선택. 
        currentState = BattleState.StartPhase;
        //유물 효과 발동
        StartCoroutine(ActivateRelics());
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
        StartCoroutine(Eroll());

        yield return new WaitUntil(() => slotManager.isRollEnd && enemySlotManager.isRollEnd);
        WaitUserInput();
        yield return new WaitUntil(() => rollPressed);
        EndUserInput();
        yield return new WaitUntil(() => slotManager.isRollEnd);
        rollPressed = false;
    }
    //전투 계산 1. 결투 시 발동하는 유물, 효과, 스킬 발동(유물 과 스킬 미구현으로 인해 뒤로 미루겠음) 2. 결투력 계산/비교 3. 추가 효과 리셋 4. 결투 승패& 종료 시를 트리거로 하는 효과 발동 (1과 마찬가지로 미구현) 
    IEnumerator CombatPhase()
    {
        print("CombatPhase 진입");
        currentState = BattleState.CombatPhase;

        yield return new WaitForSeconds(1.0f);  


        yield return new WaitForSeconds(1.0f);
        //스킬 순서 결정
        bool isPlayerFirst;
        if(playerSelectedSkill.skillPriority >= enemySelectedSkill.skillPriority)
            isPlayerFirst = true;
        else
            isPlayerFirst = false; 
        
        CalculateAndCompareCP();

        yield return new WaitForSeconds(1.0f);

        if (isPlayerFirst)
        {
            playerSelectedSkill.ExecuteSkill(player, enemy, pNum);
            yield return new WaitForSeconds(1f);
            UpdateHp();
            if (enemy.Health > 0) 
            {
                enemySelectedSkill.ExecuteSkill(enemy, player, eNum);
                yield return new WaitForSeconds(1f);
                UpdateHp();
            }
        }
        else
        {
            enemySelectedSkill.ExecuteSkill(enemy, player, eNum);
            yield return new WaitForSeconds(1f);
            UpdateHp();
            if (player.Health > 0)
            {
                playerSelectedSkill.ExecuteSkill(player, enemy, pNum);
                yield return new WaitForSeconds(1f);
                UpdateHp();
            }
        }
        TurnEnd();
        UpdateHp();
        isTurnEnd = true;
    }
    //StartPhase Function 
    IEnumerator ActivateRelics()
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
    void EnemyMoveAlloc()
    {
        //1. 사용가능 여부 확인 2. 사용가능 스킬 중에서 랜덤 선택하여 사용(MoveIndex 에 skill 배열의 index 를 할당) 
        enemySelectedSkill = enemy.skills[Random.Range(0, 2)];
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
    public void UserInputSkill()
    {
        skillPressed = true;
    }

    public void UserInputRoll()
    {
        rollPressed = true;
    }


    //RollPhase Function
    IEnumerator Roll()
    {
        slotManager.RollFunc();
        yield return null;
    }
    
    IEnumerator Eroll()
    {
        enemySlotManager.RollFunc();
        yield return null;
    }

    public void EquipWeapon(WeaponData weapon)
    {
        currentWeapon = weapon;
        button1.GetComponent<SkillButton>().mySkillData = weapon.skills[0];
        button2.GetComponent<SkillButton>().mySkillData = weapon.skills[1];
        player.GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimatorController;
    }

    //CombatPhaseFunction
    private void CalculateAndCompareCP()
    {
        cp = slotManager.slotValue.Sum();
        ecp = enemySlotManager.slotValue.Sum();
        //아래 두 줄은 나중에 애니메이션이나 효과로 대체해야함
        Debug.Log("player combat power : " + cp);
        Debug.Log("enemy combat power : " + ecp);
        pNum = playerSelectedSkill.CalculateNumber(slotManager.slotValue);
        print("플레이어 숫자 : " + pNum);
        eNum = enemySelectedSkill.CalculateNumber(enemySlotManager.slotValue);
        print("적 숫자 : " + eNum);

        if (ecp < cp)
        {
            eNum /= 2;
            print("결투력 승리. 적 숫자 (" + eNum + ") 으로 조정됨");
        }
        else if (ecp == cp)
        {
            print("결투력 동등");

        }
        else
        {
            pNum /= 2;
            print("결투력 패배. 플레이어 숫자 (" + pNum + ") 으로 조정됨");
        }
    }
    private void TurnEnd()
    {
        player.shield = 0;
        enemy.shield = 0;
    }

    void BattleResult()
    {
        if (player.Health <= 0)
            Debug.Log("플레이어 패배...");
        else
            Debug.Log("플레이어 승리!");
    }

    void UpdateHp()
    {
        playerHp.SetUp(player.data.maxHealth, player.Health, player.shield);
        enemyHp.SetUp(enemy.data.maxHealth, enemy.Health, enemy.shield);
    }
}


