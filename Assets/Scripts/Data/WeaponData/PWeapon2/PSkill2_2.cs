using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Skills/2_2")]
public class PSkill2_2 : SkillData
{
    public Buff buff;
    public override void ExecuteSkill(UnitBase caster, UnitBase target, int num)
    {
        base.ExecuteSkill(caster, target, num);
        caster.AddBuff(buff);
    }
}
