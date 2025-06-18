using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct ArmorLevelStat
{
    public int level;
    public float defense;
}

public enum ArmorType
{
    Helmet,
    BodyArmor,
    Gloves,
    Boots
}

public enum ArmorGrade
{
    Common,
    Rare,
    Epic,
    Legendary
}

public enum SpecialEffect
{
    DashCooldownReduction,   // 대쉬 쿨타임 5% 감소
    MultiplierDefense,       // 방어력 10% 증가
    IncreaseHealth,          // 체력 20 증가
    MultiplierHealth,        // 체력 10% 증폭
    ReloadSpeedReduction,    // 재장전 시간 5% 감소
    MultiplierAttackDamage,  // 공격력 5% 증가
    MultiplierMovementSpeed  // 이동 속도 10% 증가
}

[CreateAssetMenu(fileName = "NewArmorData", menuName = "Armor/Create New ArmorData")]
public class ArmorDataSO : ScriptableObject
{
    [Header("기본 정보")] 
    public string armorName;
    public ArmorType armorType;
    public ArmorGrade grade;
    public Sprite armorImage;

    [Header("레벨별 스탯")]
    public List<ArmorLevelStat> levelStats;

    [Header("가능한 특수효과 풀")]
    public List<SpecialEffect> possibleEffects;

    // 등급별 시작 / 최대 레벨 정의
    private static readonly Dictionary<ArmorGrade, (int min, int max)> gradeLevelLimits = new()
    {
        { ArmorGrade.Common,    (1, 3) },
        { ArmorGrade.Rare,      (3, 5) },
        { ArmorGrade.Epic,      (5, 7) },
        { ArmorGrade.Legendary, (7, 10) }
    };

    public int GetMaxLevelForGrade()
    {
        return gradeLevelLimits.TryGetValue(grade, out var limits) ? limits.max : 3;
    }

    public int GetMinLevelForGrade()
    {
        return gradeLevelLimits.TryGetValue(grade, out var limits) ? limits.min : 1;
    }

    public ArmorLevelStat GetStatByLevel(int level)
    {
        var (min, max) = gradeLevelLimits[grade];

        if (level < min) level = min;
        if (level > max) level = max;

        foreach (var stat in levelStats)
        {
            if (stat.level == level)
                return stat;
        }

        Debug.LogError($"[ArmorDataSO] {armorName} 레벨 {level} 데이터가 존재하지 않음.");
        return levelStats.Count > 0 ? levelStats[0] : default;
    }
}