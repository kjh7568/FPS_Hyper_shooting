using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : MonoBehaviour, IMonster
{
    public ZombieStat ZombieStat => zombieStat;
    
    public ZombieStat zombieStat;

    public void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
    }
    public Collider MainCollider => mainCollider;
    public GameObject GameObject => this.gameObject;
    
    [SerializeField] private Collider mainCollider;
    
    public Collider normalAttackCollider;
    public bool isDead = false;
    
    public void TakeDamage(CombatEvent combatEvent)
    {
        zombieStat.health -= combatEvent.Damage;

        if (zombieStat.health <= 0)
        {
            isDead = true;
            GetComponent<NormalZombieController>().Die();
            mainCollider.enabled = false;
        }
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }

}
