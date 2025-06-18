using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text bulletText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Image hpBar;

    // public bool isOpenPanel = false;
    // 상점열었을때 PlayerUI 끄다 실패
    
    private void Update()
    {
       //  if (isOpenPanel) return;
       // 상점열었을때 PlayerUI 끄다 실패
        
        if (Player.localPlayer == null) return;

        var playerStat = Player.localPlayer.playerStat;
        var armorStat = Player.localPlayer.inventory.EquipmentStat;
        var myGun = WeaponManager.instance.currentWeapon;

        var totalHealth = (playerStat.maxHealth + armorStat.increaseHealth) * armorStat.multiplierHealth;
        
        hpText.text = $"{playerStat.health} / {totalHealth}";
        bulletText.text = $"{myGun.weapon.currentAmmo} / {myGun.weapon.currentStat.magazine}";
        coinText.text = $"{Player.localPlayer.coin}";
        
        hpBar.fillAmount = Mathf.Clamp01(playerStat.health / totalHealth);
        
        Color setColor = hpBar.color;

        float ratio = (playerStat.maxHealth - playerStat.health) / (playerStat.maxHealth + armorStat.increaseHealth);

        setColor.r = ratio;     // 체력 줄수록 빨강 증가
        setColor.g = 1 - ratio; // 체력 줄수록 초록 감소
        setColor.b = 0f;

        hpBar.color = setColor;
    }
}