using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IDamageAble
{
    public static Player localPlayer;

    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;
    public PlayerStat playerStat;
    
    [SerializeField] private Collider mainCollider;

    private void Awake()
    {
        localPlayer = this;
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        playerStat.health -= combatEvent.Damage;

        if (playerStat.health <= 0)
        {
            //todo 플레이어 사망 처리
            Debug.Log("플레이어 사망처리 할 것");
        }
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new NotImplementedException();
    }
}