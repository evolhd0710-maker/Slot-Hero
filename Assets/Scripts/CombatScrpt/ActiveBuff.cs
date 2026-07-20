using UnityEngine;
[System.Serializable]
public class ActiveBuff
{
    public EffectSO blueprint; // CSV로 만든 Effect 에셋 (설계도)
    public int currentStacks;  // 현재 중첩 수
    public int duration;       // 남은 지속 시간 (TurnBased일 때 사용)

    // 이 버프를 누가 걸었는지 (데미지 계산 시 시전자 스탯이 필요할 수 있음)
    public GameObject caster;

    public ActiveBuff(EffectSO blueprint, GameObject caster, int startStacks, int duration)
    {
        this.blueprint = blueprint;
        this.caster = caster;
        this.currentStacks = startStacks;
        this.duration = duration;
    }
}
