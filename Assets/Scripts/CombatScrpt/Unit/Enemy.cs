using System.Collections;
using UnityEngine;

public class Enemy : UnitBase
{
    //아직 주어진 적 자료가 없어서 임시로 할당함

    public string enemyName;
    public int weapon;
    public int slotCount;
    public int[] slotNums;
    public SkillData[] skills;
    private void Awake()
    {
        base.Awake();
    }

}
