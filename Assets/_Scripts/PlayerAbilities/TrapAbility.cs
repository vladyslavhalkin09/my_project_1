using System.Collections;
using UnityEngine;

public class TrapAbility : BaseAbility
{
    [Header("Settings")]
    public GameObject trapPrefab;
    public GameObject trapZonePrefab;
    public float throwRange;
    public Transform attackpoint;
    public float trapBaseDamage;
    public float trapRadius;
    public float trapDuration;
    public float trapSlowMultiplier;
    public float traplifetimebeforeactive;
    [SerializeField] private float damageVariance = 0.05f;
    private SpellDamageCalculator _spellCalculator;
    public float statCoefficient = 1f;
    protected override void Awake()
    {
        base.Awake();
        _spellCalculator = GetComponent<SpellDamageCalculator>();
        Debug.Log($"SpellCalculator assigned: {_spellCalculator != null}");
    }
    public void TryUseTrapAbility(Vector3 targetPoint)
    {
        if (isoncooldown) return;

        float trapthrowdistance = Vector3.Distance(attackpoint.position, targetPoint);
        if (throwRange > trapthrowdistance)
        {
            if (!_mana.TryConsume(manaCost)) return;
            GameObject newTrap = Instantiate(trapPrefab, targetPoint, Quaternion.identity);
            Trap trapscript = newTrap.GetComponent<Trap>();

            if (trapscript != null)
            {
                float finalTrapDamage = _spellCalculator.GetSpellDamage(trapBaseDamage, statCoefficient, damageVariance);
                trapscript.Initialize(finalTrapDamage, trapRadius, trapDuration, trapSlowMultiplier, traplifetimebeforeactive, trapZonePrefab);
            }
            StartCoroutine(CooldownBase());
        }
    }
}

