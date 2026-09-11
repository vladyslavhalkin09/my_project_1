using UnityEngine;

public class EnemyRanged : EnemyBase
{
    private float enemyShotCooldown;
    public GameObject EnemyBulletprefab;
    public Transform enemyAttackpoint;
    private EnemyRangedData RangedData => data as EnemyRangedData;


    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();

    }
    protected override void Update()
    {
        if (player == null) return;
        base.Update();
        if (currentState == EnemyState.Attack)
        {
            RangedAttackOnPlayer();
        }
        float distance = Vector3.Distance(transform.position, player.position);
        if (currentState == EnemyState.Attack && distance > RangedData.attackRange)
        {
            currentState = EnemyState.Chase;
        }
    }

    void RangedAttackOnPlayer()
    {
        enemyShotCooldown -= Time.deltaTime;
        Vector3 lookAtPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookAtPos);
        if (enemyShotCooldown <= 0)
        {
            GameObject bullet = Instantiate(EnemyBulletprefab, enemyAttackpoint.position, enemyAttackpoint.rotation);
            EnemyBulllet bullletscript = bullet.GetComponent<EnemyBulllet>();
            if (bullletscript != null)
            {
                bullletscript.enemybulletdamage = _enemyStatsHolder != null ? _enemyStatsHolder.GetAttackDamage() : RangedData.minDamage;
                bullletscript.enemybulletlifetime = RangedData.projectileLifeTime;
                bullletscript.enemybulletspeed = RangedData.projectileSpeed;
                bullletscript.Initialize();
            }
            enemyShotCooldown = RangedData.enemyTimeBetweenShots;
        }
    }

    public override void Chase()
    {
        if (player == null) return;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < RangedData.attackRange)
        {
            agent.ResetPath();
            currentState = EnemyState.Attack;
        }
        else
        {
            agent.speed = data.enemyspeed;
            agent.SetDestination(player.position);
        }
    }

}