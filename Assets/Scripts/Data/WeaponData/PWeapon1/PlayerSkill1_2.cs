using UnityEngine;
[CreateAssetMenu(menuName = "Skills/1_2")]
public class PlayerSkill1_2 : SkillData
{
    public override void ExecuteSkill(UnitBase caster, UnitBase target, int num)
    {
        base.ExecuteSkill(caster, target, num);
        caster.AddShield(num);
    }
}
