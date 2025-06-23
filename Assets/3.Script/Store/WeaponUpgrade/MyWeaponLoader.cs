using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MyWeaponLoader : MonoBehaviour
{
    [Header("대상 슬롯 지정")]
    public WeaponManager.WeaponSlot targetSlot;

    [Header("업그레이드 시스템")]
    public UpgradeWeaponSystem upgradeSystem;

    [Header("UI")]
    public TMP_Text itemNameText;
    public TMP_Text itemTierText;
    public TMP_Text itemLevelText;
    public Image itemImage;

    [Header("스탯 패널")]
    public TMP_Text damageText;
    public TMP_Text fireRateText;
    public TMP_Text magazineText;
    public TMP_Text reloadTimeText;

    [Header("옵션 텍스트")]
    public List<TMP_Text> optionTexts;

    public void LoadWeapon()
    {
        var wc = WeaponManager.instance.GetWeaponBySlot(targetSlot);
        if (wc == null || wc.weapon == null)
        {
            Clear();
            return;
        }
        upgradeSystem.currentWeapon = wc.weapon;
        RefreshUI();
    }

    public void Clear()
    {
        upgradeSystem.currentWeapon = null;

        itemNameText.text = "";
        itemTierText.text = "";
        itemLevelText.text = "";
        itemImage.sprite = null;

        damageText.text = "";
        fireRateText.text = "";
        magazineText.text = "";
        reloadTimeText.text = "";

        foreach (var txt in optionTexts)
        {
            txt.text = "";
            txt.gameObject.SetActive(false);
        }
    }

    public void RefreshUI()
    {
        var weapon = upgradeSystem.currentWeapon;
        if (weapon == null) return;

        var stat = weapon.currentStat;

        itemNameText.text    = weapon.data.weaponName;
        itemTierText.text    = weapon.grade.ToString();
        itemLevelText.text   = $"Lv. {weapon.currentLevel}";
        itemImage.sprite     = weapon.data.weaponImage;

        damageText.text      = $"{stat.damage}";
        fireRateText.text    = $"{stat.fireRate}";
        magazineText.text    = $"{stat.magazine}";
        reloadTimeText.text  = $"{stat.reloadTime}";

        SetOptionDescriptions(weapon);
    }

    private void SetOptionDescriptions(Weapon weapon)
    {
        // 모든 option 텍스트 비활성화
        foreach (var txt in optionTexts)
        {
            txt.gameObject.SetActive(false);
        }

        // 옵션별로 한글 설명 활성화
        int idx = 0;
        foreach (var option in weapon.options)
        {
            if (idx >= optionTexts.Count) break;
            var txt = optionTexts[idx];
            txt.gameObject.SetActive(true);

            switch (option)
            {
                case WeaponSpecialEffect.DashCooldownReduction:
                    txt.text = "• 대시 쿨타임이 10% 감소합니다";
                    break;
                case WeaponSpecialEffect.ReloadSpeedReduction:
                    txt.text = "• 재장전 속도가 10% 빨라집니다";
                    break;
                case WeaponSpecialEffect.MultiplierAttackDamage:
                    txt.text = "• 공격력이 5% 증가합니다";
                    break;
                case WeaponSpecialEffect.MultiplierMovementSpeed:
                    txt.text = "• 이동 속도가 10% 증가합니다";
                    break;
                case WeaponSpecialEffect.IncreaseCriticalChance:
                    txt.text = "• 치명타 확률이 10% 증가합니다";
                    break;
                case WeaponSpecialEffect.IncreaseCriticalDamage:
                    txt.text = "• 치명타 데미지가 10% 증가합니다";
                    break;
                case WeaponSpecialEffect.IncreaseItemDropRate:
                    txt.text = "• 아이템 드롭 확률이 10% 증가합니다";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            idx++;
        }
    }

    public void OnClick_Refresh()
    {
        RefreshUI();
    }
}
