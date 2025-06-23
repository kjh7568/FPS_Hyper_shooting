using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MyArmorLoader : MonoBehaviour
{
    [Header("대상 방어구 타입")]
    public ArmorType targetType;

    [Header("업그레이드 시스템")]
    public UpgradeArmorSystem upgradeSystem;

    [Header("UI")]
    public TMP_Text itemNameText;
    public TMP_Text itemTierText;
    public TMP_Text itemLevelText;
    public Image   itemImage;

    [Header("스탯 패널")]
    public TMP_Text defenseText;

    [Header("옵션 텍스트")]
    public List<TMP_Text> optionTexts;

    void Start()
    {
        LoadArmor();
    }

    /// 플레이어 인벤토리에서 해당 타입 방어구를 장착하고, UI 갱신
    public void LoadArmor()
    {
        var inv = Player.localPlayer.inventory;

        // 기존에 장착된 방어구가 있으면 그대로, 없으면 UI 클리어
        if (!inv.equippedArmors.TryGetValue(targetType, out Armor armor) || armor == null)
        {
            Clear();
            return;
        }

        // 1) 인벤토리 장착 (스탯 반영용)
        inv.EquipArmor(armor);

        // 2) 업그레이드 시스템에 현재 방어구 연결
         upgradeSystem.currentArmor = armor;

        // 3) UI 갱신
        RefreshUI();
    }
    public void Clear()
    {
        itemNameText.text   = "";
        itemTierText.text   = "";
        itemLevelText.text  = "";
        itemImage.sprite    = null;
        defenseText.text    = "";

        foreach (var txt in optionTexts)
        {
            txt.gameObject.SetActive(false);
            txt.text = "";
        }
    }
    public void RefreshUI()
    {
         var armor = upgradeSystem.currentArmor;
         if (armor == null) return;
        
         // 기본 정보
         itemNameText.text  = armor.data.armorName;
         itemTierText.text  = armor.data.grade.ToString();
         itemLevelText.text = $"Lv. {armor.currentLevel}";
         itemImage.sprite   = armor.data.armorImage;
        
         // 스탯
         defenseText.text = $"{armor.currentStat.defense}";
        
         // 특수 옵션
         SetOptionDescriptions(armor);
    }

    private void SetOptionDescriptions(Armor armor)
    {
        // 모두 숨기기
        foreach (var txt in optionTexts)
        {
            txt.gameObject.SetActive(false);
        }

        // 순서대로 활성화 & 한글 설명
        int idx = 0;
        foreach (var option in armor.options)
        {
            if (idx >= optionTexts.Count) break;
            var txt = optionTexts[idx];
            txt.gameObject.SetActive(true);

            switch (option)
            {
                case SpecialEffect.DashCooldownReduction:
                    txt.text = "• 대시 쿨타임이 5% 감소합니다";
                    break;
                case SpecialEffect.MultiplierDefense:
                    txt.text = "• 방어력이 10% 증가합니다";
                    break;
                case SpecialEffect.IncreaseHealth:
                    txt.text = "• 최대 체력이 20 증가합니다";
                    break;
                case SpecialEffect.MultiplierHealth:
                    txt.text = "• 최대 체력이 10% 증가합니다";
                    break;
                case SpecialEffect.ReloadSpeedReduction:
                    txt.text = "• 재장전 시간이 5% 감소합니다";
                    break;
                case SpecialEffect.MultiplierAttackDamage:
                    txt.text = "• 공격력이 5% 증가합니다";
                    break;
                case SpecialEffect.MultiplierMovementSpeed:
                    txt.text = "• 이동 속도가 10% 증가합니다";
                    break;
            }
            idx++;
        }
    }

     public void OnClick_RefreshUI()
     {
             // 레벨업은 옵션 변경 없음 → UI만 갱신
             RefreshUI();
     }
    
}
