using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public int skillId;
    public int skillSort; //공격 0 수비 1 버프 2
    public string skillName;
    public int[] useSlotIndices;
    public string animationTrigger;

    public int CalculateNumber(int[] slotValues)
    {
        int power = 0;  
        foreach(int index in useSlotIndices)
        {
            if (index < slotValues.Length)
                power += slotValues[index];
        }
        return power;
    }
}
