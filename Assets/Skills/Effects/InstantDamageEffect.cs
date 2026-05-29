using UnityEngine;

public class InstantDamageEffect : EffectSO
{
    public override void Execute(GameObject caster, GameObject target, int amount)
    {
        target.GetComponent<UnitBase>().TakeDamage(amount, "즉시 데미지");
    }
}
