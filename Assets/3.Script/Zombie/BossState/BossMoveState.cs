using UnityEngine;

public class BossMoveState : IBossState

{
    public void EnterState(BossController boss)
    {
        // boss.animator.Play("Walk");
    }


    public void UpdateState(BossController boss)
    {
        boss.agent.SetDestination(boss.target.position);
    }

    public void ExitState(BossController boss)
    {
        
    }
}