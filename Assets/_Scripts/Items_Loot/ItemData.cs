using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public enum Rarity { Common, Rare, Epic, Legendary }

    public string ItemName;
    public bool isUsable;
    public bool isEquppable;
    public EquipmentSlot slot;
    public List<StatModifier> statModifiers;
    public Sprite icon;
    public Rarity rarity; 
}