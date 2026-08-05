using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrol,
    Idle,
    Chase,
    Attack
}
public class EnemyBase : MonoBehaviour
{
    public float frontalAggroRadius;
    public float backAggroRadius;
    public EnemyData data;
    public Transform[] patrolPoints;
    protected Transform player;
    protected EnemyState currentState;
    private int currentPatrolIndex;
    public float maxTimeOnPatrolPoint;
    public float currentTimeOnPoint;
    public float stoppingDistance;
    public SquadController squad;
    public Vector3 squadOffset;
    private Transform[] activePatrolPoints;
    protected NavMeshAgent agent;
    bool isSlowed;
    private CharacterStatsHolder _statsHolder;
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _statsHolder = GetComponent<CharacterStatsHolder>();
        if (_statsHolder != null && data != null)
        {
            _statsHolder.Stats.InitializeBaseStats(data.maxHp, 0f, 0f, data.enemyArmor);
        }
    }
    protected virtual void Start()
    {
        activePatrolPoints = squad != null ? squad.patrolPoints : patrolPoints;
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        currentState = EnemyState.Patrol;
    }
    protected virtual void Update()
    {
        switch (currentState)
        {
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Patrol:
                Patrol();
                DetectPlayer();
                break;
            case EnemyState.Idle:
                Idle();
                DetectPlayer();
                break;
            case EnemyState.Attack:
                break;
        }
    }
    public void Patrol()
    {
        if (activePatrolPoints == null || activePatrolPoints.Length == 0) return;
        float distance = Vector3.Distance(transform.position, activePatrolPoints[currentPatrolIndex].position + squadOffset);
        Vector3 targetPos = activePatrolPoints[currentPatrolIndex].position + squadOffset;
        agent.speed = data.patrolSpeed;
        agent.SetDestination(targetPos);
        if (agent.remainingDistance <= stoppingDistance && !agent.pathPending)
        {
            currentState = EnemyState.Idle;
        }
    }
    public void Idle()
    {
        agent.ResetPath();
        currentTimeOnPoint += Time.deltaTime;
        if (currentTimeOnPoint >= maxTimeOnPatrolPoint)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % activePatrolPoints.Length;
            currentState = EnemyState.Patrol;
            currentTimeOnPoint = 0;
        }

    }
    public virtual void Chase()
    {
        if (player == null) return;
        agent.speed = data.enemyspeed;
        agent.SetDestination(player.position);
    }
    public void DetectPlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);
        float dot = Vector3.Dot(transform.forward, dirToPlayer);
        if (dot > 0)
        {
            if (distance < frontalAggroRadius)
            {
                Debug.Log(gameObject.name + " побачив гравця спереду, кличе сквад");
                currentState = EnemyState.Chase;
                if (squad != null)
                {
                    Debug.Log(gameObject.name + " — squad не null, викликаю OnMemberAggro");
                    squad.OnMemberAggro();
                }
                else
                {
                    Debug.Log(gameObject.name + " — squad = NULL!");
                }
            }
        }
        else
        {
            if (distance < backAggroRadius)
            {
                currentState = EnemyState.Chase;
                if (squad != null)
                {
                    Debug.Log(gameObject.name + " — squad не null, викликаю OnMemberAggro");
                    squad.OnMemberAggro();
                }
                else
                {
                    Debug.Log(gameObject.name + " — squad = NULL!");
                }
            }
        }

    }
    public void Aggro()
    {
        currentState = EnemyState.Chase;
        currentTimeOnPoint = 0f;
    }

    IEnumerator ApplyEnemySlow(float slowDuration, float slowMultiplier)
    {
        if (isSlowed) yield break;
        isSlowed = true;
        float originalspeed = agent.speed;
        agent.speed = agent.speed * slowMultiplier;
        yield return new WaitForSeconds(slowDuration);
        agent.speed = originalspeed;
        isSlowed = false;
    }
    public void ApplySlow(float slowDuration, float slowMultiplier)
    {
        StartCoroutine(ApplyEnemySlow(slowDuration, slowMultiplier));
    }
}
