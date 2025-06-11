using UnityEngine;

[System.Serializable]
public class AugmentStat
{
    public float moveSpeedBonus;
    public float maxHealthBonus;
    public float dashCooldownReduction;
    public float attackPowerBonus;       // ✅ 추가
    public float reloadSpeedBonus; // ✅ 추가
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
                Debug.Log($"[증강 저장] 쿨다운 +{data.Value} → 누적: {dashCooldownReduction}");
                break;
            
            case AugmentType.AttackPowerUp:
                attackPowerBonus += data.Value;
                Debug.Log($"[증강 저장] 공격력 +{data.Value} → 누적: {attackPowerBonus}");
                break;
            
            case AugmentType.ReloadSpeedUp:
                reloadSpeedBonus += data.Value;
                Debug.Log($"[증강 저장] 재장전 속도 감소 +{data.Value} → 누적: {reloadSpeedBonus}");
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
            $"대시 쿨타임 감소: {dashCooldownReduction}\n" +
            $"공격력 증가: {attackPowerBonus}\n" +
            $"재장전 속도 증가: {reloadSpeedBonus}"
        );
    }
}
