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
}
