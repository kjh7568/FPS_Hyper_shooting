using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nasty : MonoBehaviour, IMonster
{
    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;
    public ZombieStat ZombieStat => zombieStat;
    
    public Collider normalAttackCollider;
    public Collider smashAttackCollider;
    
    [SerializeField] private ZombieStat zombieStat;
    [SerializeField] private Collider mainCollider;

    public void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
    }
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        zombieStat.health -= combatEvent.Damage;
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }
}
