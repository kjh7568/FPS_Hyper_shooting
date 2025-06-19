using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct WeaponLevelStat
{
    public int level;
    public float damage;
    public float fireRate;
    public float reloadTime;
    public int magazine;
}

public enum WeaponType
{
    Rifle,
    Sniper,
    Shotgun,
    Pistol,
    Grenade,
    Knife,
    UMP,
}

public enum WeaponGrade
{
    Common,
    Rare,
    Epic,
    Legendary
}
public enum GunSpecialEffect
{
    DashCooldownReduction,     // 대쉬 쿨타임 5% 감소
    ReloadSpeedReduction,      // 재장전 시간 5% 감소
    MultiplierAttackDamage,    // 공격력 5% 증가
    MultiplierMovementSpeed,   // 이동 속도 10% 증가
    IncreaseCriticalChance,    // 치명타 확률 5% 증가
    IncreaseCriticalDamage,    // 치명타 피해 10% 증가
    IncreaseItemDropRate       // 아이템 드롭 확률 10% 증가
}

[CreateAssetMenu(fileName = "NewGunData", menuName = "Gun/Create New GunData")]
public class WeaponDataSO : ScriptableObject
{
    [Header("기본 정보")] 
    public string weaponName;
    public WeaponType weaponType;
    public WeaponGrade grade;
    public Sprite weaponImage;

    [Header("레벨별 스탯")] 
    public List<WeaponLevelStat> levelStats;

    [Header("가능한 특수효과 풀")] 
    public List<GunSpecialEffect> possibleEffects;

    // 등급별 시작 / 최대 레벨 정의
    private static readonly Dictionary<WeaponGrade, (int min, int max)> gradeLevelLimits = new()
    {
        { WeaponGrade.Common, (1, 3) },
        { WeaponGrade.Rare, (3, 5) },
        { WeaponGrade.Epic, (5, 7) },
        { WeaponGrade.Legendary, (7, 10) }
    };

    public int GetMaxLevelForGrade()
    {
        return gradeLevelLimits.TryGetValue(grade, out var limits) ? limits.max : 3;
    }

    public int GetMinLevelForGrade()
    {
        return gradeLevelLimits.TryGetValue(grade, out var limits) ? limits.min : 1;
    }

    public WeaponLevelStat GetStatByLevel(int level)
    {
        var (min, max) = gradeLevelLimits[grade];

        if (level < min) level = min;
        else if (level > max) level = max;

        foreach (var stat in levelStats)
        {
            if (stat.level == level)
                return stat;
        }

        Debug.LogError($"[GunDataSO] {weaponName} 레벨 {level} 데이터가 존재하지 않음.");
        return levelStats.Count > 0 ? levelStats[0] : default;
    }
    
}