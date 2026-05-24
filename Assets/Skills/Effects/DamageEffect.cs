using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Skills/Effects/Damage")]
public class DamageEffect : SkillEffect
{
    public override void Execute(GameObject caster, GameObject target, int amount)
    {
        Debug.Log($"{caster.name}이(가) {target.name}에게 {amount}의 피해를 입힙니다!");
    }
}
