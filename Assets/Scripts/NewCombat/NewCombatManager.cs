using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewCombatManager : MonoBehaviour
{
    public NewSlotManager slotManager;

    public NewPlayer player;
    public NewEnemy enemy;

    public Slider playerHp;
    public Slider enemyHp;

    public GameObject totalDamageText;
    private void Awake()
    {
        player.Setup();
        enemy.Setup();
        totalDamageText.SetActive(false);
    }
    private IEnumerator PlayerTurnStart()
    {
        totalDamageText.SetActive(true);
        TurnContext turnContext = new TurnContext(); ;
        Dictionary<Symbol, int> symbolCounts = new Dictionary<Symbol, int>();
        slotManager.RollWrapper();
        yield return new WaitUntil(() => slotManager.isRollEnd);

        for(int i = 0; i < slotManager.RolledResults.Count; i++)
        {
            Symbol s = slotManager.RolledResults[i];

            if (symbolCounts.ContainsKey(s))
            {
                symbolCounts[s]++;
            }
            else
            {
                symbolCounts[s] = 1;
            }

            RectTransform currentReelUI = slotManager.reelImages[i].GetComponent<RectTransform>();
            yield return StartCoroutine(s.Execute(player, enemy, turnContext, symbolCounts[s], currentReelUI));
            yield return new WaitForSeconds(0.2f);
        }
        enemy.TakeDamage(turnContext.totalDamage, "½½·Ô ±¼¸²");
        player.AddShield(turnContext.totalDefense);
        totalDamageText.SetActive(false);
    }

    public void CheckHp() 
    {
        if (playerHp != null)
            playerHp.value = (float)player.CurrentHealth / (float)player.data.maxHealth;
        if (enemyHp != null)
            enemyHp.value = (float)enemy.CurrentHealth / (float)enemy.data.maxHealth;
    }

    public void CheckShield()
    {

    }

    public void TurnStart()
    {
        StartCoroutine(PlayerTurnStart());
    }

}
