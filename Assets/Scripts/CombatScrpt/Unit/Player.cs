using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBase
{
    public List<WeaponData> ownedWeapons = new List<WeaponData>();
    private void Awake()
    {
        base.Awake();
        Health = GameManager.Instance.playerCurrentHp;
        ownedWeapons = new List<WeaponData>(GameManager.Instance.weaponDatas);
    }

}
