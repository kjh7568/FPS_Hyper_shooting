using UnityEngine;

public class UpgradeWeaponSystem : MonoBehaviour
{
    public Weapon currentWeapon;

    public void OnClick_LevelUp()
    {
        if (TryUpgradeLevel() == false)
            Debug.Log("[UpgradeSystem] 레벨업 불가: 현재 등급 범위를 벗어났습니다. 등급업을 해주세요.");
    }

    public void OnClick_RankUp()
    {
        if (TryUpgradeGrade() == false)
            Debug.Log("[UpgradeSystem] 등급업 불가: 이미 최고 등급입니다.");
    }

    // 레벨업 시도: 특수효과는 변경하지 않고 레벨만 올림
    private bool TryUpgradeLevel()
    {
        const int cost = 500;

        if (currentWeapon == null)
        {
            Debug.LogWarning("[UpgradeSystem] 무기 없음");
            return false;
        }

        int curr = currentWeapon.currentLevel;
        int max  = currentWeapon.data.GetMaxLevelForGrade();

        if (curr >= max)
        {
            Debug.LogWarning("[UpgradeSystem] 이미 최대 레벨입니다.");
            return false;
        }

        if (Player.localPlayer.coin < cost)
        {
            Debug.LogWarning("[UpgradeSystem] 코인이 부족합니다.");
            return false;
        }

        Player.localPlayer.coin -= cost;

        currentWeapon.ApplyLevel(curr + 1);
        Debug.Log($"[무기 레벨업] {currentWeapon.data.weaponName} Lv.{curr} → Lv.{curr + 1}");
        return true;
    }


    // 등급업 시도: 누적으로 한 줄씩 옵션 추가
    private bool TryUpgradeGrade()
    {
        const int cost = 1000;

        if (currentWeapon == null)
        {
            Debug.LogWarning("[UpgradeSystem] 무기 없음");
            return false;
        }

        var data  = currentWeapon.data;
        var grade = data.grade;
        int curr  = currentWeapon.currentLevel;
        int max   = data.GetMaxLevelForGrade();

        if (curr < max)
        {
            Debug.LogWarning($"[UpgradeSystem] Lv.{curr}에서는 등급업 불가. 먼저 Lv.{max}까지 레벨업하세요.");
            return false;
        }

        if (grade == WeaponGrade.Legendary)
        {
            Debug.LogWarning("[UpgradeSystem] 이미 최고 등급입니다.");
            return false;
        }

        if (Player.localPlayer.coin < cost)
        {
            Debug.LogWarning("[UpgradeSystem] 코인이 부족합니다.");
            return false;
        }

        Player.localPlayer.coin -= cost;

        // 등급 상승
        data.grade = grade + 1;
        int minLevel = data.GetMinLevelForGrade();
        currentWeapon.ApplyLevel(minLevel);

        // 새 옵션 한 개만 누적 추가
        var newOpt = currentWeapon.AddRandomEffect();
        if (newOpt.HasValue)
        {
            WeaponManager.instance.ApplyWeaponOption(newOpt.Value);
            Debug.Log($"[옵션 추가] {newOpt.Value} 효과가 누적 적용되었습니다.");
        }

        Debug.Log($"[등급업 성공] {data.weaponName} → {data.grade}, Lv.{minLevel}");
        return true;
    }

}
