using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NormalZombieController : MonoBehaviour
{
    private static readonly int WALK = Animator.StringToHash("Walk");
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int RUN = Animator.StringToHash("Run");
    [SerializeField] private Animator animator;

    private Transform target; // 따라갈 대상 (예: 플레이어)

    private NavMeshAgent agent;
    private float detectionRange = 20f;

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
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (isChasingPlayer || distance < detectionRange)
        {
            // 플레이어 발견 시 추적 시작
            if (!isChasingPlayer)
            {
                agent.speed = 6;
                animator.SetTrigger(RUN);
                isChasingPlayer = true;
                agent.isStopped = false;
                StopAllCoroutines();
            }

            agent.SetDestination(target.position);
        }
        else
        {
            // 서성거림 상태로 전환
            if (!isWandering)
            {
                agent.speed = 1;
                StartCoroutine(WanderRoutine());
            }
        }
    }

    private IEnumerator WanderRoutine()
    {
        isWandering = true;
        
        while (!isChasingPlayer) // 플레이어 발견 시 중단 조건 추가
        {
            animator.SetTrigger(WALK);

            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.isStopped = false;
            agent.SetDestination(newPos);

            // 목적지 도착까지 기다림
            while (agent.pathPending || agent.remainingDistance > 0.25f)
            {
                yield return null;
            }

            // 도착 후 멈춤
            agent.isStopped = true;

            animator.SetTrigger(IDLE);

            var stopDuration = Random.Range(3, 7);
            yield return new WaitForSeconds(stopDuration);
        }

        // 플레이어 발견 시 루프 탈출 후 정리
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
}