using System;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : WeaponController
{
    [SerializeField] private LayerMask monsterLayer;
    private Rigidbody rb;
    [SerializeField] private WeaponDataSO grenadeRootData; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        this.weapon = new Weapon(grenadeRootData, WeaponGrade.Common); 
        rb.velocity += Camera.main.transform.forward * 20f;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        
        // 오직 Monster 레이어에만 반응
        Collider[] hits = Physics.OverlapSphere(transform.position, 4f, monsterLayer);

        foreach (var hit in hits)
        {
            var target = CombatSystem.Instance.GetMonsterOrNull(hit);
            if (target != null)
            {
                var combatEvent = new CombatEvent
                {
                    Sender = Player.localPlayer,
                    Receiver = target,
                    Damage = weapon.currentStat.damage,
                    HitPosition = hit.ClosestPoint(transform.position),
                    Collider = hit.GetComponent<Collider>()
                };
                CombatSystem.Instance.AddInGameEvent(combatEvent);
            }
        }

        Destroy(gameObject);
    }
    
    public override void Fire()
    {
        
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }
}