using UnityEngine;

public class BossDieState : IBossState
{
    private static readonly int DEATH = Animator.StringToHash("Death");

    public void EnterState(BossController boss)
    {
        boss.animator.SetTrigger(DEATH);
        boss.agent.isStopped = true;
    }

    public void UpdateState(BossController boss)
    {
        
    }

    public void ExitState(BossController boss)
    {
        
    }
}