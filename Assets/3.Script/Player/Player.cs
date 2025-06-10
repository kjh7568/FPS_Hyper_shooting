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
    public Gun myGun;
    
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
            Destroy(gameObject);
        }
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new NotImplementedException();
    }
}