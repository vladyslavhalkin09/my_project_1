using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private GameObject trapzoneprefab;
    private float _damage;
    private float _radius;
    private float _duration;
    private float _slow;
    private float nonactivetraplifetime;


    IEnumerator TrapLifeBeforeActive(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TrapActivation();
            Destroy(gameObject);
        }
    }

    public void Initialize(float damage, float radius, float duration, float slow, float traplifetimebeforeactive, GameObject zonePrefab)
    {
        _damage = damage;
        _radius = radius;
        _duration = duration;
        _slow = slow;
        trapzoneprefab = zonePrefab;
        nonactivetraplifetime = traplifetimebeforeactive;
        StartCoroutine(TrapLifeBeforeActive(nonactivetraplifetime));

    }
    void TrapActivation()
    {
        GameObject newZone = Instantiate(trapzoneprefab, transform.position, Quaternion.identity);
        TrapZone zoneScript = newZone.GetComponent<TrapZone>();
        if (zoneScript != null)
        {
            zoneScript.activetrapradius = _radius;
            zoneScript.activetrapdamage = _damage;
            zoneScript.activetrapduration = _duration;
            zoneScript.activetrapslowmultiplier = _slow;
        }
    }

}
