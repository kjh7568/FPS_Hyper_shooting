using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGunData", menuName = "Gun/Create New GunData")]
public class GunDataSO : ScriptableObject
{
    public string gunName;
    public GunGrade grade;
    public int maxAmmo;
    public List<GunLevelStat> levelStats;
    public GunType gunType;
    
    public List<GunSpecialEffect> possibleEffects;

    // 등급별 시작 / 최대 레벨 정의
    private static readonly Dictionary<GunGrade, (int min, int max)> gradeLevelLimits = new()
    {
        { GunGrade.Normal,    (1, 3) },
        { GunGrade.Rare,      (3, 5) },
        { GunGrade.Epic,      (5, 7) },
        { GunGrade.Legendary, (7, 10) }
    };
    public int GetMaxLevelForGrade()
    {
        return grade switch
        {
            GunGrade.Normal => 3,
            GunGrade.Rare => 5,
            GunGrade.Epic => 7,
            GunGrade.Legendary => 10,
            _ => 3
        };
    }
    public GunLevelStat GetStatByLevel(int level)
    {
        var (min, max) = gradeLevelLimits[grade];

        if (level < min)
        {
            level = min;
        }
        else if (level > max)
        {
            level = max;
        }

        foreach (var stat in levelStats)
        {
            if (stat.level == level)
                return stat;
        }
        // Debug.LogError($"[GunDataSO] {gunName} 레벨 {level} 데이터가 존재하지 않음.");
        return levelStats.Count > 0 ? levelStats[0] : default;
    }
}
public enum GunGrade
{
    Normal, Rare, Epic, Legendary
}
[System.Serializable]
public struct GunLevelStat
{
    public int level;
    public float damage;
    public float fireRate;
    public float reloadTime;
}
public enum GunSpecialEffect
{
    // 대쉬 쿨타임 5% 감소
    DashCooldownReduction,   
    
    // 재장전 시간 5% 감소
    ReloadSpeedReduction,    
    
    // 공격력 5% 증가
    MultiplierAttackDamage,  
    
    // 이동 속도 10% 증가
    MultiplierMovementSpeed,

    // 치명타 확률 5% 증가
    IncreaseCriticalChance,

    // 치명타 피해 10% 증가
    IncreaseCriticalDamage,

    // 아이템 드롭 확률 10% 증가
    IncreaseItemDropRate
    
}
public enum GunType
{
    Rifle,
    Sniper,
    Shotgun,
    Pistol,
    Grenade,
    Knife,
    UMP,
}



