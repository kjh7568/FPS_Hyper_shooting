using UnityEngine;
public enum AugmentType
{
    MoveSpeedUp,
    DashCooldownDown,
    MaxHealthUp,
    AttackPowerUp,
    ReloadSpeedUp // ✅ 직관적인 이름으로 통일
}


public class AugmentData
{
    public int ID { get; set; }
    public AugmentType Type { get; set; }
    public float Value { get; set; }
    public string Description { get; set; }
}
