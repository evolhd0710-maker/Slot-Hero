using UnityEngine;
public enum VariableType
{
    Constant,        
    SlotValue
}
[System.Serializable]
public struct FlexValue
{
    public VariableType valueType;
    public int constantValue;

    public float ResolveValue(float[] coEff, int[] slotValues)
    {
        if (valueType == VariableType.Constant)
        {
            return constantValue;
        }

        switch (valueType)
        {
            case VariableType.SlotValue:
                float slotSum = 0;
                for (int i = 0; i < coEff.Length; i++)
                {
                    slotSum += slotValues[i] * coEff[i];
                }
                return slotSum;
            default:
                Debug.Log("µðÆúÆ®");
                return 0;
        }
    }
}
