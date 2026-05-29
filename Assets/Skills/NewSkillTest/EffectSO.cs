using UnityEngine;

public enum DurationType
{
    Instant,
    TurnBased,
    StackBased
}
public enum TickTiming
{
    None,
    TurnStart,
    TurnEnd
}
public enum StackType
{
    None,
    Duration,
    Intensity
}
public enum DecayType
{
    None,
    Constant,
    Percentage
}

public enum DecayTiming
{
    None,
    TurnEnd,
    TurnStart
}

public enum EffectClassType

{
    InstantDamage,
    DotDamage,
    StatBuff

}
[CreateAssetMenu(fileName = "SkillEffect", menuName = "Scriptable Objects/SkillEffect")]
public abstract class EffectSO : ScriptableObject
{


    public int effectId;
    public string effectCode;
    public DurationType durationType;
    public TickTiming tickTiming;
    public StackType stackType;
    public DecayType decayType;
    public DecayTiming decayTiming;
    public int maxStacks;
    public int decayValue;
    public int effectOrder;
    public Sprite effectIcon;
    public EffectClassType effectClassType;

    public abstract void Execute(GameObject caster, GameObject target, int amount);
}
