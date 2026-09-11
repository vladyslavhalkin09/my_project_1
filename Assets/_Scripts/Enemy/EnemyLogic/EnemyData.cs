using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Health")]
    public float maxHp = 100f;
    [Header("Movement")]
    public float enemyspeed = 2f;
    [Header("Patrol")]
    public float patrolSpeed;
    public float enemyArmor;
     [Header("Stats")]
    public float mainStat = 0f;
    [Header("Damage")]
    public float minDamage = 10f;
    public float maxDamage = 10f;
    public float statMultiplier = 0f;
}
