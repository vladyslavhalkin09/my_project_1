using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class RoomEncounter : MonoBehaviour
{
    [SerializeField]
    public GameObject[] doors;
    [SerializeField]
    Wave[] waves;
    public EnemySpawnPoint[] bossPhaseSpawnPoints;
    public int currentWave;
    public int activeEnemies;
    private List<GameObject> currentEnemies = new List<GameObject>();
    bool isActive;

    public void StartEncounter()
    {
        if (isActive) return;
        Health.OnEnemyDied += OnEnemyDied;
        isActive = true;
        SpawnWave();
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i] != null)
            {
                doors[i].SetActive(true);
            }
        }
    }
    public void SpawnWave()
    {
        currentEnemies.Clear();
        int enemyCount = waves[currentWave].enemyCount;
        GameObject prefab = waves[currentWave].enemyPrefab;
        Debug.Log($"Хвиля {currentWave}, ворогів: {enemyCount}");
        for (int i = 0; i < enemyCount; i++)
        {
            EnemySpawnPoint point = waves[currentWave].spawnPoints[i % waves[currentWave].spawnPoints.Length];
            GameObject enemy = point.SpawnEnemy(prefab);
            currentEnemies.Add(enemy);
            BossHealth bossHealth = enemy.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.spawnPoints = bossPhaseSpawnPoints;
            }
        }
        activeEnemies = currentEnemies.Count;
    }
    void OnEnemyDied(GameObject enemy)
    {
        if (!currentEnemies.Contains(enemy)) return;
        currentEnemies.Remove(enemy);
        activeEnemies--;
        Debug.Log($"Ворог вмер, залишилось: {activeEnemies}");
        Debug.Log($"OnEnemyDied викликано, activeEnemies до: {activeEnemies}, currentWave: {currentWave}");
        if (activeEnemies <= 0)
        {
            currentWave++;
            Debug.Log($"currentWave після ++: {currentWave}");
            if (currentWave < waves.Length) SpawnWave();
            else OpenDoors();
        }
    }
    void OpenDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i] != null)
            {
                doors[i].SetActive(false);
            }
        }
        Health.OnEnemyDied -= OnEnemyDied;
        isActive = false;
    }
}
