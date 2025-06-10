using UnityEngine;

public class BossBrassState : IBossState
{
    private static readonly int BRASS = Animator.StringToHash("Brass");

    public void EnterState(BossController boss)
    {
        boss.animator.SetTrigger(BRASS);
        boss.StartDelay(1.7f, new BossMoveState());        
    }

    public void UpdateState(BossController boss)
    {
        
    }

    public void ExitState(BossController boss)
    {
        
    }
}