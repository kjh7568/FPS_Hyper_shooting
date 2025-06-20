using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPickupController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f; // 다가오는 속도
    
    private bool isReady = false;

    private void Start()
    {
        StartCoroutine(Wait_Coroutine());
    }

    private void Update()
    {
        if (!isReady) return;
        
        var detectionRange = Player.localPlayer.playerStat.pickupRadius;
        var playerPosition = Player.localPlayer.transform.position + new Vector3(0, 1f, 0);

        float distance = Vector3.Distance(transform.position, playerPosition);

        if (distance < detectionRange)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            transform.position += direction * (moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //todo 수집 효과나 사운드 넣고
            Destroy(gameObject);
            Player.localPlayer.coin += 10;
        }
    }

    private IEnumerator Wait_Coroutine()
    {
        yield return new WaitForSeconds(1f);
        
        isReady = true;
    }
}