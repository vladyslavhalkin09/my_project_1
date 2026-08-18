using System.Collections;
using UnityEngine;
using System;

public class MultishotAbility : BaseAbility
{
    [Header("Settings")]
    public int bulletsamount;
    public int wavesamount;

    public GameObject bulletprefab;
    public float timebetweenwaves;
    public Transform attackpoint;
    public float bulletsangle;
    public event Action<float, string> OnCastChanged;
    public event Action OnCastFinished;
    public float damageCoefficient = 1.2f;
    private WeaponDamageCalculator _calculator;
    protected override void Awake()
    {
        base.Awake();
        _calculator = GetComponent<WeaponDamageCalculator>();
    }

    public void TryUseMultishot(Vector3 targetPoint)
    {
        if (isoncooldown) return;
        if (!_mana.TryConsume(manaCost)) return;
        StartCoroutine(FireWaves(targetPoint));
        StartCoroutine(CooldownBase());
    }


    IEnumerator FireWaves(Vector3 targetPoint)
    {
        float totalTime = wavesamount * timebetweenwaves;
        float totalElapsed = 0f;
        for (int waves = 0; waves < wavesamount; waves++)
        {
            float halfangle = bulletsangle / 2;
            float angleStep = bulletsangle / (bulletsamount - 1);
            float distanceToPoint = Vector3.Distance(attackpoint.position, targetPoint);
            Vector3 direction = (targetPoint - attackpoint.position).normalized;
            for (int bullet = 0; bullet < bulletsamount; bullet++)
            {
                float currentAngle = -halfangle + bullet * angleStep;
                Vector3 rotatedDirection = Quaternion.Euler(0, currentAngle, 0) * direction;
                GameObject newBullet = Instantiate(bulletprefab, attackpoint.position, attackpoint.rotation);
                Bullet bulletScript = newBullet.GetComponent<Bullet>();
                bulletScript.damageType = DamageType.Ability;

                if (bulletScript != null)
                {
                    bulletScript.bullethitpoint = attackpoint.position + rotatedDirection * distanceToPoint;
                    bulletScript.maxDistance = distanceToPoint;
                    bulletScript.bulletdamage = _calculator.GetAbilityDamageMult(damageCoefficient);
                }
            }
            float elapsed = 0f;
            while (elapsed < timebetweenwaves)
            {
                elapsed += Time.deltaTime;
                totalElapsed += Time.deltaTime;
                float progress = totalElapsed / totalTime;
                OnCastChanged?.Invoke(progress, "Multishot");
                yield return null;
            }
        }
        OnCastFinished?.Invoke();
    }
}
