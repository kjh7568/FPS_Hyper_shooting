using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nasty : MonoBehaviour, IMonster
{
    private static readonly int STUNNED = Animator.StringToHash("Stunned");
    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;
    public ZombieStat ZombieStat => zombieStat;

    public Collider normalAttackCollider;
    public Collider smashAttackCollider;

    [SerializeField] private ZombieStat zombieStat;
    [SerializeField] private Collider mainCollider;

    private float desiredHealth;

    public void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        zombieStat.health -= combatEvent.Damage;

        if (zombieStat.health <= 0)
        {
            GetComponent<BossController>().SwitchState(new BossDieState());
            mainCollider.enabled = false;
        }
        else if (zombieStat.health <= desiredHealth)
        {
            GetComponent<BossController>().animator.SetTrigger(STUNNED);
        }
    }

    private void SetHitAnimationHealth()
    {
        var rate = Random.Range(0.1f, 0.3f);

        var desiredHealth = zombieStat.health * (1 - rate);
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }
}