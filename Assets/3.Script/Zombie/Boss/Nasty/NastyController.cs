using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NastyController : BossController
{
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        SwitchState(new BossMoveState());
    }

    private void Update()
    {
        CurrentState?.UpdateState(this);
        Debug.Log(Vector3.Distance(transform.position, target.position));
    }
}
