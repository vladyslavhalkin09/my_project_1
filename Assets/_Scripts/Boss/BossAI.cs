using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform target;
    private float nextAttackTime = 0f;
    public LineRenderer line;
    public GameObject cleaveIndicator;
    public CapsuleCollider dashCollider;
    public BossData bossData;
    public float nextBossDashTime;
    public float nextBossCleaveTime;
    private bool isBossDashing = false;
    private bool isBossActing = false;
    void Start()
    {
        // Шукаємо гравця за тегом
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        nextBossDashTime = Time.time + bossData.bossDashCooldown;
        nextBossCleaveTime = Time.time + bossData.bossCleaveCooldown;
        if (player != null) target = player.transform;
    }
    void Update()
    {
        if (target == null) return;
        if (isBossActing) return;
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        float distance = Vector3.Distance(transform.position, target.position);
        Vector3 lookAtPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookAtPos);

        if (distance > bossData.bossMinDashRange)
        {
            transform.Translate(Vector3.forward * bossData.bossMovespeed * Time.deltaTime);
            if (Time.time >= nextBossDashTime)
            {
                BossDash();
            }

        }
        else if (Time.time >= nextBossCleaveTime) BossCleave();

        else BossAttack();
    }
    void BossCleave()
    {
        StartCoroutine(BossCleaveCoroutine());
    }
    void BossDash()
    {
        StartCoroutine(BossDashCoroutine());
    }
    void BossAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            Health playerHP = target.GetComponent<Health>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(bossData.damageAmount);
                Debug.Log("Автоатака" + playerHP.currentValue);
                nextAttackTime = Time.time + bossData.attackRate;
            }
        }
    }

    IEnumerator BossDashCoroutine()
    {
        isBossActing = true;
        Vector3 dashtarget = target.position;
        nextBossDashTime = Time.time + bossData.bossDashCooldown;
        line.enabled = true;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, dashtarget);
        yield return new WaitForSeconds(bossData.bossDashWindup);
        line.enabled = false;
        float distanceTravelled = 0f;
        dashCollider.enabled = true;
        isBossDashing = true;
        while (distanceTravelled < bossData.bossDashMaxRange)
        {
            float step = bossData.bossDashSpeed * Time.deltaTime;
            distanceTravelled += step;
            Vector3 direction = (dashtarget - transform.position).normalized;
            transform.position += direction * step;
            yield return null;
        }

        yield return new WaitForSeconds(bossData.bossDashEndPause);
        dashCollider.enabled = false;
        isBossDashing = false;

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= bossData.bossDashHitRadius)
        {
            Health playerHP = target.GetComponent<Health>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(bossData.bossDashEndPointDamage);
                Debug.Log("Шкода від ривка (кінцева точка)!");
            }
        }
        isBossActing = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isBossDashing)
        {
            Health playerHP = target.GetComponent<Health>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(bossData.bossDashDamage);
                Debug.Log("Шкода від ривка (траєкторія)!");
            }
        }
    }

    IEnumerator BossCleaveCoroutine()
    {

        isBossActing = true;
        nextBossCleaveTime = Time.time + bossData.bossCleaveCooldown;
        Debug.Log("Cleave indicator: " + cleaveIndicator.activeSelf);
        cleaveIndicator.SetActive(true);
        Debug.Log("Після SetActive: " + cleaveIndicator.activeSelf);
        yield return new WaitForSeconds(bossData.bossCleaveCastTime);
        cleaveIndicator.SetActive(false);
        Vector3 dirToPlayer = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        float dist = Vector3.Distance(transform.position, target.position);
        if (angle <= bossData.bossCleaveAngle / 2 && dist <= bossData.bossCleaveDistance)
        {
            Health playerHP = target.GetComponent<Health>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(bossData.bossCleaveDamage);
                Debug.Log("Шкода від конусу!");
            }
        }
        isBossActing = false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rightDir = Quaternion.Euler(0, bossData.bossCleaveAngle / 2, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, -bossData.bossCleaveAngle / 2, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + rightDir * bossData.bossCleaveDistance);
        Gizmos.DrawLine(transform.position, transform.position + leftDir * bossData.bossCleaveDistance);
    }

}
