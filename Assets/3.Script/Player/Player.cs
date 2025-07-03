using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageAble
{
    public static Player localPlayer;

    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;
    public PlayerStat playerStat;
    public CoreStat coreStat;

    [SerializeField] private Collider mainCollider;

    public Inventory inventory = new Inventory();

    [SerializeField] private ArmorDataSO helmetSO;
    [SerializeField] private ArmorDataSO chestPlateSO;
    [SerializeField] private ArmorDataSO glovesSO;
    [SerializeField] private ArmorDataSO bootsSO;

    public int coin = 0;
    public int core = 0;

    private float regenTimer = 0f;

    private void Awake()
    {
        localPlayer = this;
    }

    private void Start()
    {
        Armor helmet = new Armor(helmetSO, true);
        Armor chestPlate = new Armor(chestPlateSO, true);
        Armor gloves = new Armor(glovesSO, true);
        Armor boots = new Armor(bootsSO, true);

        inventory.EquipArmor(helmet);
        inventory.EquipArmor(chestPlate);
        inventory.EquipArmor(gloves);
        inventory.EquipArmor(boots);
    }

    private void Update()
    {
        RegenerateHealthOverTime();
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        playerStat.health -= combatEvent.Damage;

        if (playerStat.health <= 0)
        {
            //todo 플레이어 사망 처리
            StartCoroutine(DelayDie());
        }
    }

    public void TakeHeal(HealEvent healEvent)
    {
        playerStat.health += healEvent.Heal;
        var totalHealth = (playerStat.maxHealth + inventory.EquipmentStat.plusHp + coreStat.plusHp +
                           GameData.Instance.augmentStat.plusHp) * inventory.EquipmentStat.increaseHealth;

        if (playerStat.health > totalHealth)
        {
            playerStat.health = totalHealth;
        }
    }

    private void RegenerateHealthOverTime()
    {
        if (Player.localPlayer == null) return;

        var coreStat = Player.localPlayer.coreStat;

        var stat = Player.localPlayer.playerStat;
        var armorStat = Player.localPlayer.inventory.EquipmentStat;
        float regen = coreStat.hpRegion;
        if (regen <= 0f) return;

        // **전체 최대체력 계산 (기본 + 코어 + 방어구)**
        float totalMax = (stat.maxHealth
                          + coreStat.plusHp
                          + armorStat.plusHp)
                         * armorStat.increaseHealth;

        if (stat.health >= totalMax) return;

        regenTimer += Time.deltaTime;
        if (regenTimer < 1f) return;
        regenTimer = 0f;

        stat.health += regen;
        if (stat.health > totalMax)
            stat.health = totalMax;
    }

    private IEnumerator DelayDie()
    {
        var pc = GetComponent<PlayerController>();

        pc.SetDieAnimation();
        pc.isOpenPanel = true;
        
        yield return new WaitForSeconds(1f);
        
        FindObjectOfType<PlayerDeadPanel>().OpenDiePanel();
    }
}