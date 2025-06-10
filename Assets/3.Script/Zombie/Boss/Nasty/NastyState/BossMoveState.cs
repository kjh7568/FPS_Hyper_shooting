using _3.Script.Zombie.BossState;
using UnityEngine;

public class BossMoveState : IBossState
{
    private static readonly int WALK = Animator.StringToHash("Walk");

    public void EnterState(BossController boss)
    {
        boss.animator.SetTrigger(WALK);
    }

    public void UpdateState(BossController boss)
    {
        float distance = Vector3.Distance(boss.transform.position, boss.target.position);
        
        Debug.Log(distance);
        
        if (distance < 10.5f)
        {
            boss.agent.isStopped = true;

            if (Random.Range(0, 100) < 60) //일반 공격
            {
                boss.SwitchState(new BossAttackState());
            }
            else //내려찍기 공격
            {
                boss.SwitchState(new BossSmashState());
            }
        }
        else
        {
            // if (Random.Range(0, 100) < 60) //일반 공격
            if (Random.Range(0, 70) < 70) //이동
            {
                boss.agent.isStopped = false;
                boss.agent.SetDestination(boss.target.position);
            }
            else //브래스
            {
                boss.SwitchState(new BossSmashState());
            }
        }
    }

    public void ExitState(BossController boss)
    {
    }
}