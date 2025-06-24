using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    public float health;
    public float maxHealth;    // 기본 최대 체력
    public float moveSpeed;
    public float dashCoolTime;
    public float pickupRadius;

    // ▶ 총 최대체력 계산 (기본 + 코어 + 방어구)
    public float GetTotalMaxHealth()
    {
        // 방어구 스탯
        var armor = Player.localPlayer.inventory.EquipmentStat;
        // 코어 스탯
        var coreHp = CoreApplier.Instance.GetCoreStat().coreHp;

        return (maxHealth + coreHp + armor.increaseHealth)
               * armor.multiplierHealth;
    }

    // ▶ 체력 회복 메서드: CoreApplier 에서 초당 amount 회복 호출 시
    public void Regenerate(float amount)
    {
        float totalMax = GetTotalMaxHealth();

        if (health >= totalMax) return;

        health += amount;
        if (health > totalMax)
            health = totalMax;
    }
}