using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private CharacterStatsHolder _stats;
    private Dictionary<EquipmentSlot, ItemData> equippedItems = new Dictionary<EquipmentSlot, ItemData>();
    void Start()
    {
        _stats = GetComponent<CharacterStatsHolder>();
    }
    public ItemData Equip(ItemData item)
    {
        ItemData previousItem = null;
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
        return previousItem;
    }

}
