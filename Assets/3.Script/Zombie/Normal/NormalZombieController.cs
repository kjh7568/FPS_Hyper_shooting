using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NormalZombieController : MonoBehaviour
{
    private static readonly int WALK = Animator.StringToHash("Walk");
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int RUN = Animator.StringToHash("Run");
    private static readonly int DEATH = Animator.StringToHash("Death");
    private static readonly int ATTACK = Animator.StringToHash("Attack");

    public Animator animator;

    private Transform target; // 따라갈 대상 (예: 플레이어)

    private NavMeshAgent agent;
    private NormalZombie normalZombie;
    private float detectionRange = 20f;
    private float attackRange = 1.3f;

    private float wanderRadius = 15f;

    private bool isWandering = false;
    private bool isChasingPlayer = false; // ▶️ 플레이어를 발견했는지 여부

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        normalZombie = GetComponent<NormalZombie>();
    }

    private void Update()
    {
        if (normalZombie.isDead) return;
        
        float distance = Vector3.Distance(transform.position, target.position);

        if (ShouldChasePlayer(distance))
        {
            HandleChase(distance);
        }
        else if (!isWandering)
        {
            agent.speed = 1;
            StartCoroutine(WanderRoutine());
        }
    }

    private bool ShouldChasePlayer(float distance)
    {
        return isChasingPlayer || distance < detectionRange;
    }

    private void HandleChase(float distance)
    {
        if (!isChasingPlayer)
        {
            StartChase();
        }

        if (distance < attackRange)
        {
            StartAttack();
        }
        else
        {
            agent.SetDestination(target.position);
        }
    }

    private void StartChase()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Run")) return;
        
        animator.ResetTrigger(ATTACK);
        animator.SetTrigger(RUN);

        isChasingPlayer = true;
        agent.isStopped = false;

        agent.speed = normalZombie.zombieStat.moveSpeed;
        
        StopAllCoroutines();
    }

    private void StartAttack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Attack")) return;
        
        transform.LookAt(target);

        animator.ResetTrigger(RUN);
        animator.SetTrigger(ATTACK);

        isChasingPlayer = false;
        agent.isStopped = true;
    }

    // private IEnumerator WanderRoutine()
    // {
    //     isWandering = true;
    //     
    //     while (!isChasingPlayer) // 플레이어 발견 시 중단 조건 추가
    //     {
    //         animator.SetTrigger(WALK);
    //
    //         Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
    //         agent.isStopped = false;
    //         agent.SetDestination(newPos);
    //
    //         // 목적지 도착까지 기다림
    //         while (agent.pathPending || agent.remainingDistance > 0.25f)
    //         {
    //             yield return null;
    //         }
    //
    //         // 도착 후 멈춤
    //         agent.isStopped = true;
    //
    //         animator.SetTrigger(IDLE);
    //
    //         var stopDuration = Random.Range(3, 7);
    //         yield return new WaitForSeconds(stopDuration);
    //     }
    //
    //     // 플레이어 발견 시 루프 탈출 후 정리
    //     isWandering = false;
    // }

    private IEnumerator WanderRoutine()
    {
        isWandering = true;

        while (!isChasingPlayer)
        {
            if (!agent.isOnNavMesh)
            {
                Debug.LogWarning("NavMesh 위에 없으므로 방황 루틴 종료");
                yield break;
            }

            animator.SetTrigger(WALK);

            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.isStopped = false;
            agent.SetDestination(newPos);

            while (agent.pathPending || (agent.isOnNavMesh && agent.remainingDistance > 0.25f))
            {
                yield return null;
            }

            agent.isStopped = true;

            animator.SetTrigger(IDLE);

            float stopDuration = Random.Range(3, 7);
            yield return new WaitForSeconds(stopDuration);
        }

        isWandering = false;
    }
    
    private Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, NavMesh.AllAreas);

        return navHit.position;
    }

    public void Die()
    {
        animator.SetTrigger(DEATH);
        agent.isStopped = true;
    }
}