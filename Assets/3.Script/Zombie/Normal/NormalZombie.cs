using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : MonoBehaviour, IMonster
{
    public ZombieStat ZombieStat => zombieStat;
    public ZombieStat zombieStat;

    [SerializeField] private GameObject bloodPrefab;

    private WaitForSeconds delay = new WaitForSeconds(2f);
    
    public void Start()
    {
        CombatSystem.Instance.RegisterMonster(this);
    }

    public Collider MainCollider => mainCollider;
    public GameObject GameObject => this.gameObject;

    [SerializeField] private Collider mainCollider;
    [SerializeField] private LayerMask monsterLayer;

    public Collider normalAttackCollider;
    public bool isDead = false;

    public void TakeDamage(CombatEvent combatEvent)
    {
        zombieStat.health -= combatEvent.Damage;

        StartCoroutine(SpawnAndRemoveBlood(combatEvent.HitPosition));
        
        Collider[] hits = Physics.OverlapSphere(transform.position, 3f, monsterLayer);

        foreach (Collider hit in hits)
        {
            hit.gameObject.GetComponent<NormalZombieController>().isHearing = true;
        }
        
        if (zombieStat.health <= 0)
        {
            isDead = true;
            GetComponent<NormalZombieController>().Die();
            GetComponent<NormalZombieSound>().PlayDeathSound();
            RandomItemDrop();
            GoldDrop();
            mainCollider.enabled = false;
        }
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }

    private void RandomItemDrop()
    {
        var ratio = Random.Range(0, 1000);

        //todo 아이템 드롭확률 구현 되면 적용해볼 것
        if (ratio < 300)
        {
            ItemGenerator.instance.SpawnItem(transform.position);
        }
    }

    private void GoldDrop()
    {
        var goldCount = Random.Range(1, 5);
        
        ItemGenerator.instance.SpawnGold(transform.position, goldCount);
    }

    private IEnumerator SpawnAndRemoveBlood(Vector3 position)
    {
        var blood =  Instantiate(bloodPrefab, position, Quaternion.identity);

        yield return delay;
        
        Destroy(blood);
    }
}