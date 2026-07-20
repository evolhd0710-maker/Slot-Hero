using UnityEngine;
[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Effects/Defense")]
public class DefenseEffect : SymbolEffect
{
    public override void Apply(NewPlayer player, NewEnemy enemy, TurnContext context, Symbol parent, int countInTurn)
    {
        context.totalDefense += parent.baseDefense;
    }
}
