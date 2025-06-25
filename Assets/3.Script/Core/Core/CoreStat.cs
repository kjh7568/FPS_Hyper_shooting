using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class CoreStat
{
    //곱연산만 1f로 초기화
    
    [Header("방어 스탯")]
    public float coreDefense = 1f;
    public float coreHp;
    public float coreHpRegion;

    [Header("공격 스탯")]
    public float coreDamage = 1f;
    public float primaryDamage;
    public float secondaryDamage;
    public float meleeDamage;
    public float grenadeDamage;

    [Header("유틸리티 스탯")]
    public float coinGainMultiplier = 1f;
    public float coinDropRange = 1f;
    public float itemDropChance;
    public float coreMovementSpeed = 1f;
    public float grenadeCooldown;
    public float grenadeRange;
}