using UnityEngine;

[RequireComponent(typeof(CharacterStatsHolder))]
public class EnemyStatsHolder : MonoBehaviour
{
    private CharacterStatsHolder _baseStats;
    private EnemyBase _enemyBase;

    public CharacterStats Stats => _baseStats.Stats;

    private void Awake()
    {
        _baseStats = GetComponent<CharacterStatsHolder>();
        _enemyBase = GetComponent<EnemyBase>();
    }

    public float GetAttackDamage()
    {
        EnemyData data = _enemyBase != null ? _enemyBase.data : null;
        if (data == null)
        {
            Debug.LogWarning(gameObject.name + ": EnemyStatsHolder has no EnemyData to roll damage from.");
            return 0f;
        }

        float damageRoll = Random.Range(data.minDamage, data.maxDamage);
        float statBonus = _baseStats.Stats.GetStat(StatType.MainStat) * data.statMultiplier;
        return damageRoll + statBonus;
    }
}