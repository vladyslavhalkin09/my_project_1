using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class WeaponDamageCalculator : MonoBehaviour
{
    private PlayerEquipment _equipment;
    private CharacterStatsHolder _stats;

    private void Awake()
    {
        _equipment = GetComponent<PlayerEquipment>();
        _stats = GetComponent<CharacterStatsHolder>();
    }
    public float GetAutoAttackDamage()
    {
        WeaponData weapon = _equipment.GetCurrentWeapon();
        float damageRoll = Random.Range(weapon.minDamage, weapon.maxDamage);
        float finalDamage = damageRoll + _stats.Stats.GetStat(StatType.MainStat) * weapon.statsMultiplier;
        return finalDamage;
    }
    public float GetAbilityDamageMult(float cofficient)
    {
        return GetAutoAttackDamage() * cofficient;
    }
}
