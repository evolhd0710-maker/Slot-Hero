using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EffectTargetType
{
    Target, 
    Caster  
}

public enum Trait
{
    // 트레잇 나중에 추가한다니까 일단 넣어놓기 
}

[CreateAssetMenu(fileName = "SkillSO", menuName = "Scriptable Objects/SkillSO")]
public class SkillSO : ScriptableObject
{
    [System.Serializable]
    public struct EffectContainer
    {
        public EffectSO effectBlueprint;
        public FlexValue flexAmount;
        public int chance;
        public EffectTargetType targetType;
        public string effectName;
    }

    [Header("Skill Base Data")]
    public int skillId;
    public string skillName;
    public float[] coEff;
    public Sprite skillIcon;
    [TextArea(3, 5)] public string description;
    public string animation;
    public EffectTargetType effectTargetType;

    [Header("Assembled Effects")]
    public List<EffectContainer> effects = new List<EffectContainer>();

    public void UseSkill(GameObject caster, GameObject target, int[] slotValues)
    {
        if (effects == null || effects.Count == 0)
        {
            Debug.LogWarning($"[{skillName}] 스킬에 장착된 효과 블록이 없습니다.");
            return;
        }

        float slotSum = 0;
        if (coEff != null && slotValues != null)
        {
            int loopCount = Mathf.Min(coEff.Length, slotValues.Length);
            for (int i = 0; i < loopCount; i++)
            {
                slotSum += slotValues[i] * coEff[i];
            }
        }

        foreach (EffectContainer container in effects)
        {
            if (container.effectBlueprint != null)
            {

                int finalAmount = (int)container.flexAmount.ResolveValue(coEff, slotValues);                
                GameObject realTarget = target;
                if (container.targetType == EffectTargetType.Caster)
                {
                    realTarget = caster; 
                }

               switch (container.effectBlueprint.effectClassType)
                {
                    case EffectClassType.InstantDamage:
                        // 1. 즉발 데미지는 버프 핸들러 없이 그 자리에서 즉시 진짜 행동(Execute)을 시킵니다.
                        container.effectBlueprint.Execute(caster, realTarget, finalAmount);
                        break;

                    case EffectClassType.DotDamage:
                    case EffectClassType.StatBuff:
                        // 2. 도트 데미지나 상시 스탯 버프는 BuffHandler를 찾아 등록만 해줍니다.
                        BuffHandler handler = realTarget.GetComponent<BuffHandler>();
                        if (handler != null)
                        {
                            int duration = container.flexAmount.constantValue; 
                            handler.AddBuff(container.effectBlueprint, caster, finalAmount, duration);
                        }
                        else
                        {
                            Debug.LogWarning($"{realTarget.name}에게 BuffHandler 컴포넌트가 없습니다! 효과를 등록할 수 없습니다.");
                        }
                        break;
                }
            }
            else
            {
                Debug.LogWarning($"[{skillName}] 스킬 내부의 특정 효과 부품(Blueprint)이 널(Null) 상태입니다.");
            }
        }

        Debug.Log($"[{skillName}] 스킬 모든 효과 실행 완료.");
    }
}