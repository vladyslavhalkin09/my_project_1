using UnityEngine;


public class EnemyBulllet : MonoBehaviour
{
    public float enemybulletspeed;
    public float enemybulletlifetime;
    public float enemybulletdamage;
    public GameObject enemybulletprefab;
    private Vector3 enemybulletspawnposition;
    public float enemybulletdistance;
    private bool hasHit = false;

    public void Initialize()
    {
        Destroy(gameObject, enemybulletlifetime);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * enemybulletspeed * Time.deltaTime);

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter спрацював з: " + other.gameObject.name + " | тег: " + other.tag + " | час: " + Time.time);
        if (hasHit) return;
        hasHit = true;
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet")) return;
        if (other.CompareTag("Player"))
        {
            Health playerhealth = other.GetComponent<Health>();
            if (playerhealth != null)
            {
                playerhealth.TakeDamage(enemybulletdamage);
            }
            else Debug.LogError("no health on player");

        }
        // Debug.Log("Куля вдарила: " + other.gameObject.name + " тег: " + other.tag);
        Destroy(gameObject);
    }

}
