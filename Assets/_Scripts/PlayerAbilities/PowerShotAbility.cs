
using System;
using UnityEngine;

public class PowerShotAbility : BaseAbility
{

    public bool hasHitFirstEnemy;
    public float PowerShotDamage;
    public float BaseDamage;
    public float PowerShotCastTime;
    public GameObject ArrowPrefab;
    public float MaxChargeTime = 3f;
    public float ChargeTime;
    public Transform attackpoint;
    public event Action<float, string> OnCastChanged;
    public event Action OnCastFinished;

    public void TryUsePowerShot(Vector3 targetPoint)
    {
        if (isoncooldown) return;
        if (!_mana.TryConsume(manaCost)) return;
        float damage = Mathf.Round(Mathf.Lerp(BaseDamage, PowerShotDamage, ChargeTime / MaxChargeTime));
        ChargeTime = 0f;
        OnCastFinished?.Invoke();
        GameObject newPowerShotArrow = Instantiate(ArrowPrefab, attackpoint.position, attackpoint.rotation);
        PowerShotArrow PSScript = newPowerShotArrow.GetComponent<PowerShotArrow>();
        if (PSScript != null)
        {
            PSScript.ArrowHitpoint = targetPoint;
            PSScript.PowerShotArrowDamage = damage;
        }
        StartCoroutine(CooldownBase());

    }
    public void PowerShotCharge(Vector3 targetPoint)
    {
        if (isoncooldown) return;
        if (!_mana.HasEnough(manaCost)) return;
        ChargeTime += Time.deltaTime;
        ChargeTime = Mathf.Clamp(ChargeTime, 0f, MaxChargeTime);
        float progress = ChargeTime / MaxChargeTime;
        OnCastChanged?.Invoke(progress, "PowerShot");
        if (ChargeTime >= MaxChargeTime)
        {
            TryUsePowerShot(targetPoint);
        }

    }
}

