using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public CharacterData data;
    [SerializeField] private int currentHealth;
    protected int shield;
    public List<Buff> activeBuffs = new List<Buff>();

    public int Health
    {
        get { return currentHealth; }
        protected set
        {
            // 체력이 0보다 작아지거나 Max를 넘지 않게 제어
             currentHealth = Mathf.Clamp(value, 0, data.maxHealth);
        }
    }

    public virtual void Setup()
    {
        Health = data.maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (shield > 0)
        {
            if (shield >= damage)
            {
                shield -= damage;
                damage = 0;
            }
            else
            {
                damage -= shield;
                shield = 0;
            }
        }

        Health -= damage;
        print($"{data.name}에 {damage} 데미지 부여 남은체력 {Health}");
    }

    public virtual void AddShield(int num)
    {
        shield = 0;
        shield += num;
        print($"{data.name}에 {shield} 실드 부여");
    }

    public virtual void AddBuff(Buff buff)
    {
        activeBuffs.Add(buff);
    }
    

    protected virtual void Die()
    {
        Debug.Log($"{data.unitName} 사망");
    }

}
