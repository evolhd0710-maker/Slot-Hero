using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int weaponId;
    public Sprite weaponSprite;
    public List<SkillData> skills = new List<SkillData>();
    public AnimatorOverrideController weaponAnimatorController;
}
