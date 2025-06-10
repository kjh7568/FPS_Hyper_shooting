using UnityEngine;

[CreateAssetMenu(fileName = "NewAugment", menuName = "Augmentation/Augment")]
public class AugmentData : ScriptableObject
{
    public string augmentName;
    public string description;
    public Sprite icon;

    public float moveSpeedBonus; // 기본 이동속도 증가
    // 향후 특수 능력 bool/enum도 추가 가능
}