using UnityEngine;

public class DotDamageEffect : EffectSO
{
    public override void Execute(GameObject caster, GameObject target, int amount)
    {

        target.GetComponent<UnitBase>().TakeDamage(amount,"도트 데미지");
    }
}
