using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerStatUIManager : MonoBehaviour
{
    [Header("패널")] [SerializeField] private GameObject playerStatUIPanel;
    [SerializeField] private GameObject hoveringPanel;

    [Header("텍스트 UI")] [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text defenseText;
    [SerializeField] private TMP_Text primaryText;
    [SerializeField] private TMP_Text secondaryText;
    [SerializeField] private TMP_Text knifeText;
    [SerializeField] private TMP_Text grenadeText;
    [SerializeField] private TMP_Text grenadeCoolTimeText;
    [SerializeField] private TMP_Text grenadeExplosionRangeText;
    [SerializeField] private TMP_Text criticalChanceText;
    [SerializeField] private TMP_Text criticalDamageText;
    [SerializeField] private TMP_Text reloadReductionText;
    [SerializeField] private TMP_Text coinGainRangeText;
    [SerializeField] private TMP_Text coinGainMultiplierText;
    [SerializeField] private TMP_Text itemDropChanceText;
    [SerializeField] private TMP_Text movementSpeedText;
    [SerializeField] private TMP_Text dashCoolTimeText;

    [Header("이미지 UI")] [SerializeField] private Image primaryWeaponImage;
    [SerializeField] private Image secondaryWeaponImage;
    [SerializeField] private Image knifeWeaponImage;
    [SerializeField] private Image grenadeWeaponImage;
    [SerializeField] private Image helmetImage;
    [SerializeField] private Image bodyArmorImage;
    [SerializeField] private Image glovesImage;
    [SerializeField] private Image bootsImage;

    [Header("UI 레이케스터")]
    public GraphicRaycaster raycaster;
    
    [Header("호버링 패널 컴포넌트")]
    [SerializeField] private DropItemUIFiled hoverPanelFiled;

    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    private bool isOpenStatPanel = false;
    private RectTransform panelRectTransform;
    private RectTransform hoverRectTransform;

    private PlayerController playerController;

    private void Start()
    {
        panelRectTransform = playerStatUIPanel.GetComponent<RectTransform>();
        hoverRectTransform = hoveringPanel.GetComponent<RectTransform>();
        
        playerController = Player.localPlayer.GetComponent<PlayerController>();

        pointerEventData = new PointerEventData(EventSystem.current);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isOpenStatPanel)
        {
            isOpenStatPanel = !isOpenStatPanel;
            panelRectTransform.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutCubic);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerController.isOpenPanel = true;

            SetText();
            SetImage();
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isOpenStatPanel)
        {
            isOpenStatPanel = !isOpenStatPanel;
            panelRectTransform.DOAnchorPosY(1080f, 0.5f).SetEase(Ease.OutCubic);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerController.isOpenPanel = false;
        }

        //호버링 로직
        HoveringPanel();
    }

    private void SetText()
    {
        var pStat = Player.localPlayer.playerStat;
        var eStat = Player.localPlayer.inventory.EquipmentStat;
        var cStat = Player.localPlayer.coreStat;
        var aStat = GameData.Instance.augmentStat;

        var primaryStat = WeaponManager.instance.primaryWeapon.weapon.currentStat;
        var secondaryStat = WeaponManager.instance.secondaryWeapon.weapon.currentStat;
        var meleeStat = WeaponManager.instance.knifeWeapon.weapon.currentStat;
        var grenadeStat = WeaponManager.instance.grenadeWeapon.weapon.currentStat;

        float totalHp = (pStat.maxHealth + eStat.plusHp + cStat.plusHp + aStat.plusHp) * eStat.increaseHealth;
        float totalDefense = eStat.totalDefense * (eStat.increaseDefense + aStat.increaseDefense) *
                             cStat.multiplierDefense;
        float primaryDamage = primaryStat.damage *
                              (eStat.increaseAttack + cStat.increasePrimaryDamage + aStat.increaseAttack) *
                              cStat.multiplierAllDamage;
        float secondaryDamage = secondaryStat.damage *
                                (eStat.increaseAttack + cStat.increaseSecondaryDamage + aStat.increaseAttack) *
                                cStat.multiplierAllDamage;
        float meleeDamage = meleeStat.damage *
                            (eStat.increaseAttack + cStat.increaseMeleeDamage + aStat.increaseAttack) *
                            cStat.multiplierAllDamage;
        float grenadeDamage = grenadeStat.damage *
                              (eStat.increaseAttack + cStat.increaseGrenadeDamage + aStat.increaseAttack) *
                              cStat.multiplierAllDamage;
        float grenadeCooldown = WeaponManager.instance.grenadeCooldown * cStat.increaseCooldown;
        float grenadeRange = grenadeStat.explosionRange * cStat.increaseExplosionRange;
        int criticalChance = eStat.criticalChance;
        float criticalDamage = eStat.criticalDamage;
        float reloadReduction = eStat.increaseReloadSpeed + aStat.increaseReloadSpeed;
        float coinRange = pStat.pickupRadius * cStat.increaseCoinDropRange;
        float coinIncrease = cStat.increaseCoinGain;
        float itemDropChance = eStat.increaseItemDropChance * cStat.increaseItemDropChance;
        float movementSpeed = pStat.moveSpeed * (eStat.increaseMovementSpeed + aStat.increaseMoveSpeed) *
                              cStat.multiplierMovementSpeed;
        float dashCooldown = pStat.dashCoolTime * (1 - eStat.increaseDashCooldown + aStat.increaseDashCooldown);

        hpText.text = $"{totalHp} ({cStat.hpRegion:F1}/s)";
        defenseText.text = $"{totalDefense} ({totalDefense * 0.01f}%)";

        primaryText.text = $"{primaryDamage:F1}";
        secondaryText.text = $"{secondaryDamage:F1}";
        knifeText.text = $"{meleeDamage:F1}";
        grenadeText.text = $"{grenadeDamage:F1}";

        grenadeCoolTimeText.text = $"{grenadeCooldown:F1}초";
        grenadeExplosionRangeText.text = $"{grenadeRange:F1}m";

        criticalChanceText.text = $"{criticalChance}%";
        criticalDamageText.text = $"{criticalDamage * 100f:F1}%";

        reloadReductionText.text = $"{reloadReduction * 100f:F1}%";
        coinGainRangeText.text = $"{coinRange:F1}m";
        coinGainMultiplierText.text = $"{(1 - coinIncrease) * 100f:F1}%";
        itemDropChanceText.text = $"{itemDropChance * 100f:F1}%";
        movementSpeedText.text = $"{movementSpeed:F1}";
        dashCoolTimeText.text = $"{dashCooldown:F1}초";
    }

    private void SetImage()
    {
        var armorSet = Player.localPlayer.inventory.equippedArmors;
        var weaponSet = WeaponManager.instance;

        primaryWeaponImage.sprite = weaponSet.primaryWeapon.weapon.data.weaponImage;
        secondaryWeaponImage.sprite = weaponSet.secondaryWeapon.weapon.data.weaponImage;
        knifeWeaponImage.sprite = weaponSet.knifeWeapon.weapon.data.weaponImage;
        grenadeWeaponImage.sprite = weaponSet.grenadeWeapon.weapon.data.weaponImage;
        helmetImage.sprite = armorSet[ArmorType.Helmet].data.armorImage;
        bodyArmorImage.sprite = armorSet[ArmorType.BodyArmor].data.armorImage;
        glovesImage.sprite = armorSet[ArmorType.Gloves].data.armorImage;
        bootsImage.sprite = armorSet[ArmorType.Boots].data.armorImage;
    }

    private void HoveringPanel()
    {
        pointerEventData.position = Input.mousePosition;

        raycastResults.Clear();
        raycaster.Raycast(pointerEventData, raycastResults);

        bool found = false;

        foreach (RaycastResult result in raycastResults)
        {
            string name = result.gameObject.name;
            Debug.Log($"Hit: {name}");

            switch (name)
            {
                case "PrimaryImageOutline":
                    Debug.Log("주무기 위에 마우스 있음");
                    // TODO: primary 관련 처리
                    SetHoverPanel(WeaponManager.instance.primaryWeapon.weapon);
                    found = true;
                    break;

                case "SecondaryImageOutline":
                    Debug.Log("보조무기 위에 마우스 있음");
                    // TODO: secondary 관련 처리
                    SetHoverPanel(WeaponManager.instance.secondaryWeapon.weapon);
                    found = true;
                    break;

                case "KnifeImageOutline":
                    Debug.Log("근접무기 위에 마우스 있음");
                    // TODO: knife 관련 처리
                    SetHoverPanel(WeaponManager.instance.knifeWeapon.weapon);
                    found = true;
                    break;

                case "GrenadeImageOutline":
                    Debug.Log("수류탄 위에 마우스 있음");
                    // TODO: grenade 관련 처리
                    SetHoverPanel(WeaponManager.instance.grenadeWeapon.weapon);
                    found = true;
                    break;
            }
        }

        hoveringPanel.SetActive(found);
        
        if (found)
        {
            // 마우스 오른쪽으로 offsetX만큼 떨어진 위치로 패널 이동
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                hoverRectTransform.parent as RectTransform,
                Input.mousePosition + new Vector3(230f, -300f, 0),
                null,
                out mousePos
            );
            hoverRectTransform.anchoredPosition = mousePos;
        }
    }
    
    private void SetHoverPanel(Weapon target)
    {
        hoverPanelFiled.targetLevelText.text = $"LV. {target.currentStat.level}";
        hoverPanelFiled.targetNameText.text = $"{target.data.weaponName}";
        hoverPanelFiled.targetTierText.text = $"{target.grade.ToString()}";

        hoverPanelFiled.targetItemImage.sprite = target.data.weaponImage;

        hoverPanelFiled.targetKnifeInfoPanel.SetActive(false);
        hoverPanelFiled.targetGrenadeInfoPanel.SetActive(false);
        hoverPanelFiled.targetArmorInfoPanel.SetActive(false);
        hoverPanelFiled.targetGunInfoPanel.SetActive(false);

        switch (target.Type)
        {
            case WeaponType.Akm:
            case WeaponType.M4:
            case WeaponType.Sniper:
            case WeaponType.Shotgun:
            case WeaponType.Ump:
            case WeaponType.Pistol:
                hoverPanelFiled.targetGunInfoPanel.SetActive(true);

                hoverPanelFiled.targetGunDamageValueText.text = target.currentStat.damage.ToString();
                hoverPanelFiled.targetGunFireRateValueText.text = target.currentStat.fireRate.ToString();
                hoverPanelFiled.targetGunMagazineSizeValueText.text = target.currentStat.magazine.ToString();
                hoverPanelFiled.targetGunReloadTimeValueText.text = target.currentStat.reloadTime.ToString();

                break;
            case WeaponType.Knife:
                hoverPanelFiled.targetKnifeInfoPanel.SetActive(true);

                hoverPanelFiled.targetKnifeDamageValueText.text = target.currentStat.damage.ToString();
                hoverPanelFiled.targetKnifeFireRateValueText.text = target.currentStat.fireRate.ToString();

                break;
            case WeaponType.Grenade:
                hoverPanelFiled.targetGrenadeInfoPanel.SetActive(true);

                hoverPanelFiled.targetGrenadeDamageValueText.text = target.currentStat.damage.ToString();
                hoverPanelFiled.targetGrenadeExplosionRangeValueText.text = $"{target.currentStat.explosionRange}m";

                break;
        }

        SetDescription(target, hoverPanelFiled.targetDescriptionTexts);
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
}