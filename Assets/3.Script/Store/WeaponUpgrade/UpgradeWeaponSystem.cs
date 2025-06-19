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

    // 레벨업 시도: 현재 등급의 maxLevel을 넘지 않을 때만 레벨업
    private bool TryUpgradeLevel()
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning("[UpgradeSystem] 무기 없음");
            return false;
        }

        int curr = currentWeapon.currentLevel;
        int max = currentWeapon.data.GetMaxLevelForGrade();

        // 아직 레벨업 가능 구간이면
        if (curr < max)
        {
            currentWeapon.ApplyLevel(curr + 1);
            Debug.Log($"[무기 레벨업] {currentWeapon.data.weaponName} Lv.{curr} → Lv.{curr + 1}");
            return true;
        }

        // 그 외(== max)에선 레벨업 불가
        return false;
    }

    // 등급업 시도: 현재 등급의 maxLevel(==grade end)에서만 실행
    private bool TryUpgradeGrade()
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning("[UpgradeSystem] 무기 없음");
            return false;
        }

        var data = currentWeapon.data;
        var grade = data.grade;
        int curr = currentWeapon.currentLevel;
        int max = data.GetMaxLevelForGrade();

        // 만약 아직 maxLevel에 도달 못 했으면 등급업 불가
        if (curr < max)
        {
            Debug.LogWarning($"[UpgradeSystem] Lv.{curr}에서는 등급업 불가. 먼저 Lv.{max}까지 레벨업하세요.");
            return false;
        }

        // Legendary 이상이면 불가
        if (grade == WeaponGrade.Legendary)
        {
            Debug.LogWarning("[UpgradeSystem] 이미 최고 등급입니다.");
            return false;
        }

        // 등급 올리고 레벨 초기화
        var nextGrade = grade + 1;
        data.grade = nextGrade;

        // 새 등급의 최소레벨(겹치는 구간) 유지
        int minLevel = data.GetMinLevelForGrade();
        currentWeapon.GenerateRandomEffects();
        currentWeapon.ApplyLevel(minLevel);

        Debug.Log($"[등급업 성공] {data.weaponName} → {nextGrade}, Lv.{minLevel}");
        return true;
    }
}
