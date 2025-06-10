using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BossController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target; // 따라갈 대상 (예: 플레이어)
    
    protected IBossState CurrentState;

    protected void SwitchState(IBossState newState)
    {
        CurrentState?.ExitState(this);
        CurrentState = newState;
        CurrentState?.EnterState(this);
    }
}