using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nasty : MonoBehaviour, IDamageAble
{
    [SerializeField] private ZombieStat zombieStat;

    public void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
    }
    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;
    
    [SerializeField] private Collider mainCollider;
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        Debug.Log("Nasty 맞음");
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }
}
