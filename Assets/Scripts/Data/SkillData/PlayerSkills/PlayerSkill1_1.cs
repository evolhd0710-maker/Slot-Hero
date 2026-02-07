using UnityEngine;
//1번 무기의 1번스킬 
[CreateAssetMenu(menuName = "Skills/1_1")]
public class PlayerSkill1_1 : SkillData
{
    public override void ExecuteSkill(UnitBase caster, UnitBase target, int num)
    {
        base.ExecuteSkill(caster, target, num);
        target.TakeDamage(num);
        Debug.Log($"{target.name}에 {num} 데미지");
    }
}
