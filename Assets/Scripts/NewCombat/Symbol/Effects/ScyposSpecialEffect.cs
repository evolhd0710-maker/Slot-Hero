using UnityEngine;
[CreateAssetMenu(fileName = "ScyposSpecialEffect", menuName ="SpecialEffects/Scypos")]
public class ScyposSpecialEffect :SymbolEffect
{
    public override void Apply(NewPlayer player, NewEnemy enemy, TurnContext context, Symbol parent, int countInTurn)
    {
        enemy.TakeDamage(parent.baseAttack, "사이포스 특수효과");
    }
}
