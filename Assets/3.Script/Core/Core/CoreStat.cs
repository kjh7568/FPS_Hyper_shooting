using UnityEngine.Serialization;

[System.Serializable]
public class CoreStat
{
    // Defense
    public float coreDefense;
    public float coreHp;
    public float coreHpRegion;

    // Attack
    public float coreDamage;
    public float primaryDamage;
    public float secondaryDamage;
    public float meleeDamage;
    public float grenadeDamage;

    // Utility
    public float coinGainMultiplier;
    public float coinDropRange;
    public float itemDropChance;
    public float coreMovementSpeed;
    public float grenadeCooldown;
    public float grenadeRange;
}