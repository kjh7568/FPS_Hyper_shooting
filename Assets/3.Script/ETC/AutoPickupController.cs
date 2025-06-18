using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPickupController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f; // 다가오는 속도

    private void Update()
    {
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
        }
    }
}