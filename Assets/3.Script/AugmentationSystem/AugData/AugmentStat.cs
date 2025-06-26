using UnityEngine;

[System.Serializable]
public class AugmentStat
{
    // 아이템
    public float moveSpeedBonus;
    public float maxHealthBonus;
    public float dashCooldownReduction;
    public float attackPowerBonus;  
    public float reloadSpeedBonus;
    public float defenseBonus;
    // 희귀
   // public float reloadDamageBonus;
   // public float reloadBuffDuration = 1.5f; // (필요시 TSV 확장)

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
                Debug.Log($"[증강 저장] 대시 쿨다운 감소 +{data.Value} → 누적: {dashCooldownReduction}");
                break;

            case AugmentType.AttackPowerUp:
                attackPowerBonus += data.Value;
                Debug.Log($"[증강 저장] 공격력 +{data.Value} → 누적: {attackPowerBonus}");
                break;

            case AugmentType.ReloadSpeedUp:
                reloadSpeedBonus += data.Value;
                Debug.Log($"[증강 저장] 재장전 속도 증가 +{data.Value} → 누적: {reloadSpeedBonus}");
                break;

            case AugmentType.DefenseUp:
                defenseBonus = data.Value;
                Debug.Log($"[증강 저장] 방어력 증가{data.Value} → 누적: {defenseBonus}");
                break;

            default:
                Debug.LogWarning($"[경고] 알 수 없는 증강 타입: {data.Type}");
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
            $"재장전 속도 증가: {reloadSpeedBonus}\n" +
            $"방어력 증가: {defenseBonus} "
        );
    }
}
