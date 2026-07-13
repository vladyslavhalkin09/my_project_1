using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public GameObject SpawnEnemy(GameObject prefab)
    {
        Debug.Log($"Spawning {prefab.name} at {transform.position}");
        GameObject enemySpawned = Instantiate(prefab, transform.position, Quaternion.identity);
        return enemySpawned;
    }
}
