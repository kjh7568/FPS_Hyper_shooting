using UnityEngine;

public class CoreManager : MonoBehaviour
{
    public static CoreManager Instance;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
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

        var coreStat = Player.localPlayer.coreStat;
        
        switch (data.coreID)
        {
            // Attack
            case CoreID.AttUp:
                coreStat.coreDamage = value;
                break;
            case CoreID.PrimaryAttUp:
                coreStat.primaryDamage = value;
                break;
            case CoreID.SecondaryAttUp:
                coreStat.secondaryDamage = value;
                break;
            case CoreID.KnifeAttUp:
                coreStat.meleeDamage = value;
                break;
            case CoreID.GrenadeAttUp:
                coreStat.grenadeDamage = value;
                break;

            // Defense
            case CoreID.DefUp:
                coreStat.coreDefense = value;
                break;
            case CoreID.HpRegenUp:
                coreStat.coreHpRegion += value;
                break;
            case CoreID.MaxHpUp:
                coreStat.coreHp += value;
                break;

            // Utility
            case CoreID.MovementSpeedUp:
                coreStat.coreMovementSpeed = value;
                break;
            case CoreID.CoinDropChanceUp:
                coreStat.coinGainMultiplier = value;
                break;
            case CoreID.ItemDropChanceUp:
                coreStat.itemDropChance = value;
                break;
            case CoreID.CoinGetRangeUp:
                coreStat.coinDropRange = value;
                break;
            case CoreID.GrenadeCoolReduce:
                coreStat.grenadeCooldown = value;
                break;
            case CoreID.GrenadeRangeUp:
                coreStat.grenadeRange = value;
                break;

            default:
                Debug.LogWarning($"[CoreApplier] 알 수 없는 CoreID: {data.coreID}");
                break;
        }

        // TODO: PlayerStat.Instance.ApplyCoreStat(coreStat); 여기에 반영할 수도 있음
    }
}
