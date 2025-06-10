using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text bulletText;

    private void Update()
    {
        if (Player.localPlayer == null) return;
        
        hpText.text = $"{Player.localPlayer.playerStat.health} / {Player.localPlayer.playerStat.maxHealth}";
        bulletText.text = $"{Player.localPlayer.myGun.CurrentAmmo} / {Player.localPlayer.myGun.gunData.maxAmmo}";
    }
}
