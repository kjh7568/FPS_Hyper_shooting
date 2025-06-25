using System;
using UnityEngine;

[Serializable]
public class EquipmentStat
{
    public float increaseHealth = 0;
    public float multiplierHealth = 1f;
    public float totalDefense = 0;
    public float multiplierDefense = 1f;
    public float dashCooldownReduction = 0;
    public float reloadSpeedReduction = 0;
    public float multiplierAttack = 1f;
    public float multiplierMovementSpeed = 1f;
    public int criticalChance = 0;
    public float multiplierCriticalDamage = 1f;
    public float multiplierRareItemChance = 1f;

    public void PrintOption()
    {
        Debug.Log($"increaseHealth: {increaseHealth}");
        Debug.Log($"multiplierHealth: {multiplierHealth}");
        Debug.Log($"totalDefense: {totalDefense}");
        Debug.Log($"multiplierDefense: {multiplierDefense}");
        Debug.Log($"dashCooldownReduction: {dashCooldownReduction}");
        Debug.Log($"reloadSpeedReduction: {reloadSpeedReduction}");
        Debug.Log($"multiplierAttack: {multiplierAttack}");
        Debug.Log($"multiplierMovementSpeed: {multiplierMovementSpeed}");
        Debug.Log($"criticalChance: {criticalChance}");
        Debug.Log($"multiplierCriticalDamage: {multiplierCriticalDamage}");
        Debug.Log($"multiplierRareItemChance: {multiplierRareItemChance}");
    }
}