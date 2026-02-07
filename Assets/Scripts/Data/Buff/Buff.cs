using UnityEngine;

public class Buff
{
    public BuffData data;
    public int currentMagnitude;
    public int remainTime;

    public Buff(BuffData data, int magnitude, int duration)
    {
        this.data = data;
        this.currentMagnitude = magnitude;
        this.remainTime = duration;
    }   
}
