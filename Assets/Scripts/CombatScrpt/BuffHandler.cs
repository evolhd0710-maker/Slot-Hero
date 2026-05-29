using System.Collections.Generic;
using UnityEngine;

public class BuffHandler : MonoBehaviour
{
    // 이 캐릭터에게 현재 걸려있는 버프/디버프 목록
    public List<ActiveBuff> activeBuffs = new List<ActiveBuff>();

    private void OnEnable()
    {
        // CombatManager의 전역 이벤트를 구독 (도청 시작)
        CombatManager.OnTickTimingTriggered += HandleTickTiming;
        CombatManager.OnDecayTimingTriggered += HandleDecayTiming;
    }

    private void OnDisable()
    {
        // 메모리 누수 방지를 위해 해제 시 구독 취소
        CombatManager.OnTickTimingTriggered -= HandleTickTiming;
        CombatManager.OnDecayTimingTriggered -= HandleDecayTiming;
    }

    // 🟢 1단계: 스킬의 Execute 등을 통해 버프가 등록되는 메서드
    public void AddBuff(EffectSO newEffect, GameObject caster, int amount, int duration)
    {
        // 이미 똑같은 효과 코드가 걸려있는지 확인 (예: 이미 독 상태인가?)
        ActiveBuff existingBuff = activeBuffs.Find(b => b.blueprint.effectCode == newEffect.effectCode);

        if (existingBuff != null)
        {
            // [중첩 처리 방식 규칙 적용]
            switch (newEffect.stackType)
            {
                case StackType.Duration: // 지속 시간만 연장됨 (슬더스 상취 등)
                    existingBuff.duration += duration;
                    break;

                case StackType.Intensity: // 스택(위력)이 늘어남 (슬더스 독 등)
                    existingBuff.currentStacks = Mathf.Min(existingBuff.currentStacks + amount , newEffect.maxStacks);
                    // 스택형 디버프는 보통 들어올 때 지속 시간도 새로고침 됨
                    existingBuff.duration = duration;
                    break;

                case StackType.None:
                    // 중첩 불가능하면 시간만 갱신하거나 무시
                    existingBuff.duration = duration;
                    break;
            }
        }
        else
        {
            // 새로 걸리는 버프라면 리스트에 추가
            activeBuffs.Add(new ActiveBuff(newEffect, caster, amount, duration));
        }

        Debug.Log($"{gameObject.name}에게 [{newEffect.effectCode}] 효과 등록됨.");
    }

    // 🟢 2단계: 알맞은 타이밍에 효과 행동(Tick) 실행
    private void HandleTickTiming(GameObject activeCharacter, TickTiming timing)
    {
        if (activeCharacter != gameObject) return;

        if (timing == TickTiming.None) return;

        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            ActiveBuff buff = activeBuffs[i];

            if (buff.blueprint.tickTiming == timing)
            {
                // 내 턴 시작이 확실하므로 독 데미지 등을 실행
                buff.blueprint.Execute(buff.caster, gameObject, buff.currentStacks);
            }
        }
    }

    // 🟢 3단계: 알맞은 타이밍에 수치 감소(Decay) 및 삭제 처리
    private void HandleDecayTiming(GameObject activeCharacter, DecayTiming timing)
    {
        if (activeCharacter != gameObject) return;


        if (timing == DecayTiming.None) return;

        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            ActiveBuff buff = activeBuffs[i];

            if (buff.blueprint.decayTiming == timing)
            {
                // [지속 시간 방식 처리]
                if (buff.blueprint.durationType == DurationType.TurnBased)
                {
                    buff.duration--; // 1턴 감소
                }

                // [스택 감소 형태 처리]
                switch (buff.blueprint.decayType)
                {
                    case DecayType.Constant: // 매 턴 정해진 수치만큼 스택이 까임 (예: 매턴 독 -1)
                        buff.currentStacks -= buff.blueprint.decayValue;
                        break;

                    case DecayType.Percentage: // 매 턴 퍼센트로 까임
                        buff.currentStacks = Mathf.RoundToInt(buff.currentStacks * (1f - (buff.blueprint.decayValue / 100f)));
                        break;
                }

                // [삭제 조건 검사] 지속 시간이 다 되거나 스택이 0 이하가 되면 삭제
                if (buff.duration <= 0 || buff.currentStacks <= 0)
                {
                    Debug.Log($"{gameObject.name}의 [{buff.blueprint.effectCode}] 효과가 만료되어 사라졌습니다.");
                    activeBuffs.RemoveAt(i);
                    // 필요 시 여기서 스탯 원상복구 로직 호출
                }
            }
        }
    }
}
