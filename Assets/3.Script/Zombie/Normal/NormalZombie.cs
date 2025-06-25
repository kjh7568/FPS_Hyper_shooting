using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : MonoBehaviour, IMonster
{
    private const int MaxRollValue = 1000;
    private const float BaseDropRate = 0.3f; // 300 / 1000 = 30%

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
        if (Player.localPlayer == null) return;

        float finalDropChance = CalculateFinalDropChance();
        int roll = Random.Range(0, MaxRollValue);

        if (roll < MaxRollValue * finalDropChance)
        {
            ItemGenerator.instance.SpawnItem(transform.position);
        }
    }

    private float CalculateFinalDropChance()
    {
        var equipment = Player.localPlayer.inventory.EquipmentStat;
        var core = Player.localPlayer.coreStat;

        float multiplier = equipment.multiplierRareItemChance + core.itemDropChance;
        return BaseDropRate * multiplier;
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