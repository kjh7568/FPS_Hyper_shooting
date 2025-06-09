using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalZombieController : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform target; // 따라갈 대상 (예: 플레이어)

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position); // 목표 위치로 이동
        }
    }
}
