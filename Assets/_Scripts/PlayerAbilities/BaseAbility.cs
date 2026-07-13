using UnityEngine;
using System;
using System.Collections;

public class BaseAbility : MonoBehaviour
{
    public event Action<float> OnCooldownChanged;
    [SerializeField] protected float manaCost;
    protected Mana _mana;
    protected bool isoncooldown;
    [SerializeField] protected float cooldown = 5f;

    void Awake()
    {
        _mana = GetComponent<Mana>();
    }
    protected IEnumerator CooldownBase()
    {
        isoncooldown = true;
        float elapsed = 0f;
        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            float cooldownresult = elapsed / cooldown;
            OnCooldownChanged?.Invoke(cooldownresult);
            yield return null;
        }
        isoncooldown = false;
        OnCooldownChanged?.Invoke(1f);
    }
}
