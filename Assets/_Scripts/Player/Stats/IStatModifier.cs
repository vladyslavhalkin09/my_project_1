public interface IStatModifier
{
    StatType AffectedStat { get; }
    float Value { get; }
}