using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public int skillId;
    public string skillName;
    public int[] coEff;
    public string animationTrigger;
    public string formula;
    //나중에 효과 배열도 만듭시다. 
    public int CalculateNumber(int[] slotValues)
    {
        int power = 0;  
        for(int i = 0; i < coEff.Length; i++)
        {
                power += coEff[i] * slotValues[i];
        }
        return power;
    }

    public virtual void ExecuteSkill(UnitBase caster, UnitBase target, int num)
    {
        caster.anim.SetTrigger(animationTrigger);
    }
}
