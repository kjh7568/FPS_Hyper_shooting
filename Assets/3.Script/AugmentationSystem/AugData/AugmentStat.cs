using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class AugmentStat
{
    // 아이템
    [FormerlySerializedAs("moveSpeedBonus")] public float increaseMoveSpeed;
    [FormerlySerializedAs("maxHealthBonus")] public float plusHp;
    [FormerlySerializedAs("attackPowerBonus")] public float increaseAttack;  
    [FormerlySerializedAs("defenseBonus")] public float increaseDefense;
    [FormerlySerializedAs("reloadSpeedBonus")] public float increaseReloadSpeed;
    [FormerlySerializedAs("dashCooldownReduction")] public float increaseDashCooldown;
    // 희귀
   // public float reloadDamageBonus;
   // public float reloadBuffDuration = 1.5f; // (필요시 TSV 확장)

    public void Apply(AugmentData data)
    {
        switch (data.Type)
        {
            case AugmentType.MoveSpeedUp:
                increaseMoveSpeed += data.Value;
                Debug.Log($"[증강 저장] 이속 +{data.Value} → 누적: {increaseMoveSpeed}");
                break;

            case AugmentType.MaxHealthUp:
                plusHp += data.Value;
                Debug.Log($"[증강 저장] 체력 +{data.Value} → 누적: {plusHp}");
                break;

            case AugmentType.DashCooldownDown:
                increaseDashCooldown += data.Value;
                Debug.Log($"[증강 저장] 대시 쿨다운 감소 +{data.Value} → 누적: {increaseDashCooldown}");
                break;

            case AugmentType.AttackPowerUp:
                increaseAttack += data.Value;
                Debug.Log($"[증강 저장] 공격력 +{data.Value} → 누적: {increaseAttack}");
                break;

            case AugmentType.ReloadSpeedUp:
                increaseReloadSpeed += data.Value;
                Debug.Log($"[증강 저장] 재장전 속도 증가 +{data.Value} → 누적: {increaseReloadSpeed}");
                break;

            case AugmentType.DefenseUp:
                increaseDefense = data.Value;
                Debug.Log($"[증강 저장] 방어력 증가{data.Value} → 누적: {increaseDefense}");
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
            $"이속 증가: {increaseMoveSpeed}\n" +
            $"최대 체력 증가: {plusHp}\n" +
            $"대시 쿨타임 감소: {increaseDashCooldown}\n" +
            $"공격력 증가: {increaseAttack}\n" +
            $"재장전 속도 증가: {increaseReloadSpeed}\n" +
            $"방어력 증가: {increaseDefense} "
        );
    }
}
