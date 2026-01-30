using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBase
{
    public List<WeaponData> ownedWeapons = new List<WeaponData>();

    private void Start()
    {
        ownedWeapons = new List<WeaponData>(GameManager.Instance.weaponDatas);
    }
    public override IEnumerator ExecuteTurn()
    {
        yield return null;
    }
}
