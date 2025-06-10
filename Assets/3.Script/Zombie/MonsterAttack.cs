using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private IMonster Owner;
    
    private void Start()
    {
        Owner = gameObject.GetComponentInParent<IMonster>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        Debug.Log(Owner);
        Debug.Log(Owner.ZombieStat);
        
        ZombieStat zombieStat = Owner.ZombieStat;
        
        CombatEvent combatEvent = new CombatEvent
        {
            Sender = Owner,
            Receiver = Player.localPlayer,
            HitPosition = other.ClosestPoint(transform.position),
            Collider = other,
            Damage = zombieStat.damage,
        };
        
        CombatSystem.Instance.AddInGameEvent(combatEvent);
    }
}
