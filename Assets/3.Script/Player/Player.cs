using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageAble
{
    public static Player localPlayer;

    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;
    public PlayerStat playerStat;
    
    [SerializeField] private Collider mainCollider;
    
    public Inventory inventory = new Inventory();

    [SerializeField] private ArmorDataSO helmetSO;
    [SerializeField] private ArmorDataSO chestPlateSO;
    [SerializeField] private ArmorDataSO glovesSO;
    [SerializeField] private ArmorDataSO bootsSO;

    public int coin = 0;
    
    private void Awake()
    {
        localPlayer = this;
    }

    private void Start()
    {
        Armor helmet = new Armor(helmetSO);
        Armor chestPlate = new Armor(chestPlateSO);
        Armor gloves = new Armor(glovesSO);
        Armor boots = new Armor(bootsSO);
        
        inventory.EquipArmor(helmet);
        inventory.EquipArmor(chestPlate);
        inventory.EquipArmor(gloves);
        inventory.EquipArmor(boots);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.DebugPrintTotalDefense();
        }
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        playerStat.health -= combatEvent.Damage;

        if (playerStat.health <= 0)
        {
            //todo 플레이어 사망 처리
            Debug.Log("플레이어 사망처리 할 것");
        }
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new NotImplementedException();
    }
}