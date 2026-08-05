using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponData : ItemData
{
    public float minDamage;
    public float maxDamage;
    public float statsMultiplier = 1f;
}
