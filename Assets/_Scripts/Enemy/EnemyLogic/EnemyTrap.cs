using System.Collections;
using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    private GameObject EnemyTrapZonePrefab;
    public float _damage;
    public float _duration;
    public float _slow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                p.StartSpeedBoost(_slow, _duration);
                p.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
    }
}
