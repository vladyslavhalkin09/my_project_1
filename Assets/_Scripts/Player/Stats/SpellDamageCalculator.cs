using UnityEngine;

public class SpellDamageCalculator : MonoBehaviour
{
    private CharacterStatsHolder _stats;

    private void Awake()
    {
        _stats = GetComponent<CharacterStatsHolder>();
    }

    public float GetSpellDamage(float baseDamage, float statCoefficient, float variancePercent = 0f)
    {
        float baseResult = baseDamage + _stats.Stats.GetStat(StatType.MainStat) * statCoefficient;
        float variance = Random.Range(-variancePercent, variancePercent);
        return baseResult * (1f + variance);
    }
}