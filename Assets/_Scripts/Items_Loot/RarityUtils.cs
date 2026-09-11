using UnityEngine;

public static class RarityUtils
{
    public static Color GetColor(ItemData.Rarity rarity)
    {
        switch (rarity)
        {
            case ItemData.Rarity.Rare: return new Color(0.2f, 0.5f, 1f);
            case ItemData.Rarity.Epic: return new Color(0.6f, 0f, 1f);
            case ItemData.Rarity.Legendary: return new Color(1f, 0.5f, 0f);
            default: return Color.white; // Common
        }
    }
}