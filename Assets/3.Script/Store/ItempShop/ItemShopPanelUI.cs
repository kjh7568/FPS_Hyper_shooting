using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopPanelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text gradeText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text fireRateText;
    [SerializeField] private TMP_Text reloadTimeText;
    

    public void SetGunData(GunDataSO gun)
    {
        GunLevelStat stat = gun.GetStatByLevel(1); // 기본 레벨 스탯 사용

        weaponNameText.text = $"Name {gun.name}";
        levelText.text = $"level {stat.level}";
        gradeText.text = $"Grade {gun.grade}";
        damageText.text = $"DMG: {stat.damage}";
        fireRateText.text = $"FR: {stat.fireRate}";
        reloadTimeText.text = $"RL: {stat.reloadTime}";
    }
}