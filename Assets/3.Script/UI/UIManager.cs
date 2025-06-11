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
    [SerializeField] private Image hpBar;

    private void Update()
    {
        if (Player.localPlayer == null) return;

        var playerStat = Player.localPlayer.playerStat;
        var myGun = Player.localPlayer.myGun;

        hpText.text = $"{playerStat.health} / {playerStat.maxHealth}";
        bulletText.text = $"{myGun.CurrentAmmo} / {myGun.gunData.maxAmmo}";

        hpBar.fillAmount = Mathf.Clamp01(playerStat.health / playerStat.maxHealth);
        
        Color setColor = hpBar.color;

        float ratio = (playerStat.maxHealth - playerStat.health) / playerStat.maxHealth;

        setColor.r = ratio;     // 체력 줄수록 빨강 증가
        setColor.g = 1 - ratio; // 체력 줄수록 초록 감소
        setColor.b = 0f;

        hpBar.color = setColor;
    }
}