using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class NewUnitBase :MonoBehaviour
{
    private int currentHealth;
    private int shield;
    public CharacterData data;

    public event Action<int, int> OnHpChanged; 
    public event Action<int> OnShieldChanged;

    public int CurrentHealth
    {
        get { return currentHealth; }
        // 자식 클래스에서도 안전하게 접근할 수 있도록 protected set 유지
        protected set
        {
            // 체력이 0보다 작아지거나 Max를 넘지 않게 제어
            currentHealth = Mathf.Clamp(value, 0, data.maxHealth);
            OnHpChanged?.Invoke(currentHealth, data.maxHealth);
        }
    }

    public int CurrentShield => shield;

    public virtual void Setup()
    {
        CurrentHealth = data.maxHealth;
        shield = 0;
        OnShieldChanged?.Invoke(shield);
    }

    public virtual void TakeDamage(int damage, string reason)
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
            OnShieldChanged?.Invoke(shield);
        }

        CurrentHealth -= damage;

        print($"{data.name}에 {damage} 데미지 부여 남은체력 {CurrentHealth} : {reason}");
    }

    public virtual void AddShield(int num)
    {
        shield += num;
        OnShieldChanged?.Invoke(shield);
        print($"{data.name}에 {num} 실드 부여 남은 실드 {shield}");
    }
}