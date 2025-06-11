using UnityEngine;

[System.Serializable]
public class AugmentStat
{
    public float moveSpeedBonus;
    public float maxHealthBonus;
    public float dashCooldownReduction;

    public void Apply(AugmentData data)
    {
        switch (data.Type)
        {
            case AugmentType.MoveSpeedUp:
                moveSpeedBonus += data.Value;
                Debug.Log($"[증강 저장] 이속 +{data.Value} → 누적: {moveSpeedBonus}");
                break;

            case AugmentType.MaxHealthUp:
                maxHealthBonus += data.Value;
                Debug.Log($"[증강 저장] 체력 +{data.Value} → 누적: {maxHealthBonus}");
                break;

            case AugmentType.DashCooldownDown:
                dashCooldownReduction += data.Value;
                Debug.Log($"[증강 저장] 쿨다운 -{data.Value} → 누적: {dashCooldownReduction}");
                break;

            default:
                Debug.LogWarning($"알 수 없는 증강 타입: {data.Type}");
                break;
        }
    }

    public void PrintAll()
    {
        Debug.Log(
            $"[누적 증강 상태]\n" +
            $"이속 증가: {moveSpeedBonus}\n" +
            $"최대 체력 증가: {maxHealthBonus}\n" +
            $"대시 쿨타임 감소: {dashCooldownReduction}"
        );
    }
}
