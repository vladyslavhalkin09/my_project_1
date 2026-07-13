using UnityEngine;

public class BossHealth : EnemyHealth
{
    public float phaseTrheshold;
    bool phaseTriggered = false;
    public EnemySpawnPoint[] spawnPoints;
    public GameObject enemyPrefab;
    public int phaseEnemyCount;
    public override void TakeDamage(float dmg, DamageType type = DamageType.Normal)
    {
        base.TakeDamage(dmg, type);
        if (currentValue <= maxValue * phaseTrheshold && !phaseTriggered)
        {
            phaseTriggered = true;
            if (spawnPoints != null && spawnPoints.Length > 0)
            {

                for (int i = 0; i < phaseEnemyCount; i++)
                {
                    EnemySpawnPoint point = spawnPoints[i % spawnPoints.Length];
                    GameObject enemy = point.SpawnEnemy(enemyPrefab);
                }
            }
        }
    }

}
