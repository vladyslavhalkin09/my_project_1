using UnityEngine;
using System;

public class Resource : MonoBehaviour
{
    public float currentValue;
    public float maxValue;
    public event Action<float> OnResourceChanged;
    public virtual void Spend(float amount)
    {
        currentValue -= amount;
        if (currentValue <= 0)
        {
            currentValue = 0;
        }
        UpdateUi();
    }
    public virtual void Restore(float amount)
    {
        currentValue += amount;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
        UpdateUi();
    }
    protected virtual void UpdateUi()
    {
        OnResourceChanged?.Invoke(currentValue / maxValue);
    }
}
