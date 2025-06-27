using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class CoreStat
{
    //곱연산만 1f로 초기화
    
    [Header("방어 스탯")]
    public float multiplierDefense = 1f;
    public float plusHp;
    public float hpRegion;

    [Header("공격 스탯")]
    public float multiplierAllDamage = 1f;
    public float increasePrimaryDamage;
    public float increaseSecondaryDamage;
    public float increaseMeleeDamage;
    public float increaseGrenadeDamage;

    [Header("유틸리티 스탯")]
    public float increaseCoinGain = 1f;
    public float increaseCoinDropRange = 1f;
    public float increaseItemDropChance;
    public float multiplierMovementSpeed = 1f;
    public float increaseCooldown;
    public float increaseExplosionRange;
}