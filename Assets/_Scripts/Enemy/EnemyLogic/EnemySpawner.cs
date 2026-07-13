using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawntime = 3f;
    public float spawnradius = 5f;
    public int maxEnemies = 20;
    private int spawnedCount = 0;
    public float timer;
    void Update()
    {
        if (spawnedCount >= maxEnemies)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer >= spawntime)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-spawnradius, spawnradius), 0, Random.Range(-spawnradius, spawnradius));
            Vector3 finalSpawnPos = transform.position + randomOffset;
            Instantiate(enemyPrefab, finalSpawnPos, Quaternion.identity);
            spawnedCount++;
            timer = 0f;
            if (spawnedCount == maxEnemies)
            {
                Debug.Log("All spawned");
            }
        }


    }
}
