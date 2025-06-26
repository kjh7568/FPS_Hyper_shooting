using UnityEngine;

public enum AugmentType
{
    MoveSpeedUp,
    DashCooldownDown,
    MaxHealthUp,
    AttackPowerUp,
    ReloadSpeedUp,
    DefenseUp,
}

public enum AugmentGrade
{
    Normal,
    Rare,
    Legend
}

public class AugmentData
{
    public int ID { get; set; }
    public AugmentType Type { get; set; }
    public float Value { get; set; }
    public AugmentGrade Grade { get; set; } // 추가됨
    public string Description { get; set; }
    public string IconPath { get; set; }
    public float Weight { get; set; }

    public Sprite LoadIcon()
    {
        if (string.IsNullOrEmpty(IconPath)) return null;
        return Resources.Load<Sprite>(IconPath);
    }
}