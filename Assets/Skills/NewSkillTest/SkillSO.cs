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
        public SkillEffect effectBlueprint;
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
                Debug.Log(finalAmount);
                
                GameObject realTarget = target;
                if (container.targetType == EffectTargetType.Caster)
                {
                    realTarget = caster; 
                }

               
                container.effectBlueprint.Execute(caster, realTarget, finalAmount);
            }
            else
            {
                Debug.LogWarning($"[{skillName}] 스킬 내부의 특정 효과 부품(Blueprint)이 널(Null) 상태입니다.");
            }
        }

        Debug.Log($"[{skillName}] 스킬 모든 효과 실행 완료.");
    }
}