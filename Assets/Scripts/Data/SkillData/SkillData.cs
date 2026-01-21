using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public int skillId;
    public int skillSort; //공격 0 수비 1 버프 2
    public string skillName;
    public string formula; // 계산 공식 a + b + c 꼴
    // 계산방식 의문점 슬롯은 계속 늘어나는데 그럼 계산 공식은 어떻게 되는거죠
}
