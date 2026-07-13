using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "Data/Bosses")]
public class BossData : ScriptableObject
{
    [Header("Health")]
    public float bossHp = 1000f;
    [Header("Movement")]
    public float bossMovespeed = 2f;
    public float attackDistance = 1.2f;
    [Header("Attack")]
    public float damageAmount = 10f;
    public float attackRate = 1f;
    public float bossDashMaxRange;
    public float bossDashSpeed;
    public float bossDashDamage;
    public float bossDashWindup;
    public float bossDashCooldown;
    public float bossDashEndPointDamage;
    public float bossDashHitRadius;
    public float bossDashEndPause;
    public float bossMinDashRange;
    public float bossCleaveCooldown;
    public float bossCleaveCastTime;
    public float bossCleaveDamage;
    public float bossCleaveAngle;
    public float bossCleaveDistance;


}
