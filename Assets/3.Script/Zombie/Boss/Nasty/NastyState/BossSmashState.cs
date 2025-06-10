using UnityEngine;

namespace _3.Script.Zombie.BossState
{
    public class BossSmashState : IBossState
    {
        private static readonly int SMASH = Animator.StringToHash("Smash");

        public void EnterState(BossController boss)
        {
            boss.animator.SetTrigger(SMASH);
            boss.StartDelay(3f, new BossMoveState());
        }

        public void UpdateState(BossController boss)
        {
        }

        public void ExitState(BossController boss)
        {
        }
    }
}