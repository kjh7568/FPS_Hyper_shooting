using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class EquipmentStat
{
    [FormerlySerializedAs("increaseHealth")] public float plusHp = 0;
    [FormerlySerializedAs("multiplierHealth")] public float increaseHealth = 1f;
    public float totalDefense = 0;
    [FormerlySerializedAs("multiplierDefense")] public float increaseDefense = 1f;
    [FormerlySerializedAs("dashCooldownReduction")] public float increaseDashCooldown;
    [FormerlySerializedAs("reloadSpeedReduction")] public float increaseReloadSpeed;
    [FormerlySerializedAs("IncreaseAttack")] [FormerlySerializedAs("multiplierAttack")] public float increaseAttack = 1f;
    [FormerlySerializedAs("multiplierMovementSpeed")] public float increaseMovementSpeed = 1f;
    public int criticalChance = 0;
    [FormerlySerializedAs("multiplierCriticalDamage")] public float criticalDamage = 1.5f;
    [FormerlySerializedAs("multiplierRareItemChance")] public float increaseItemDropChance = 1f; 
}