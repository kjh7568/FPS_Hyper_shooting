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
        if (!IsValidPlayer(other)) return;

        float damage = CalculateDamage();
        Vector3 hitPosition = other.ClosestPoint(transform.position);

        CombatEvent combatEvent = CreateCombatEvent(other, hitPosition, damage);
        CombatSystem.Instance.AddInGameEvent(combatEvent);
    }

    private bool IsValidPlayer(Collider other)
    {
        if (!other.CompareTag("Player")) return false;
        if (other.transform.root.gameObject != Player.localPlayer.gameObject) return false;
        return true;
    }

    private float CalculateDamage()
    {
        ZombieStat zombieStat = Owner.ZombieStat;
        EquipmentStat equip = Player.localPlayer.inventory.EquipmentStat;
        float defenseFactor = equip.totalDefense *
                              (equip.increaseDefense + GameData.Instance.augmentStat.increaseDefense) *
                              Player.localPlayer.coreStat.multiplierDefense * 0.0001f;
        return zombieStat.damage * damageMultiplier * (1f - defenseFactor);
    }

    private CombatEvent CreateCombatEvent(Collider other, Vector3 hitPosition, float damage)
    {
        return new CombatEvent
        {
            Sender = Owner,
            Receiver = Player.localPlayer,
            HitPosition = hitPosition,
            Collider = other,
            Damage = damage,
        };
    }
}