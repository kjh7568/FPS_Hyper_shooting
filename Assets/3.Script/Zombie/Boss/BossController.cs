using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BossController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target; // 따라갈 대상 (예: 플레이어)
    public Animator animator;
    
    protected IBossState CurrentState;

    public void SwitchState(IBossState newState)
    {
        CurrentState?.ExitState(this);
        CurrentState = newState;
        CurrentState?.EnterState(this);
    }

    public void StartDelay(float delayTime, IBossState nextState)
    {
        StartCoroutine(Delay_Coroutine(delayTime, nextState));
    }
    
    private IEnumerator Delay_Coroutine(float delayTime, IBossState nextState)
    {
        yield return new WaitForSeconds(delayTime);
        
        SwitchState(nextState);
    }
}