using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : MonoBehaviour, IMonster
{
    public ZombieStat ZombieStat => zombieStat;
    
    [SerializeField] private ZombieStat zombieStat;

    public void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
    }
    public Collider MainCollider => mainCollider;
    public GameObject GameObject => this.gameObject;
    
    [SerializeField] private Collider mainCollider;
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        Debug.Log("좀비 맞음");
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }

}
