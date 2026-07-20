using UnityEngine;
[CreateAssetMenu(fileName = "DamageEffect", menuName = "Effects/Damage")]
public class DamageEffect : SymbolEffect
{
    public override void Apply(NewPlayer player, NewEnemy enemy, TurnContext context, Symbol parent, int countInTurn)
    {
        context.totalDamage += (parent.baseAttack);
    }
}
