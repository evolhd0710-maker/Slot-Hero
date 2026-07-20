using UnityEngine;

public abstract class SymbolEffect : ScriptableObject
{
    public abstract void Apply(NewPlayer player, NewEnemy enemy, TurnContext context, Symbol parent, int countInTurn);
}
