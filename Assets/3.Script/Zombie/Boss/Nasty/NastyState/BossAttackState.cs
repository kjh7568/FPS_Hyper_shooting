using System.Collections;
using UnityEngine;

public class BossAttackState : IBossState
{
    private static readonly int ATTACK = Animator.StringToHash("Attack");

    public void EnterState(BossController boss)
    {
        boss.animator.SetTrigger(ATTACK);
        boss.StartDelay(1.7f, new BossMoveState());        
    }

    public void UpdateState(BossController boss)
    {
    }

    public void ExitState(BossController boss)
    {
    }
}