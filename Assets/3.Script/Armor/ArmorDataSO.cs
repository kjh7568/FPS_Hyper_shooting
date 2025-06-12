using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArmorData", menuName = "Armor/Create New ArmorData")]
public class ArmorDataSO : ScriptableObject
{
    public string armorName;
    public ArmorType armorType;
    public ArmorGrade grade;
    public List<ArmorLevelStat> levelStats;

    public Sprite icon;
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
        return grade switch
        {
            ArmorGrade.Common => 3,
            ArmorGrade.Rare => 5,
            ArmorGrade.Epic => 7,
            ArmorGrade.Legendary => 10,
            _ => 3
        };
    }

    public ArmorLevelStat GetStatByLevel(int level)
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

        Debug.LogError($"[ArmorDataSO] {armorName} 레벨 {level} 데이터가 존재하지 않음.");
        return levelStats.Count > 0 ? levelStats[0] : default;
    }
}
public enum ArmorType
{
    Helmet,
    Chestplate,
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

[System.Serializable]
public struct ArmorLevelStat
{
    public int level;
    public float defense;
}