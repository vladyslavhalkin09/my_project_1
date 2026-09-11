using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRangedData", menuName = "Data/EnemyRanged")]
public class EnemyRangedData : EnemyData
{
    [Header("Attack settings")]
    public float enemyTimeBetweenShots = 5f;
    public float attackRange = 10f;
    public float projectileSpeed = 8f;
    public float projectileLifeTime = 2f;
}
