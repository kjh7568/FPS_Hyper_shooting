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

    [SerializeField] private Image grenadeCoolTimeImage;
    
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

        var pStat = Player.localPlayer.playerStat;
        var eStat = Player.localPlayer.inventory.EquipmentStat;
        var cStat = Player.localPlayer.coreStat;
        var aStat = GameData.Instance.augmentStat;

        var myGun = WeaponManager.instance.currentWeapon;

        var totalHealth = (pStat.maxHealth + eStat.plusHp + cStat.plusHp + aStat.plusHp) * eStat.increaseHealth;

        hpText.text = $"{pStat.health} / {totalHealth}";
        bulletText.text = $"{myGun.weapon.currentAmmo} / {myGun.weapon.currentStat.magazine}";
        coinText.text = $"{Player.localPlayer.coin}";

        hpBar.fillAmount = Mathf.Clamp01(pStat.health / totalHealth);

        Color setColor = hpBar.color;

        float ratio = (totalHealth - pStat.health) / totalHealth;

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

    public IEnumerator DrawGrenadeCoolTime(float cooldownTime)
    {
        float elapsed = 0f;
        grenadeCoolTimeImage.fillAmount = 1f;

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            grenadeCoolTimeImage.fillAmount = Mathf.Clamp01(1f - (elapsed / cooldownTime));
            yield return null;
        }

        grenadeCoolTimeImage.fillAmount = 0f;
    }
}