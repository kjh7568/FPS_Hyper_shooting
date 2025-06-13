using System;
using UnityEngine;

[Serializable]
public class ArmorStat
{
    [Header("방어력 관련")]
    public float totalDefense;

    public void ResetStat()
    {
        totalDefense = 0f;
    }
    public void Add(ArmorStat other)
    {
        totalDefense += other.totalDefense;
    }
    public void AddFromLevelStat(ArmorLevelStat stat)
    {
        totalDefense += stat.defense;
    }
}