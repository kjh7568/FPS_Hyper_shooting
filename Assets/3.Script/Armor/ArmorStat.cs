using System;
using UnityEngine;

[Serializable]
public class ArmorStat
{
    public float increaseHealth = 0;
    public float multiplierHealth = 1f;
    public float totalDefense = 0;
    public float multiplierDefense = 1f;
    public float dashCooldownReduction = 1f;
    public float reloadSpeedReduction = 1f;
    public float multiplierAttack = 1f;
    public float multiplierMovementSpeed = 1f;

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