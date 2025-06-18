using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] private float damageMultiplier = 1f;
    
    private IMonster Owner;
    
    private void Start()
    {
        Owner = gameObject.GetComponentInParent<IMonster>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        ZombieStat zombieStat = Owner.ZombieStat;
        
        CombatEvent combatEvent = new CombatEvent
        {
            Sender = Owner,
            Receiver = Player.localPlayer,
            HitPosition = other.ClosestPoint(transform.position),
            Collider = other,
            Damage =  zombieStat.damage * damageMultiplier * (1f - (Player.localPlayer.inventory.EquipmentStat.totalDefense * Player.localPlayer.inventory.EquipmentStat.multiplierDefense) * 0.0001f),
        };
        
        CombatSystem.Instance.AddInGameEvent(combatEvent);
    }
}
