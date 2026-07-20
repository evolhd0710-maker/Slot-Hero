
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum SymbolTag
{
    제국,
    도검,
    신전,
    고대,
    둔기,
    방패,
    상징
}

public enum SymbolType
{
    글라디우스,
    사이포스,
    실드,
    몽둥이,
    스쿠툼,
    조각상
}

[CreateAssetMenu(fileName = "NewSymbol", menuName = "Symbol")]
public class Symbol : ScriptableObject
{
    [Header("기본 정보")]
    public SymbolType symbolType;
    public Sprite symbolSprite;
    public List<SymbolTag> symbolTags;

    [Header("무기 고유 스탯")]
    public int baseAttack;
    public int baseDefense;

    [Header("이 무기가 발동할 효과들")]
    public List<SymbolEffect> basicEffects;
    public List<SymbolEffect> specialEffects;


    public IEnumerator Execute(NewPlayer player, NewEnemy enemy, TurnContext context, int countInTurn, RectTransform targetUI)
    {
        int damageSnapshot = context.totalDamage;
        int defenseSnapshot = context.totalDefense;

        foreach (var effect in specialEffects)
        {
            if (targetUI != null)
            {
                UIFXManager.Instance.StartCoroutine(Co_PlayPulseAnimation(targetUI, 0.3f, 1.3f));
            }
            effect.Apply(player, enemy, context, this, countInTurn);
            yield return new WaitForSeconds(0.3f);
        }

        foreach (var effect in basicEffects)
        {
            if (targetUI != null)
            {
                UIFXManager.Instance.StartCoroutine(Co_PlayPulseAnimation(targetUI, 0.3f, 1.3f));
            }
            effect.Apply(player, enemy, context, this, countInTurn);
            yield return new WaitForSeconds(0.3f);
        }

        int finalFlyDamage = context.totalDamage - damageSnapshot;
        int finalFlyDefense = context.totalDefense - defenseSnapshot;

        if (finalFlyDamage > 0 && targetUI != null)
        {
            yield return UIFXManager.Instance.Co_FlyNumber(
                finalFlyDamage,
                targetUI.position,
                UIFXManager.Instance.totalDamageTargetUI,
                Color.red,
                UIFXManager.Instance.totalDamageText,
                context.totalDamage // 미래 수치로 현재 상태를 그대로 주입
            );
        }

        if (finalFlyDefense > 0 && targetUI != null)
        {
            yield return UIFXManager.Instance.Co_FlyNumber(
                finalFlyDefense,
                targetUI.position,
                UIFXManager.Instance.totalShieldTargetUI,
                Color.cyan,
                UIFXManager.Instance.totalShieldText,
                context.totalDefense
            );
        }
    }

    private IEnumerator Co_PlayPulseAnimation(RectTransform uiTransform, float duration, float maxScale)
    {
        float elapsedTime = 0f;
        Vector3 originScale = Vector3.one;
        Vector3 targetScale = Vector3.one * maxScale;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float pulseProgress = Mathf.Sin(t * Mathf.PI);
            uiTransform.localScale = originScale + (targetScale - originScale) * pulseProgress;
            yield return null;
        }
        uiTransform.localScale = originScale;
    }
}

