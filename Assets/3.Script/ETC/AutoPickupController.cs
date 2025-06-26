using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPickupController : MonoBehaviour
{
    public bool isCore = false;
    
    private const float PlayerYOffset = 1f;
    private const int BaseCoinValue = 10;
    private const float moveSpeed = 20f; // 다가오는 속도

    private bool isReady = false;

    private void Start()
    {
        StartCoroutine(Wait_Coroutine());
    }

    private void Update()
    {
        if (!isReady || Player.localPlayer == null) return;

        Transform playerTransform = Player.localPlayer.transform;
        Vector3 playerPosition = playerTransform.position + Vector3.up * PlayerYOffset;

        float detectionRange = GetPickupRange();
        float distance = Vector3.Distance(transform.position, playerPosition);

        if (distance < detectionRange || isCore)
        {
            MoveTowards(playerPosition);
        }
    }

    private float GetPickupRange()
    {
        var stat = Player.localPlayer.playerStat;
        var core = Player.localPlayer.coreStat;
        return stat.pickupRadius * core.coinDropRange;
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * (moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (Player.localPlayer == null) return;

        if (!isCore)
        {
            int finalCoin = Mathf.RoundToInt(BaseCoinValue * Player.localPlayer.coreStat.coinGainMultiplier);
            GiveCoinToPlayer(finalCoin);
        }
        else
        {
            GiveCoreToPlayer(1);
        }

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

    private void GiveCoreToPlayer(int amount)
    {
        Player.localPlayer.core += amount;

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