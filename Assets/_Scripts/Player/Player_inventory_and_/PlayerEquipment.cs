using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerEquipment : MonoBehaviour
{
    public WeaponData testWeapon;
    private CharacterStatsHolder _stats;
    public WeaponData fallbackWeapon;
    private Dictionary<EquipmentSlot, ItemData> equippedItems = new Dictionary<EquipmentSlot, ItemData>();
    private Dictionary<EquipmentSlot, System.Type> slotTypeRequirements = new Dictionary<EquipmentSlot, System.Type>

{
    { EquipmentSlot.Weapon, typeof(WeaponData) }
};
    public event Action<EquipmentSlot> OnEquipmentChanged;
    void Start()
    {
        _stats = GetComponent<CharacterStatsHolder>();
    }
    public ItemData Equip(ItemData item)
    {
        ItemData previousItem = null;
        if (slotTypeRequirements.TryGetValue(item.slot, out System.Type requiredType))
        {
            if (!requiredType.IsInstanceOfType(item))
            {
                Debug.LogError($"{item.name} has slot {item.slot} but is not a {requiredType.Name}!");
                return null;
            }
        }

        if (equippedItems.TryGetValue(item.slot, out ItemData oldItem))
        {
            foreach (var modifier in oldItem.statModifiers)
            {
                _stats.Stats.RemoveModifier(modifier);
            }
            previousItem = oldItem;
        }
        equippedItems[item.slot] = item;
        foreach (var modifier in item.statModifiers)
        {
            _stats.Stats.AddModifier(modifier);
        }
        OnEquipmentChanged?.Invoke(item.slot);
        return previousItem;


    }
    public ItemData Unequip(EquipmentSlot slot)
    {
        ItemData previousItem = null;
        if (equippedItems.TryGetValue(slot, out ItemData oldItem))
        {
            foreach (var modifier in oldItem.statModifiers)
            {
                _stats.Stats.RemoveModifier(modifier);
            }
            previousItem = oldItem;
        }
        equippedItems.Remove(slot);
        OnEquipmentChanged?.Invoke(slot);
        return previousItem;
    }
    public WeaponData GetCurrentWeapon()
    {
        if (equippedItems.TryGetValue(EquipmentSlot.Weapon, out ItemData item))
        {
            WeaponData weapon = item as WeaponData;
            if (weapon != null)
            {
                return weapon;
            }
            return fallbackWeapon;
        }
        else
        {
            return fallbackWeapon;
        }
    }
    [ContextMenu("Test Equip Weapon")]
    private void TestEquipWeapon()
    {
        Equip(testWeapon);
    }
}
