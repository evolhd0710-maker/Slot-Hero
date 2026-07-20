using UnityEngine;
using UnityEngine.UI;
// 스킬 버튼이 자신이 가지고 있는 스킬의 데이터를 combatmanager로 보내기 위한 스크립트

public class SkillButton : MonoBehaviour
{
    public SkillSO mySkillData;
    public CombatManager combatManager;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    public void OnClicked()
    {
        print("스킬" + mySkillData.name +"선택");
        combatManager.playerSelectedSkill = mySkillData;
    }

    public void SetSkill(SkillSO skill)
    {
        mySkillData = skill;
    }
}

