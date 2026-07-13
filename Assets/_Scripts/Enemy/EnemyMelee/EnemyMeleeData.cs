using UnityEngine;
[CreateAssetMenu(fileName = "EnemyMeleeData", menuName = "Data/EnemyMelee")]
public class EnemyMeleeData : EnemyData
{
    public float attackDistance = 1.2f;
    [Header("Attack")]
    public float damageAmount = 10f;
    public float attackRate = 1f;

}
