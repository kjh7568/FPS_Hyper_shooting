using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGunData", menuName = "Gun/Create New GunData")]
public class GunDataSO : ScriptableObject
{
    public string gunName;
    public GunGrade grade;
    public int maxAmmo;
    public List<GunLevelStat> levelStats;

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
            Debug.LogWarning($"{gunName} 등급({grade})은 {min}레벨부터 시작. 요청한 {level} → {min}으로 자동 보정됨.");
            level = min;
        }
        else if (level > max)
        {
            Debug.LogWarning($"{gunName} 등급({grade})은 {max}레벨이 최대. 요청한 {level} → {max}으로 자동 보정됨.");
            level = max;
        }

        foreach (var stat in levelStats)
        {
            if (stat.level == level)
                return stat;
        }

        Debug.LogError($"[GunDataSO] {gunName} 레벨 {level} 데이터가 존재하지 않음.");
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

