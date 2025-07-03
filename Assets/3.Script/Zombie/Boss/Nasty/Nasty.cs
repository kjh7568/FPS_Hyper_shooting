using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nasty : MonoBehaviour, IMonster
{
    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;
    public ZombieStat ZombieStat => zombieStat;

    public Collider normalAttackCollider;
    public Collider smashAttackCollider;
    public int stunnedCount = 3;

    [SerializeField] private ZombieStat zombieStat;
    [SerializeField] private Collider mainCollider;
    [SerializeField] private GameObject bloodPrefab;

    private WaitForSeconds delay = new WaitForSeconds(2f);
    private float desiredHealth;
    private bool isAlreadyDrop = false;
    
    public void Start()
    {
        MonsterUIManager.instance.SetBossHpBar(ZombieStat);
        CombatSystem.Instance.RegisterMonster(this);
        SetHitAnimationHealth();
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        zombieStat.health -= combatEvent.Damage;

        StartCoroutine(SpawnAndRemoveBlood(combatEvent.HitPosition));
        
        if (zombieStat.health <= 0)
        {
            var controller = GetComponent<BossController>();
            
            controller.SwitchState(new BossDieState());
            StartCoroutine(controller.WaitAfterDeath_Coroutine());
            
            CoreDrop();
            
            FindObjectOfType<BossHPUI>().OffHpBar();
            
            mainCollider.enabled = false;
        }
        else if (zombieStat.health <= desiredHealth && stunnedCount > 0)
        {
            stunnedCount--;
            GetComponent<BossController>().SwitchState(new BossStunnedState());
            SetHitAnimationHealth();
        }
    }

    private void SetHitAnimationHealth()
    {
        var rate = Random.Range(0.2f, 0.35f);

        desiredHealth = zombieStat.health * (1 - rate);
        Debug.Log(desiredHealth);
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator SpawnAndRemoveBlood(Vector3 position)
    {
        var blood =  Instantiate(bloodPrefab, position, Quaternion.identity);

        yield return delay;
        
        Destroy(blood);
    }
    
    private void CoreDrop()
    {
        if (isAlreadyDrop) return;
        isAlreadyDrop = true;
        
        var coreCount = Random.Range(5, 10);
        
        FindObjectOfType<BossUIManager>().gainedCore += coreCount;
        Debug.Log(FindObjectOfType<BossUIManager>().gainedCore);
        
        ItemGenerator.instance.SpawnCore(transform.position, coreCount);
    }
}