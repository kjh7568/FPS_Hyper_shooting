using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombieAttackAction : StateMachineBehaviour
{
    [Range(0f, 1f)] public float startNormalizedTime;
    [Range(0f, 1f)] public float endNormalizedTime;

    private bool isPassStartNormalizedTime;
    private bool isPassEndNormalizedTime;
    private Collider attackCollider;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isPassStartNormalizedTime = false;
        isPassEndNormalizedTime = false;
        
        attackCollider = animator.GetComponent<NormalZombie>().normalAttackCollider;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float time = stateInfo.normalizedTime % 1f;

        // 콜라이더 켜기 시점
        if (!isPassStartNormalizedTime && time >= startNormalizedTime)
        {
            isPassStartNormalizedTime = true;
            attackCollider.enabled = true;  
        }

        // 콜라이더 끄기 시점
        if (!isPassEndNormalizedTime && time >= endNormalizedTime)
        {
            isPassEndNormalizedTime = true;
            attackCollider.enabled = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
