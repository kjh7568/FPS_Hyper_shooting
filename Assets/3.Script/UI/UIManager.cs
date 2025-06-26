using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text bulletText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Image hpBar;

    [SerializeField] private GameObject damageUI;
    [SerializeField] private RectTransform damageUIParent;

    private WaitForSeconds delay = new WaitForSeconds(1.5f);

    // public bool isOpenPanel = false;
    // 상점열었을때 PlayerUI 끄다 실패

    private void Update()
    {
        //  if (isOpenPanel) return;
        // 상점열었을때 PlayerUI 끄다 실패

        if (Player.localPlayer == null) return;

        var playerStat = Player.localPlayer.playerStat;
        var armorStat = Player.localPlayer.inventory.EquipmentStat;
        var coreStat = Player.localPlayer.coreStat;

        var myGun = WeaponManager.instance.currentWeapon;

        var totalHealth = (playerStat.maxHealth + armorStat.increaseHealth + coreStat.coreHp) *
                          armorStat.multiplierHealth;
        // var totalHealth = (playerStat.maxHealth + armorStat.increaseHealth) * armorStat.multiplierHealth;

        hpText.text = $"{playerStat.health} / {totalHealth}";
        bulletText.text = $"{myGun.weapon.currentAmmo} / {myGun.weapon.currentStat.magazine}";
        coinText.text = $"{Player.localPlayer.coin}";

        hpBar.fillAmount = Mathf.Clamp01(playerStat.health / totalHealth);

        Color setColor = hpBar.color;

        //  float ratio = (playerStat.maxHealth - playerStat.health) / (playerStat.maxHealth + armorStat.increaseHealth);
        float ratio = (totalHealth - playerStat.health) / totalHealth;

        setColor.r = ratio; // 체력 줄수록 빨강 증가
        setColor.g = 1 - ratio; // 체력 줄수록 초록 감소
        setColor.b = 0f;

        hpBar.color = setColor;
    }

    public IEnumerator PrintDamage_Coroutine(CombatEvent combatEvent, float damage, bool isCritical)
    {
        // UI 생성
        GameObject uiObj = Instantiate(damageUI, damageUIParent);

        Transform point = new GameObject("HitPoint").transform;
        
        float randomDistance = Random.Range(0.5f, 1f);

        Vector3 randomDir = Random.insideUnitSphere.normalized;

        point.position = combatEvent.HitPosition + randomDir * randomDistance;
        point.SetParent(combatEvent.Collider.gameObject.transform);
        
        uiObj.GetComponent<DamageUI>().Set(point ,combatEvent.HitPosition, damage, isCritical);

        // 일정 시간 대기 후 삭제    
        yield return delay;

        Destroy(uiObj);
    }
}