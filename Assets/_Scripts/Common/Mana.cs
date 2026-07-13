using UnityEngine;

public class Mana : Resource
{
    public float regenPerSecond;
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
}
