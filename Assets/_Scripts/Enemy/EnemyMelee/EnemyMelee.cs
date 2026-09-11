using UnityEngine;

public class EnemyMelee : EnemyBase
{
    public float nextAttackTime;
    private EnemyMeleeData MeleeData => data as EnemyMeleeData;

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
        base.Update();
        if (player == null) return;
        if (currentState == EnemyState.Attack)
        {
            AttackPlayer();
        }
        float distance = Vector3.Distance(transform.position, player.position);
        if (currentState == EnemyState.Attack && distance > MeleeData.attackDistance)
        {
            currentState = EnemyState.Chase;
        }
    }
    void AttackPlayer()
    {
        Vector3 lookAtPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookAtPos);
        if (Time.time >= nextAttackTime)
        {
            Health playerHP = player.GetComponent<Health>();
            if (playerHP != null)
            {
                float dmg = _enemyStatsHolder != null ? _enemyStatsHolder.GetAttackDamage() : MeleeData.minDamage;
                playerHP.TakeDamage(dmg);
                Debug.Log("Вдарив гравця! Залишилось HP: " + playerHP.currentValue);
                nextAttackTime = Time.time + MeleeData.attackRate;
            }
        }
    }

    public override void Chase()
    {
        if (player == null) return;
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < MeleeData.attackDistance)
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