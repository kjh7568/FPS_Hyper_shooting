using UnityEngine;

public class UpgradeWeaponSystem : MonoBehaviour
{
    public Weapon currentWeapon;

    public bool TryUpgrade()
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning("[UpgradeSystem] 무기 없음");
            return false;
        }

        int nextLevel = currentWeapon.currentLevel + 1;
        int maxLevel = currentWeapon.data.GetMaxLevelForGrade();

        if (nextLevel <= maxLevel)
        {
            currentWeapon.ApplyLevel(nextLevel);
            Debug.Log($"[무기 레벨업] {currentWeapon.data.weaponName} → Lv.{nextLevel}");
            return true;
        }
        else
        {
            return TryUpgradeGrade();
        }
    }

    public bool TryUpgradeGrade()
    {
        var currentGrade = currentWeapon.data.grade;
        if (currentGrade == WeaponGrade.Legendary)
        {
            Debug.Log("[등급업] 이미 최고 등급입니다.");
            return false;
        }

        var nextGrade = currentGrade + 1;
        currentWeapon.data.grade = nextGrade;

        currentWeapon.GenerateRandomEffects();
        currentWeapon.ApplyLevel(currentWeapon.data.GetMinLevelForGrade());

        Debug.Log($"[등급업] {currentWeapon.data.weaponName} → {nextGrade}");
        return true;
    }
}
