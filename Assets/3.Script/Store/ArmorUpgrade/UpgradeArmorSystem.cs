using UnityEngine;

public class UpgradeArmorSystem : MonoBehaviour
{
    public Armor currentArmor;

    /// 레벨업 버튼에 연결
    public void OnClick_LevelUp()
    {
        if (TryUpgradeLevel() == false)
            Debug.Log("[UpgradeArmorSystem] 레벨업 불가: 현재 등급 범위를 벗어났습니다. 등급업을 해주세요.");
    }

    /// 등급업 버튼에 연결
    public void OnClick_RankUp()
    {
        if (TryUpgradeGrade() == false)
            Debug.Log("[UpgradeArmorSystem] 등급업 불가: 이미 최고 등급입니다.");
    }
    private int GetArmorLevelUpCost(int currentLevel)
    {
        // 1 → 2 = 100, 2 → 3 = 200, ..., 9 → 10 = 900
        return currentLevel * 100;
    }
    private int GetArmorGradeUpCost(ArmorGrade grade)
    {
        return grade switch
        {
            ArmorGrade.Common    => 300,   // → Rare
            ArmorGrade.Rare      => 500,   // → Epic
            ArmorGrade.Epic      => 1000,  // → Legendary
            _ => 0  // Legendary는 업그레이드 불가
        };
    }


    private bool TryUpgradeLevel()
    {
        
        if (currentArmor == null)
        {
            Debug.LogWarning("[UpgradeArmorSystem] 방어구 없음");
            return false;
        }

        int curr = currentArmor.currentLevel;
        int next = curr + 1;
        int max  = currentArmor.data.GetMaxLevelForGrade();

        // 먼저 등급별 최대값 검사
        if (curr >= max)
            return false;

        // **레벨 데이터 존재 여부 검사**
        bool hasData = currentArmor.data.levelStats.Exists(s => s.level == next);
        if (!hasData)
        {
            Debug.LogWarning($"[UpgradeArmorSystem] Lv.{next} 데이터가 존재하지 않습니다.");
            return false;
        }
        int cost = GetArmorLevelUpCost(currentArmor.currentLevel);
        if (Player.localPlayer.coin < cost)
        {
            Debug.LogWarning("[UpgradeArmorSystem] 코인이 부족합니다.");
            return false;
        }

        Player.localPlayer.coin -= cost;

        // 데이터가 있으면 레벨업 실행
        currentArmor.ApplyLevel(next);
        Debug.Log($"[방어구 레벨업] {currentArmor.data.armorName} Lv.{curr} → Lv.{next}");
        return true;
    }

    private bool TryUpgradeGrade()
    {
        
        if (currentArmor == null)
        {
            Debug.LogWarning("[UpgradeArmorSystem] 방어구 없음");
            return false;
        }

        var data  = currentArmor.data;
        var grade = data.grade;
        int curr  = currentArmor.currentLevel;
        int max   = data.GetMaxLevelForGrade();

        if (curr < max)
        {
            Debug.LogWarning($"[UpgradeArmorSystem] Lv.{curr}에서는 등급업 불가. 먼저 Lv.{max}까지 레벨업하세요.");
            return false;
        }

        if (grade == ArmorGrade.Legendary)
        {
            Debug.LogWarning("[UpgradeArmorSystem] 이미 최고 등급입니다.");
            return false;
        }
        int cost = GetArmorGradeUpCost(grade);
        
        if (Player.localPlayer.coin < cost)
        {
            Debug.LogWarning("[UpgradeSystem] 코인이 부족합니다.");
            return false;
        }
        Player.localPlayer.coin -= cost;
        
        // 등급 상승 & 레벨 초기화
        data.grade = grade + 1;
        int minLevel = data.GetMinLevelForGrade();
        currentArmor.ApplyLevel(minLevel);

        // 새 옵션 한 개만 누적 추가
        var newOpt = currentArmor.AddRandomEffect();
        if (newOpt.HasValue)
        {
            // 인벤토리에 바로 적용
            Player.localPlayer.inventory.ApplyEquipmentOption(newOpt.Value);

            Debug.Log($"[옵션 추가] {newOpt.Value} 효과가 누적 적용되었습니다.");
            // 옵션 적용 후 현재 스탯 확인용
            Player.localPlayer.inventory.EquipmentStat.PrintOption();
        }

        Debug.Log($"[등급업 성공] {data.armorName} → {data.grade}, Lv.{minLevel}");
        return true;
    }

}
