using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public int skillId;
    //수비 0 버프 1 공격 2
    public int skillPriority; 
    public string skillName;
    //계산되는 슬롯의 인덱스를 넣는다. 
    public int[] useSlotIndices;
    //스킬 발동될때 재생될 애니메이션의 트리거 
    public string animationTrigger;
    //버프 스킬의 경우 사용될 변수
    public BuffData buffToApply;
    public int buffMagnitude, buffDuration;
    public int CalculateNumber(int[] slotValues)
    {
        int power = 0;  
        foreach(int index in useSlotIndices)
        {
            if (index < slotValues.Length)
                power += slotValues[index];
        }
        return power;
    }

    public virtual void ExecuteSkill(UnitBase caster, UnitBase target, int num)
    {
        caster.anim.SetTrigger(animationTrigger);
    }
}
