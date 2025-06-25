using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAction : StateMachineBehaviour
{
    [Range(0f, 1f)] public float startNormalizedTime;

    private bool isPassStartNormalizedTime;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isPassStartNormalizedTime = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float time = stateInfo.normalizedTime % 1f;

        if (!isPassStartNormalizedTime && time >= startNormalizedTime)
        {
            isPassStartNormalizedTime = true;
            WeaponManager.instance.knifeWeapon.Fire();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var weaponManager = WeaponManager.instance;
        
        weaponManager.currentWeapon.gameObject.SetActive(true);
        weaponManager.knifeWeapon.gameObject.SetActive(false);
        
        if (weaponManager.isPrimary)
        {
            weaponManager.characterModel.localPosition = weaponManager.RifleStancePosition;
            weaponManager.characterModel.localRotation = weaponManager.RifleStanceRotation;
        }
        else
        {
            weaponManager.characterModel.localPosition = weaponManager.PistolStancePosition;
            weaponManager.characterModel.localRotation = weaponManager.PistolStanceRotation;
        }
    }

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
