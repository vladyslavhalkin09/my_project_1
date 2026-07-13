using UnityEngine;

public class SlowModifier : MonoBehaviour, IAttackModifier
{
    public float slowMultiplier;
    public float slowDuration;
    public void Apply(Bullet bullet)
    {
        bullet.slowDuration = slowDuration;
        bullet.slowMultiplier = slowMultiplier;
    }

}
