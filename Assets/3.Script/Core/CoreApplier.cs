using UnityEngine;

[System.Serializable]
public class CoreStat
{
    // Defense
    public float coreDefense;
    public float coreHp;
    public float coreHpRegion;

    // Attack
    public float coreDamage;
    public float primaryDamage;
    public float secondaryDamage;
    public float meleeDamage;
    public float grenadeDamage;

    // Utility
    public float coinDropChance;
    public float coinDropRange;
    public float itemDropChance;
    public float coreMovementSpeed;
    public float grenadeCooldown;
    public float grenadeRange;
}

public class CoreApplier : MonoBehaviour
{
    public static CoreApplier Instance;

    private CoreStat coreStat = new CoreStat();

    private float regenTimer = 0f;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Update()
    {
        RegenerateHealthOverTime();
    }

    private void RegenerateHealthOverTime()
    {
        if (Player.localPlayer == null) return;

        var stat      = Player.localPlayer.playerStat;
        var armorStat = Player.localPlayer.inventory.EquipmentStat;
        float regen   = coreStat.coreHpRegion;
        if (regen <= 0f) return;

        // **전체 최대체력 계산 (기본 + 코어 + 방어구)**
        float totalMax = (stat.maxHealth 
                          + coreStat.coreHp 
                          + armorStat.increaseHealth) 
                         * armorStat.multiplierHealth;

        if (stat.health >= totalMax) return;

        regenTimer += Time.deltaTime;
        if (regenTimer < 1f) return;
        regenTimer = 0f;

        stat.health += regen;
        if (stat.health > totalMax)
            stat.health = totalMax;
    }



    public void ApplyCore(CoreDataSO data, int level)
    {
        var levelData = data.levelStats.Find(x => x.level == level);
        if (levelData == null)
        {
            Debug.LogWarning($"[CoreApplier] {data.coreID} {level}레벨 정보 없음");
            return;
        }

        float value = levelData.value;

        switch (data.coreID)
        {
            // Attack
            case CoreID.AttUp:
                coreStat.coreDamage += value;
                break;
            case CoreID.PrimaryAttUp:
                coreStat.primaryDamage += value;
                break;
            case CoreID.SecondaryAttUp:
                coreStat.secondaryDamage += value;
                break;
            case CoreID.KnifeAttUp:
                coreStat.meleeDamage += value;
                break;
            case CoreID.GrenadeAttUp:
                coreStat.grenadeDamage += value;
                break;

            // Defense
            case CoreID.DefUp:
                coreStat.coreDefense += value;
                break;
            case CoreID.HpRegenUp:
                coreStat.coreHpRegion += value;
                break;
            case CoreID.MaxHpUp:
                coreStat.coreHp += value;
                break;

            // Utility
            case CoreID.MovementSpeedUp:
                coreStat.coreMovementSpeed += value;
                break;
            case CoreID.CoinDropChanceUp:
                coreStat.coinDropChance += value;
                break;
            case CoreID.ItemDropChanceUp:
                coreStat.itemDropChance += value;
                break;
            case CoreID.CoinGetRangeUp:
                coreStat.coinDropRange += value;
                break;
            case CoreID.GrenadeCoolReduce:
                coreStat.grenadeCooldown += value;
                break;
            case CoreID.GrenadeRangeUp:
                coreStat.grenadeRange += value;
                break;

            default:
                Debug.LogWarning($"[CoreApplier] 알 수 없는 CoreID: {data.coreID}");
                break;
        }

        // TODO: PlayerStat.Instance.ApplyCoreStat(coreStat); 여기에 반영할 수도 있음
    }

    public CoreStat GetCoreStat()
    {
        return coreStat;
    }
}
