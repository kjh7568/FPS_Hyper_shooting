using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemShopLoader : MonoBehaviour
{
    [SerializeField] private ItemShopPanelUIComponents[] panelSlots; // 이미 있는 3개의 슬롯 할당

    [SerializeField] private WeaponDataSO[] weapons;

    public Weapon[] storeWeapons = new Weapon[3];
    public int[] prices = new int[3];
    
    private void OnEnable()
    {
        SetStoreItem();
        SetStoreUI();
    }

    #region 아이템 데이터 생성 로직

    private void SetStoreItem()
    {
        for (int i = 0; i < 3; i++)
        {
            storeWeapons[i] = GetRandomWeapon();
        }
    }

    private Weapon GetRandomWeapon()
    {
        int roll = Random.Range(0, weapons.Length);

        return new Weapon(weapons[roll], GetRandomGrade());
    }

    private WeaponGrade GetRandomGrade()
    {
        int roll = Random.Range(0, 100);

        if (roll < 50) return WeaponGrade.Common;
        else if (roll < 80) return WeaponGrade.Rare;
        else if (roll < 97) return WeaponGrade.Epic;
        else return WeaponGrade.Legendary;
    }

    #endregion

    #region 상점 UI와 연동 로직

    private void SetStoreUI()
    {
        for (int i = 0; i < 3; i++)
        {
            panelSlots[i].nameText.text = storeWeapons[i].data.weaponName;
            panelSlots[i].levelText.text = storeWeapons[i].currentStat.level.ToString();
            panelSlots[i].gradeText.text = storeWeapons[i].data.grade.ToString();
            panelSlots[i].damageText.text = storeWeapons[i].currentStat.damage.ToString(CultureInfo.CurrentCulture);
            panelSlots[i].fireRateText.text = storeWeapons[i].currentStat.fireRate.ToString(CultureInfo.CurrentCulture);
            panelSlots[i].magazineText.text = storeWeapons[i].currentStat.magazine.ToString();
            panelSlots[i].reloadTimeText.text = storeWeapons[i].currentStat.reloadTime.ToString(CultureInfo.CurrentCulture);
            SetDescription(storeWeapons[i], panelSlots[i].descriptions);
            prices[i] = SetPrice(storeWeapons[i], panelSlots[i].priceText);
        }
    }

    private void SetDescription(Weapon weapon, TMP_Text[] descriptions)
    {
        int idx = 0;

        for (int i = 0; i < 3; i++)
        {
            descriptions[i].gameObject.SetActive(false);
        }
        
        foreach (var option in weapon.options)
        {
            descriptions[idx].gameObject.SetActive(true);

            switch (option)
            {
                case WeaponSpecialEffect.DashCooldownReduction:
                    descriptions[idx].text = "• 대시 쿨타임이 10% 감소합니다";
                    break;
                case WeaponSpecialEffect.ReloadSpeedReduction:
                    descriptions[idx].text = "• 재장전 속도가 10% 빨라집니다";
                    break;
                case WeaponSpecialEffect.MultiplierAttackDamage:
                    descriptions[idx].text = "• 공격력이 5% 증가합니다";
                    break;
                case WeaponSpecialEffect.MultiplierMovementSpeed:
                    descriptions[idx].text = "• 이동 속도가 10% 증가합니다";
                    break;
                case WeaponSpecialEffect.IncreaseCriticalChance:
                    descriptions[idx].text = "• 치명타 확률이 10% 증가합니다";
                    break;
                case WeaponSpecialEffect.IncreaseCriticalDamage:
                    descriptions[idx].text = "• 치명타 데미지가 10% 증가합니다";
                    break;
                case WeaponSpecialEffect.IncreaseItemDropRate:
                    descriptions[idx].text = "• 아이템 드롭 확률이 10% 증가합니다.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            idx++;
        }
    }

    private int SetPrice(Weapon weapon, TMP_Text priceText)
    {
        int price = 0;

        switch (weapon.grade)
        {
            case WeaponGrade.Common:
                price = 100;
                break;
            case WeaponGrade.Rare:
                price = 300;
                break;
            case WeaponGrade.Epic:
                price = 500;
                break;
            case WeaponGrade.Legendary:
                price = 1000;
                break;
        }
        
        priceText.text = price.ToString();
        
        return price;
    }
    
    #endregion
}