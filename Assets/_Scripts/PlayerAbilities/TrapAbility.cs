using System.Collections;
using UnityEngine;

public class TrapAbility : BaseAbility
{
    [Header("Settings")]
    public GameObject trapPrefab;
    public GameObject trapZonePrefab;
    public float throwRange;
    public Transform attackpoint;
    public float trapDamage;
    public float trapRadius;
    public float trapDuration;
    public float trapSlowMultiplier;
    public float traplifetimebeforeactive;
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
                trapscript.Initialize(trapDamage, trapRadius, trapDuration, trapSlowMultiplier, traplifetimebeforeactive, trapZonePrefab);
            }
            StartCoroutine(CooldownBase());
        }
    }
}

