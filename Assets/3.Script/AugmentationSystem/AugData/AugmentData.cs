using UnityEngine;
public enum AugmentType
{
    MoveSpeedUp,
    DashCooldownDown,
    MaxHealthUp,
    AttackPowerUp,
    ReloadSpeedUp,
    ReloadDamageBuff 
}


public class AugmentData
{
    public int ID { get; set; }
    public AugmentType Type { get; set; }
    public float Value { get; set; }
    public string Description { get; set; }
}
