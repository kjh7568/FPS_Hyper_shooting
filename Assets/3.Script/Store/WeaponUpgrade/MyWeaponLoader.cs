using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
        // 각 패널 전용 시스템에만 currentWeapon 설정
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
            txt.text = "";
    }

    public void RefreshUI()
    {
        var weapon = upgradeSystem.currentWeapon;
        if (weapon == null) return;

        var stat = weapon.currentStat;

        itemNameText.text = weapon.data.weaponName;
        itemTierText.text = weapon.grade.ToString();
        itemLevelText.text = $"Lv. {weapon.currentLevel}";
        itemImage.sprite = weapon.data.weaponImage;

        damageText.text = $"{stat.damage}";
        fireRateText.text = $"{stat.fireRate}";
        magazineText.text = $"{stat.magazine}";
        reloadTimeText.text = $"{stat.reloadTime}";

        for (int i = 0; i < optionTexts.Count; i++)
        {
            if (i < weapon.options.Count)
                optionTexts[i].text = $"• {weapon.options[i]}";
            else
                optionTexts[i].text = "";
        }
    }
    public void OnClick_Refresh()
    {
        RefreshUI();
    }

}
