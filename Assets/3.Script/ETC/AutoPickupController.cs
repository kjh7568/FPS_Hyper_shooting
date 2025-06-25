using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPickupController : MonoBehaviour
{
    private const int BaseCoinValue = 10;
    
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
        if (!other.CompareTag("Player")) return;
        if (Player.localPlayer == null) return;

        int finalCoin = Mathf.RoundToInt(BaseCoinValue * Player.localPlayer.coreStat.coinGainMultiplier);
        GiveCoinToPlayer(finalCoin);

        Destroy(gameObject);
    }

    private void GiveCoinToPlayer(int amount)
    {
        Player.localPlayer.coin += amount;

        // TODO: 수집 효과나 사운드 재생 추가
        // 추후 UI 반영이나 사운드 재생 등을 여기에 추가 가능
        // 예: UIManager.Instance.ShowCoinEffect(amount);
        // 예: AudioManager.Instance.Play("CoinPickup");
    }

    private IEnumerator Wait_Coroutine()
    {
        yield return new WaitForSeconds(1f);
        
        isReady = true;
    }
}