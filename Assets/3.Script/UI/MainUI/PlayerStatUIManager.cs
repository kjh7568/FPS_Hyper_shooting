using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUIManager : MonoBehaviour
{
    [Header("패널")] [SerializeField] private GameObject playerStatUIPanel;

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

    [Header("이미지 UI")] 
    [SerializeField] private Image primaryWeaponImage;
    [SerializeField] private Image secondaryWeaponImage;
    [SerializeField] private Image knifeWeaponImage;
    [SerializeField] private Image grenadeWeaponImage;
    [SerializeField] private Image helmetImage;
    [SerializeField] private Image bodyArmorImage;
    [SerializeField] private Image glovesImage;
    [SerializeField] private Image bootsImage;
    
    private bool isOpenStatPanel = false;
    private RectTransform panelRectTransform;

    private PlayerController playerController;

    private void Start()
    {
        panelRectTransform = playerStatUIPanel.GetComponent<RectTransform>();
        playerController = Player.localPlayer.GetComponent<PlayerController>();
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
        float totalDefense = eStat.totalDefense * (eStat.increaseDefense + aStat.increaseDefense) * cStat.multiplierDefense;
        float primaryDamage = primaryStat.damage * (eStat.increaseAttack + cStat.increasePrimaryDamage + aStat.increaseAttack) * cStat.multiplierAllDamage;
        float secondaryDamage = secondaryStat.damage * (eStat.increaseAttack + cStat.increaseSecondaryDamage + aStat.increaseAttack) * cStat.multiplierAllDamage;
        float meleeDamage = meleeStat.damage * (eStat.increaseAttack + cStat.increaseMeleeDamage + aStat.increaseAttack) * cStat.multiplierAllDamage;
        float grenadeDamage = grenadeStat.damage * (eStat.increaseAttack + cStat.increaseGrenadeDamage + aStat.increaseAttack) * cStat.multiplierAllDamage;
        float grenadeCooldown = WeaponManager.instance.grenadeCooldown * cStat.increaseCooldown;
        float grenadeRange = grenadeStat.explosionRange * cStat.increaseExplosionRange;
        int criticalChance = eStat.criticalChance;
        float criticalDamage = eStat.criticalDamage;
        float reloadReduction = eStat.increaseReloadSpeed + aStat.increaseReloadSpeed;
        float coinRange = pStat.pickupRadius * cStat.increaseCoinDropRange;
        float coinIncrease = cStat.increaseCoinGain;
        float itemDropChance = eStat.increaseItemDropChance * cStat.increaseItemDropChance;
        float movementSpeed = pStat.moveSpeed * (eStat.increaseMovementSpeed + aStat.increaseMoveSpeed) * cStat.multiplierMovementSpeed;
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
        coinGainMultiplierText.text = $"{(1 - coinIncrease) * 100f :F1}%";
        itemDropChanceText.text = $"{itemDropChance * 100f:F1}%";
        movementSpeedText.text = $"{movementSpeed:F1}";
        dashCoolTimeText.text = $"{dashCooldown:F1}초";
    }

    private void SetImage()
    {
        var armorSet = Player.localPlayer.inventory.equippedArmors;
        
        // primaryWeaponImage.sprite = 
        // secondaryWeaponImage.sprite = 
        // knifeWeaponImage.sprite = 
        // grenadeWeaponImage.sprite = 
        helmetImage.sprite = armorSet[ArmorType.Helmet].data.armorImage;
        bodyArmorImage.sprite =  armorSet[ArmorType.BodyArmor].data.armorImage;
        glovesImage.sprite =  armorSet[ArmorType.Gloves].data.armorImage;
        bootsImage.sprite =  armorSet[ArmorType.Boots].data.armorImage;
    }
}