using UnityEngine;

[CreateAssetMenu(fileName = "GladiusDefenseEffect", menuName = "Effects/Gladius Defense")]
public class GladiusDefenseEffect :SymbolEffect
{
    public override void Apply(NewPlayer player, NewEnemy enemy, TurnContext context, Symbol parent, int countInTurn)
    {
        context.totalDefense += parent.baseDefense + countInTurn;
    }
}
