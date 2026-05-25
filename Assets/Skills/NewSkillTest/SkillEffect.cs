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
[CreateAssetMenu(fileName = "SkillEffect", menuName = "Scriptable Objects/SkillEffect")]
public abstract class SkillEffect : ScriptableObject
{


    public int effectId;
    public int effectCode;
    public DurationType durationType;
    public int maxStacks;
    public int DecayValue;
    public int effectOrder;
    public Sprite effectIcon;

    public abstract void Execute(GameObject caster, GameObject target, int amount);
}
