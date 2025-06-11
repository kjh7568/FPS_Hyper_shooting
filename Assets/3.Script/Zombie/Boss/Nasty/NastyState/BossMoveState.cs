using _3.Script.Zombie.BossState;
using UnityEngine;

public class BossMoveState : IBossState
{
    private static readonly int WALK = Animator.StringToHash("Walk");

    private float brassDelay = 5f;
    private float brassTimer = 0f;
    
    public void EnterState(BossController boss)
    {
        boss.animator.SetTrigger(WALK);
        brassTimer = 0;
    }

    public void UpdateState(BossController boss)
    {
        float distance = Vector3.Distance(boss.transform.position, boss.target.position);

        brassTimer += Time.deltaTime;

        if (distance < 10f)
        {
            boss.agent.isStopped = true;                
            boss.transform.LookAt(boss.target);
            boss.SwitchState(Random.Range(0, 100) < 60 ? new BossAttackState() : new BossSmashState());
        }
        else if (distance < 20f)
        {
            if (brassTimer >= brassDelay)
            {
                boss.agent.isStopped = true;
                boss.transform.LookAt(boss.target);
                boss.SwitchState(new BossBrassState());
            }
            else
            {
                boss.agent.isStopped = false;
                boss.agent.SetDestination(boss.target.position);
            }
        }
        else
        {
            boss.agent.isStopped = false;
            boss.agent.SetDestination(boss.target.position);
        }
    }

    public void ExitState(BossController boss)
    {
    }
}