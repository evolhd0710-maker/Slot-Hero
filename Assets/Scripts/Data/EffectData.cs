using UnityEngine;

public class EffectData 
{
    //언제 발동할지
    public enum EffectTriggerType
    { 
        OnTurnStart, OnTurnEnd, OnTakeDamage, OnDealDamage
    }
    //스택 감소 방식
    public enum EffectDecreaseType
    {
        None, MinusOne, Half, All
    }
    //스택 감소 시기 
    public enum DecreaseTime
    {
        WhenTurnEnd
    }
    public EffectTriggerType triggerType;
    public int effectId;
    public string effectName;
    public int magnitude;
    public int remainTime;
    public int decreaseNum;
    

    public virtual void OnTriggered()
    {

    }
}
