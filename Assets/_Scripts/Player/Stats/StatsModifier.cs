using UnityEngine;
[System.Serializable]
public class StatModifier : IStatModifier
{
    [SerializeField] private StatType affectedStat;
    [SerializeField] private float value;

    public StatType AffectedStat => affectedStat;
    public float Value => value;

    public StatModifier(StatType affectedStat, float value)
    {
        this.affectedStat = affectedStat;
        this.value = value;
    }
}