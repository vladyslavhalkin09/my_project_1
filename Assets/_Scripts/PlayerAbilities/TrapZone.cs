using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapZone : MonoBehaviour
{

    public float activetrapradius;
    public float activetrapduration;
    public float activetrapdamage;
    public float activetrapslowmultiplier;
    List<Health> enemiesinzone = new List<Health>();

    public void Start()
    {
        transform.localScale = new Vector3(activetrapradius, 0.1f, activetrapradius);
        StartCoroutine(ZoneDuration(activetrapduration));
        StartCoroutine(ZoneDamageTime());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                enemiesinzone.Add(health);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                enemiesinzone.Remove(health);
            }
        }
    }

    IEnumerator ZoneDuration(float time)
    {
        yield return new WaitForSeconds(activetrapduration);
        Destroy(gameObject);
    }
    IEnumerator ZoneDamageTime()
    {
        while (activetrapduration > 0)
        {
            activetrapduration -= Time.deltaTime;

            foreach (Health health in enemiesinzone)
            {
                if (health != null)
                {
                    health.TakeDamage(activetrapdamage, DamageType.Ability);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
