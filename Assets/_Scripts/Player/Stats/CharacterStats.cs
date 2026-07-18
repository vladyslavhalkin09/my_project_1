using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    [Header("Base Stats (from level)")]
    [SerializeField] private float baseHealth;
    [SerializeField] private float baseMainStat;
    [SerializeField] private float baseIntelligence;
    [SerializeField] private float baseArmor;

    private List<IStatModifier> modifiers = new List<IStatModifier>();

    public event Action OnStatsChanged;

    public float GetStat(StatType type)
    {
        float baseValue = type switch
        {
            StatType.Health => baseHealth,
            StatType.Armor => baseArmor,
            StatType.Intelligence => baseIntelligence,
            StatType.MainStat => baseMainStat,
            _ => 0f
        };
        float bonus = 0f;

        foreach (var modifier in modifiers)
        {
            if (modifier.AffectedStat == type)
            {
                bonus += modifier.Value;
            }
        }
        return baseValue + bonus;
    }

    public void AddModifier(IStatModifier modifier)
    {
        modifiers.Add(modifier);
        OnStatsChanged?.Invoke();
    }


    public void RemoveModifier(IStatModifier modifier)
    {

        if (modifiers.Remove(modifier))
        {
            OnStatsChanged?.Invoke();
        }
    }
}