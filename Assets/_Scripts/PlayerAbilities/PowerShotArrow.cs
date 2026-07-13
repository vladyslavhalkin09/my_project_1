using UnityEngine;

public class PowerShotArrow : MonoBehaviour
{
    public float PowerShotArrowDamage;
    public bool hasHitFirstEnemy;
    public Vector3 ArrowHitpoint;
    private Vector3 ArrowDirection;
    private Vector3 ArrowSpawnPosition;
    public float PowerShotArrowSpeed;

    void Start()
    {
        ArrowSpawnPosition = transform.position;
        ArrowDirection = (ArrowHitpoint - ArrowSpawnPosition).normalized;
        ArrowDirection.y = 0;
        ArrowDirection = ArrowDirection.normalized;
        Destroy(gameObject, 2f);
    }
    void Update()
    {
        transform.Translate(ArrowDirection * PowerShotArrowSpeed * Time.deltaTime, Space.World);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health enemyhealth = other.GetComponent<Health>();
            if (enemyhealth != null)
            {
                if (hasHitFirstEnemy == false)
                {
                    enemyhealth.TakeDamage(PowerShotArrowDamage, DamageType.Ability);
                    hasHitFirstEnemy = true;
                }
                else if (hasHitFirstEnemy == true)
                {
                    enemyhealth.TakeDamage(PowerShotArrowDamage / 2, DamageType.Ability);
                }
            }
            else
            {
                Debug.LogError("no health script");
            }
        }
    }
}
