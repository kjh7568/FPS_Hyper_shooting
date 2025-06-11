using UnityEngine;

public class BossStunnedState : IBossState
{
    private static readonly int STUNNED = Animator.StringToHash("Stunned");

    public void EnterState(BossController boss)
    {
        boss.animator.SetTrigger(STUNNED);
        boss.StartDelay(1.7f, new BossMoveState());        
    }

    public void UpdateState(BossController boss)
    {
    }

    public void ExitState(BossController boss)
    {
    }
}