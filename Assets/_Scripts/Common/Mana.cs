using UnityEngine;

public class Mana : Resource
{
    public float regenPerSecond;
    public CharacterStatsHolder _stats;
    void Start()
    {
        _stats = GetComponent<CharacterStatsHolder>();
        _stats.Stats.OnStatsChanged += HandleStatsChanged;
        HandleStatsChanged();

    }
    void Update()
    {
        if (currentValue < maxValue)
        {
            Restore(regenPerSecond * Time.deltaTime);
        }
    }
    public bool TryConsume(float amount)
    {
        if (currentValue < amount) return false;
        Spend(amount);
        return true;
    }

    public bool HasEnough(float amount)
    {
        return currentValue >= amount;
    }
    private void HandleStatsChanged()
    {
        SetMaxValue(_stats.Stats.GetStat(StatType.Intelligence));
    }
    void OnDestroy()
    {
        _stats.Stats.OnStatsChanged -= HandleStatsChanged;
    }
}
