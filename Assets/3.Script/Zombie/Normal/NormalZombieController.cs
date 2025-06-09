using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalZombieController : MonoBehaviour
{
    [SerializeField] private Transform target; // 따라갈 대상 (예: 플레이어)

    private NavMeshAgent agent;
    private float detectionRange = 30f;
    
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
        float distance = Vector3.Distance(transform.position, target.position);
        

        if (distance < detectionRange)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            agent.ResetPath();
        }
    }
}