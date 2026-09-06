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
        float mainStatValue = _stats.Stats.GetStat(StatType.MainStat);
        float baseResult = baseDamage + mainStatValue * statCoefficient;
        float variance = Random.Range(-variancePercent, variancePercent);
        float result = baseResult * (1f + variance);
        Debug.Log($"[SpellDmg] MainStat={mainStatValue}, base={baseDamage}, coeff={statCoefficient}, variance%={variancePercent}, result={result}");
        return result;
    }
}