using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    [System.Serializable]
    public struct DropEntry
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float chance;
    }
    public List<DropEntry> drops;
    private void OnEnable()
    {
        Health.OnEnemyDied += Drop;
    }
    private void OnDisable()
    {
        Health.OnEnemyDied -= Drop;
    }

    private void Drop(GameObject enemy)
    {
        if (enemy != gameObject) return;
        foreach (var drop in drops)
        {
            float roll = Random.Range(0f, 1f);
            if (roll <= drop.chance)
            {
                Instantiate(drop.prefab, transform.position, Quaternion.identity);
            }
        }
    }
}
