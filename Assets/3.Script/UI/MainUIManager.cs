using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainUIManager : MonoBehaviour
{
    [Header("타켓 패널 컴포넌트")] [SerializeField]
    private GameObject targetPanel;

    [SerializeField] private DropItemUIFiled targetFiled;

    [Header("현재 장비 패널 컴포넌트")] [SerializeField]
    private GameObject currentPanel;

    [SerializeField] private DropItemUIFiled currentFiled;

    private GameObject currentTarget;

    private bool isOpen = false;

    private void Start()
    {
        targetFiled = targetPanel.GetComponent<DropItemUIFiled>();
        currentFiled = currentPanel.GetComponent<DropItemUIFiled>();
    }

    void Update()
    {
        IsItemHovered();

        if (isOpen && Input.GetKeyDown(KeyCode.E))
        {
            if (currentTarget.GetComponent<DroppedItem>().isWeapon)
            {
                var targetItem = currentTarget.GetComponent<DroppedItem>().dropedWeapon;
                Weapon temp = null;
                
                switch (targetItem.Type)
                {
                    case WeaponType.Akm:
                    case WeaponType.M4:
                    case WeaponType.Sniper:
                    case WeaponType.Shotgun:
                    case WeaponType.Ump:
                        temp = WeaponManager.instance.primaryWeapon.weapon;
                        break;
                    case WeaponType.Pistol:
                        temp = WeaponManager.instance.secondaryWeapon.weapon;
                        break;
                    case WeaponType.Knife:
                        temp = WeaponManager.instance.knifeWeapon.weapon;
                        break;
                    case WeaponType.Grenade:
                        temp = WeaponManager.instance.grenadeWeapon.weapon;
                        break;
                }

                ChangeWeapon(targetItem);
                currentTarget.GetComponent<DroppedItem>().dropedWeapon = temp;

                OpenTargetEquipmentPanel(currentTarget.GetComponent<DroppedItem>().dropedWeapon);
                OpenCurrentEquipmentPanel(targetItem);
            }
            else
            {
                var targetItem = currentTarget.GetComponent<DroppedItem>().dropedArmor;
                var temp = Player.localPlayer.inventory.equippedArmors[targetItem.Type];

                Player.localPlayer.inventory.EquipArmor(targetItem);
                currentTarget.GetComponent<DroppedItem>().dropedArmor = temp;

                OpenTargetEquipmentPanel(currentTarget.GetComponent<DroppedItem>().dropedArmor);
                OpenCurrentEquipmentPanel(targetItem);
            }
        }
    }

    private void IsItemHovered()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            ClosePanelsIfOpen();
            return;
        }

        if (!hit.collider.CompareTag("DropItem") || hit.distance > 1.5f)
        {
            ClosePanelsIfOpen();
            return;
        }

        GameObject target = hit.collider.gameObject;
        if (target == currentTarget) return;

        currentTarget = target;

        var droppedItem = target.GetComponent<DroppedItem>();
        if (droppedItem == null) return;

        targetPanel.SetActive(true);
        currentPanel.SetActive(true);

        OpenTargetPanelByAnimation(targetPanel.GetComponent<RectTransform>());
        OpenCurrentPanelByAnimation(currentPanel.GetComponent<RectTransform>());

        if (droppedItem.isWeapon)
        {
            OpenTargetEquipmentPanel(droppedItem.dropedWeapon);

            switch (droppedItem.dropedWeapon.Type)
            {
                case WeaponType.Akm:
                case WeaponType.M4:
                case WeaponType.Sniper:
                case WeaponType.Shotgun:
                case WeaponType.Ump:
                    OpenCurrentEquipmentPanel(WeaponManager.instance.primaryWeapon.weapon);
                    break;
                case WeaponType.Pistol:
                    OpenCurrentEquipmentPanel(WeaponManager.instance.secondaryWeapon.weapon);
                    break;
                case WeaponType.Knife:
                    OpenCurrentEquipmentPanel(WeaponManager.instance.knifeWeapon.weapon);
                    break;
                case WeaponType.Grenade:
                    OpenCurrentEquipmentPanel(WeaponManager.instance.grenadeWeapon.weapon);
                    break;
            }
        }
        else
        {
            OpenTargetEquipmentPanel(droppedItem.dropedArmor);
            OpenCurrentEquipmentPanel(Player.localPlayer.inventory.equippedArmors[droppedItem.dropedArmor.Type]);
        }

        isOpen = true;
        targetPanel.SetActive(true);
        currentPanel.SetActive(true);
    }

    private void ClosePanelsIfOpen()
    {
        if (!isOpen) return;

        isOpen = false;
        currentTarget = null;

        targetPanel.SetActive(false);
        currentPanel.SetActive(false);
    }


    #region 무기 관련 로직

    private void OpenTargetEquipmentPanel(Weapon target)
    {
        targetFiled.targetLevelText.text = $"LV. {target.currentStat.level}";
        targetFiled.targetNameText.text = $"{target.data.weaponName}";
        targetFiled.targetTierText.text = $"{target.grade.ToString()}";

        targetFiled.targetItemImage.sprite = target.data.weaponImage;

        targetFiled.targetKnifeInfoPanel.SetActive(false);
        targetFiled.targetGrenadeInfoPanel.SetActive(false);
        targetFiled.targetArmorInfoPanel.SetActive(false);
        targetFiled.targetGunInfoPanel.SetActive(false);

        switch (target.Type)
        {
            case WeaponType.Akm:
            case WeaponType.M4:
            case WeaponType.Sniper:
            case WeaponType.Shotgun:
            case WeaponType.Ump:
            case WeaponType.Pistol:
                targetFiled.targetGunInfoPanel.SetActive(true);

                targetFiled.targetGunDamageValueText.text = target.currentStat.damage.ToString();
                targetFiled.targetGunFireRateValueText.text = target.currentStat.fireRate.ToString();
                targetFiled.targetGunMagazineSizeValueText.text = target.currentStat.magazine.ToString();
                targetFiled.targetGunReloadTimeValueText.text = target.currentStat.reloadTime.ToString();

                break;
            case WeaponType.Knife:
                targetFiled.targetKnifeInfoPanel.SetActive(true);

                targetFiled.targetKnifeDamageValueText.text = target.currentStat.damage.ToString();
                targetFiled.targetKnifeFireRateValueText.text = target.currentStat.fireRate.ToString();

                break;
            case WeaponType.Grenade:
                targetFiled.targetGrenadeInfoPanel.SetActive(true);

                targetFiled.targetGrenadeDamageValueText.text = target.currentStat.damage.ToString();
                targetFiled.targetGrenadeExplosionRangeValueText.text = $"{target.currentStat.explosionRange}m";

                break;
        }

        SetDescription(target, targetFiled.targetDescriptionTexts);
    }

    private void OpenCurrentEquipmentPanel(Weapon target)
    {
        currentFiled.targetLevelText.text = $"LV. {target.currentStat.level}";
        currentFiled.targetNameText.text = $"{target.data.weaponName}";
        currentFiled.targetTierText.text = $"{target.grade.ToString()}";

        currentFiled.targetItemImage.sprite = target.data.weaponImage;

        currentFiled.targetKnifeInfoPanel.SetActive(false);
        currentFiled.targetGrenadeInfoPanel.SetActive(false);
        currentFiled.targetArmorInfoPanel.SetActive(false);
        currentFiled.targetGunInfoPanel.SetActive(false);

        switch (target.Type)
        {
            case WeaponType.Akm:
            case WeaponType.M4:
            case WeaponType.Sniper:
            case WeaponType.Shotgun:
            case WeaponType.Ump:
            case WeaponType.Pistol:
                currentFiled.targetGunInfoPanel.SetActive(true);

                currentFiled.targetGunDamageValueText.text = target.currentStat.damage.ToString();
                currentFiled.targetGunFireRateValueText.text = target.currentStat.fireRate.ToString();
                currentFiled.targetGunMagazineSizeValueText.text = target.currentStat.magazine.ToString();
                currentFiled.targetGunReloadTimeValueText.text = target.currentStat.reloadTime.ToString();

                break;
            case WeaponType.Knife:
                currentFiled.targetKnifeInfoPanel.SetActive(true);

                currentFiled.targetKnifeDamageValueText.text = target.currentStat.damage.ToString();
                currentFiled.targetKnifeFireRateValueText.text = target.currentStat.fireRate.ToString();

                break;
            case WeaponType.Grenade:
                currentFiled.targetGrenadeInfoPanel.SetActive(true);

                currentFiled.targetGrenadeDamageValueText.text = target.currentStat.damage.ToString();
                currentFiled.targetGrenadeExplosionRangeValueText.text = $"{target.currentStat.explosionRange}m";

                break;
        }

        SetDescription(target, currentFiled.targetDescriptionTexts);
    }

    private void OpenCurrentPanelByAnimation(RectTransform targetRect)
    {
        // 초기 위치: 오른쪽 바깥으로 설정 (예: x = 500)
        targetRect.anchoredPosition = new Vector2(1200f, targetRect.anchoredPosition.y);

        // 등장: 왼쪽으로 슬라이드 이동 (x = 0 위치로)
        targetRect.DOAnchorPosX(750f, 0.5f).SetEase(Ease.OutCubic);
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

    private void ChangeWeapon(Weapon target)
    {
        //옵션 수치 변경
        switch (target.Type)
        {
            //주무기
            case WeaponType.Akm:
            case WeaponType.M4:
            case WeaponType.Sniper:
            case WeaponType.Shotgun:
            case WeaponType.Ump:
                WeaponManager.instance.RemoveWeaponOption(WeaponManager.instance.primaryWeapon.weapon);
                break;
            //보조무기
            case WeaponType.Pistol:
                WeaponManager.instance.RemoveWeaponOption(WeaponManager.instance.secondaryWeapon.weapon);
                break;
            //칼
            case WeaponType.Knife:
                WeaponManager.instance.RemoveWeaponOption(WeaponManager.instance.knifeWeapon.weapon);
                break;
            //수류탄
            case WeaponType.Grenade:
                WeaponManager.instance.RemoveWeaponOption(WeaponManager.instance.grenadeWeapon.weapon);
                break;
        }
        
        WeaponManager.instance.ApplyWeaponOption(target);
        
        //아이템 유형에 따라 무기 모델 변경 & 웨폰 컨트롤러의 웨폰 값을 변경
        WeaponManager.instance.ChangeWeapon(target);
    }

    #endregion

    #region 방어구 관련 로직

    private void OpenTargetEquipmentPanel(Armor target)
    {
        targetFiled.targetLevelText.text = $"LV. {target.currentStat.level}";
        targetFiled.targetNameText.text = $"{target.data.armorName}";
        targetFiled.targetTierText.text = $"{target.grade.ToString()}";

        targetFiled.targetGunInfoPanel.SetActive(false);
        targetFiled.targetKnifeInfoPanel.SetActive(false);
        targetFiled.targetGrenadeInfoPanel.SetActive(false);

        targetFiled.targetArmorInfoPanel.SetActive(true);
        targetFiled.targetArmorValueText.text = $"{target.currentStat.defense}";

        SetDescription(target, targetFiled.targetDescriptionTexts);

        targetFiled.targetItemImage.sprite = target.data.armorImage;
    }

    private void OpenCurrentEquipmentPanel(Armor target)
    {
        currentFiled.targetLevelText.text = $"LV. {target.currentStat.level}";
        currentFiled.targetNameText.text = $"{target.data.armorName}";
        currentFiled.targetTierText.text = $"{target.grade.ToString()}";

        currentFiled.targetGunInfoPanel.SetActive(false);
        currentFiled.targetKnifeInfoPanel.SetActive(false);
        currentFiled.targetGrenadeInfoPanel.SetActive(false);

        currentFiled.targetArmorInfoPanel.SetActive(true);
        currentFiled.targetArmorValueText.text = $"{target.currentStat.defense}";

        SetDescription(target, currentFiled.targetDescriptionTexts);

        currentFiled.targetItemImage.sprite = target.data.armorImage;
    }

    private void OpenTargetPanelByAnimation(RectTransform targetRect)
    {
        targetRect.localScale = Vector3.one * 0.01f;

        // 1초 동안 (EaseOut으로) 스케일 1까지 확대
        targetRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    private void SetDescription(Armor parts, TMP_Text[] descriptions)
    {
        int idx = 0;

        for (int i = 0; i < 3; i++)
        {
            descriptions[i].gameObject.SetActive(false);
        }

        foreach (var option in parts.options)
        {
            descriptions[idx].gameObject.SetActive(true);

            switch (option)
            {
                case SpecialEffect.DashCooldownReduction:
                    descriptions[idx].text = "• 대시 쿨타임이 10% 감소합니다";
                    break;
                case SpecialEffect.MultiplierDefense:
                    descriptions[idx].text = "• 방어력이 10% 증가합니다";
                    break;
                case SpecialEffect.IncreaseHealth:
                    descriptions[idx].text = "• 최대 체력이 20 증가합니다";
                    break;
                case SpecialEffect.MultiplierHealth:
                    descriptions[idx].text = "• 최대 체력이 10% 증가합니다";
                    break;
                case SpecialEffect.ReloadSpeedReduction:
                    descriptions[idx].text = "• 재장전 속도가 10% 빨라집니다";
                    break;
                case SpecialEffect.MultiplierAttackDamage:
                    descriptions[idx].text = "• 공격력이 5% 증가합니다";
                    break;
                case SpecialEffect.MultiplierMovementSpeed:
                    descriptions[idx].text = "• 이동 속도가 10% 증가합니다";
                    break;
            }

            idx++;
        }
    }

    #endregion
}