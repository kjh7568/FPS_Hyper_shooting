using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("플레이어가 맞음!");
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new NotImplementedException();
    }
}