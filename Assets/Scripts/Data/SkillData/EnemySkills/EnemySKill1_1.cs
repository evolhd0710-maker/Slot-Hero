using UnityEngine;

[CreateAssetMenu(menuName = "EnemySkills/1_1")]
public class EnemySKill1_1:SkillData
{
    public override void ExecuteSkill(UnitBase caster, UnitBase target, int num)
    {
        base.ExecuteSkill(caster, target, num);
        base.ExecuteSkill(caster, target, num);
        target.TakeDamage(num);
    }
}
