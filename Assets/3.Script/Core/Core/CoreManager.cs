using UnityEngine;
using UnityEngine.Rendering.UI;

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
                coreStat.multiplierAllDamage = value;
                break;
            case CoreID.PrimaryAttUp:
                coreStat.increasePrimaryDamage = value;
                break;
            case CoreID.SecondaryAttUp:
                coreStat.increaseSecondaryDamage = value;
                break;
            case CoreID.KnifeAttUp:
                coreStat.increaseMeleeDamage = value;
                break;
            case CoreID.GrenadeAttUp:
                coreStat.increaseGrenadeDamage = value;
                break;

            // Defense
            case CoreID.DefUp:
                coreStat.multiplierDefense = value;
                break;
            case CoreID.HpRegenUp:
                coreStat.hpRegion += value;
                break;
            case CoreID.MaxHpUp:
                coreStat.plusHp += value;
                break;

            // Utility
            case CoreID.MovementSpeedUp:
                coreStat.multiplierMovementSpeed = value;
                break;
            case CoreID.CoinDropChanceUp:
                coreStat.increaseCoinGain = value;
                break;
            case CoreID.ItemDropChanceUp:
                coreStat.increaseItemDropChance = value;
                break;
            case CoreID.CoinGetRangeUp:
                coreStat.increaseCoinDropRange = value;
                break;
            case CoreID.GrenadeCoolReduce:
                coreStat.increaseCooldown = value;
                break;
            case CoreID.GrenadeRangeUp:
                coreStat.increaseExplosionRange = value;
                break;
        }

        // TODO: PlayerStat.Instance.ApplyCoreStat(coreStat); 여기에 반영할 수도 있음
    }
}