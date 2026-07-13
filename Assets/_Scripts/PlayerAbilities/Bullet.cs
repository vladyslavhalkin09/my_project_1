using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletspeed = 10f;
    public float lifetime = 2f;
    public float bulletdamage = 25;
    public float slowMultiplier;
    public float slowDuration;
    public GameObject impactEffectPrefab;
    private Vector3 spawnposition;
    public Vector3 bullethitpoint;
    private Vector3 bulletdirection;
    public DamageType damageType = DamageType.Normal;

    public float maxDistance;
    void Start()
    {
        // Destroy(gameObject, lifetime);
        spawnposition = transform.position;
        bulletdirection = (bullethitpoint - spawnposition).normalized;

    }
    void Update()
    {
        transform.Translate(bulletdirection * bulletspeed * Time.deltaTime, Space.World);
        float travelDistance = Vector3.Distance(spawnposition, transform.position);
        if (maxDistance <= travelDistance)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Влучання у: " + other.gameObject.name);
        if (other.CompareTag("Player") || other.CompareTag("Bullet") ||
    other.CompareTag("ImpactEffect") || other.CompareTag("EnemyBullet") ||
    other.CompareTag("TrapZone") || other.CompareTag("TriggerArea"))
        {
            return;
        }
        float randomsize = Random.Range(0.8f, 1.2f);
        GameObject hiteffect = Instantiate(impactEffectPrefab, transform.position, transform.rotation);
        hiteffect.transform.localScale = impactEffectPrefab.transform.localScale * randomsize;

        Destroy(hiteffect, 0.5f);
        if (other.CompareTag("Enemy"))
        {
            Health enemyhealth = other.GetComponent<Health>();
            if (enemyhealth != null)
            {
                enemyhealth.TakeDamage(bulletdamage, damageType);
                EnemyBase enemy = other.GetComponent<EnemyBase>();
                enemy?.ApplySlow(slowDuration, slowMultiplier);
            }
            else
            {
                Debug.LogError("no health script");
            }
        }
        Destroy(gameObject);
    }
}
