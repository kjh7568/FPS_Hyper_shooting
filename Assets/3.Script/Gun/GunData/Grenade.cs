using System;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : Gun
{
    [SerializeField] private LayerMask monsterLayer;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        InitGun();
        rb.velocity += Camera.main.transform.forward * 20f;
    }


    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        
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
                    Damage = currentStat.damage,
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