using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nasty : MonoBehaviour, IDamageAble
{
    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;
    
    public Collider normalAttackCollider;
    public Collider smashAttackCollider;
    
    [SerializeField] private Collider mainCollider;
    [SerializeField] private ZombieStat zombieStat;

    public void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
    }
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        Debug.Log("Nasty 맞음");
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }
}
