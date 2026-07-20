using UnityEngine;

public class SkillTester : MonoBehaviour
{
    public SkillSO testSkill;
    public int[] testSlotValue;
    public GameObject caster, target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        testSlotValue = new int[5]{1,2,3,4,5};
        testSkill.UseSkill(caster, target, testSlotValue);
        Debug.Log("UseSkill");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
