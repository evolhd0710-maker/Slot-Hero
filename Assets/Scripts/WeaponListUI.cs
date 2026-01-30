using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//무기 리스트 출력하는 UI 관리용 코드 
public class WeaponListUI : MonoBehaviour
{
    public Player player;
    public CombatManager combatManager;
    public Transform contentParent;
    public GameObject weaponButtonPrefab;
    private List<GameObject> spawnedButtons = new List<GameObject>();
    public void ToggleWeaponList()
    {
        ClearButtons();
        int index = 0;
        foreach(WeaponData weapon in player.ownedWeapons)
        {
            GameObject btn = Instantiate(weaponButtonPrefab, contentParent);
            spawnedButtons.Add(btn);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{index++}";
            btn.GetComponent<Button>().onClick.AddListener(() => OnWeaponSelected(weapon));

        }
    }

    void OnWeaponSelected(WeaponData weapon)
    {
        combatManager.EquipWeapon(weapon);
        Debug.Log($"{weapon.weaponName}을(를) 장착했습니다!");
    }

    void ClearButtons()
    {
        foreach (var btn in spawnedButtons) Destroy(btn);
            spawnedButtons.Clear();
    }
}
