using System;
using UnityEngine;

public class PowerShotAbility : BaseAbility
{

    public bool hasHitFirstEnemy;
    public float maxCoefficient;
    public float minCoefficient;
    public float PowerShotCastTime;
    public GameObject ArrowPrefab;
    public float MaxChargeTime = 3f;
    public float ChargeTime;
    public Transform attackpoint;
    [Tooltip("Fixed travel distance, independent of click distance — like Windranger's Powershot. Should exceed the base attack's maxRange (currently 15).")]
    public float range = 20f;
    public event Action<float, string> OnCastChanged;
    public event Action OnCastFinished;
    private WeaponDamageCalculator _calculator;
    protected override void Awake()
    {
        base.Awake();
        _calculator = GetComponent<WeaponDamageCalculator>();
    }

    public void TryUsePowerShot(Vector3 targetPoint)
    {
        if (isoncooldown) return;
        if (!_mana.TryConsume(manaCost)) return;
        float coefficient = Mathf.Lerp(minCoefficient, maxCoefficient, ChargeTime / MaxChargeTime);
        float damage = _calculator.GetAbilityDamageMult(coefficient);
        ChargeTime = 0f;
        OnCastFinished?.Invoke();
        Vector3 direction = (targetPoint - attackpoint.position).normalized;
        direction.y = 0;
        direction = direction.normalized;
        float travelDistance = range;
        if (Physics.Linecast(attackpoint.position, attackpoint.position + direction * range, out RaycastHit wallHit, obstacleLayer))
        {
            travelDistance = wallHit.distance;
        }
        GameObject newPowerShotArrow = Instantiate(ArrowPrefab, attackpoint.position, attackpoint.rotation);
        PowerShotArrow PSScript = newPowerShotArrow.GetComponent<PowerShotArrow>();
        if (PSScript != null)
        {
            PSScript.ArrowHitpoint = targetPoint;
            PSScript.maxDistance = travelDistance;
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