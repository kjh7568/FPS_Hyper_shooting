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
    [Header("타켓 패널 컴포넌트")]
    [SerializeField] private GameObject targetPanel; 
    [SerializeField] private TMP_Text targetLevelText; 
    [SerializeField] private TMP_Text targetNameText; 
    [SerializeField] private TMP_Text targetTierText; 
    [SerializeField] private Image targetItemImage; 
    [SerializeField] private TMP_Text targetValueText; 
    [SerializeField] private TMP_Text[] targetDescriptionTexts;
    
    [Header("현재 장비 패널 컴포넌트")]
    [SerializeField] private GameObject currentPanel; 
    [SerializeField] private TMP_Text currentLevelText; 
    [SerializeField] private TMP_Text currentNameText; 
    [SerializeField] private TMP_Text currentTierText; 
    [SerializeField] private Image currentItemImage; 
    [SerializeField] private TMP_Text currentValueText; 
    [SerializeField] private TMP_Text[] currentDescriptionTexts;


    private bool isOpen = false;
    
    void Update()
    {
        IsItemHovered();
    }

    private void IsItemHovered()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1.5f)) // 1.5f는 감지 거리
        {
            if (hit.collider.gameObject.CompareTag("DropItem"))
            {
                GameObject target = hit.collider.gameObject;
                
                //todo 드롭 장비 종류에 따라 오버로딩 할 것
                OpenTargetEquipmentPanel(target.GetComponent<DroppedItem>().dropedItem);
            }
            else
            {
                isOpen = false;
                targetPanel.SetActive(false);
            }
        }
    }
    
    private void OpenTargetEquipmentPanel(Armor target)
    {
        if (isOpen) return;

        isOpen = true;
        targetPanel.SetActive(true);
        OpenTargetPanelByAnimation_coroutine(targetPanel.GetComponent<RectTransform>());

        targetLevelText.text = $"LV. {target.currentStat.level}";
        targetNameText.text = $"{target.data.armorName}";
        targetTierText.text = $"{target.grade.ToString()}";
        targetValueText.text = $"{target.currentStat.defense}";
        SetDescription(target, targetDescriptionTexts);
         
        targetItemImage.sprite = target.data.icon;
    }

    private void OpenTargetPanelByAnimation_coroutine(RectTransform targetRect)
    {
        targetRect.localScale = Vector3.one * 0.01f;

        // 1초 동안 (EaseOut으로) 스케일 1까지 확대
        targetRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    private void OpenCurrentEquipmentPanel()
    {
        
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
                    descriptions[idx].text = "대시 쿨타임이 10% 감소합니다";
                    break;
                case SpecialEffect.MultiplierDefense:
                    descriptions[idx].text = "방어력이 10% 증가합니다";
                    break;
                case SpecialEffect.IncreaseHealth:
                    descriptions[idx].text = "최대 체력이 20 증가합니다";
                    break;
                case SpecialEffect.MultiplierHealth:
                    descriptions[idx].text = "최대 체력이 10% 증가합니다";
                    break;
                case SpecialEffect.ReloadSpeedReduction:
                    descriptions[idx].text = "재장전 속도가 10% 빨라집니다";
                    break;
                case SpecialEffect.MultiplierAttackDamage:
                    descriptions[idx].text = "공격력이 5% 증가합니다";
                    break;
                case SpecialEffect.MultiplierMovementSpeed:
                    descriptions[idx].text = "이동 속도가 10% 증가합니다";
                    break;
            }

            idx++;
        }
    }
}
