using UnityEngine;


public enum BuffType
{
    AttackBoost,   // 공격력 증가
    DefenseBoost,  // 방어력 증가
    HealthRegen    // 매 턴 체력 회복
}

[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Objects/BuffData")]
public class BuffData : ScriptableObject
{
    public string buffName;
    public BuffType type;
    public int defaultMagnitude;
    public int defaultDuration;
    public Sprite icon;
}
