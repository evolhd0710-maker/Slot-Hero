using System.Collections;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public CharacterData data;
    protected int currentHealth;

    public virtual void Setup()
    {
        currentHealth = data.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{data.unitName}이(가) {damage}의 피해를 입음! 남은 체력: {currentHealth}");

        if (currentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        Debug.Log($"{data.unitName} 사망");
    }
    public abstract IEnumerator ExecuteTurn();

}
