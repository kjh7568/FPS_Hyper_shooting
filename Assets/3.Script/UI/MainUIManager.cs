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

    private GameObject currentTarget;

    private bool isOpen = false;
    
    void Update()
    {
        IsItemHovered();

        if (isOpen && Input.GetKeyDown(KeyCode.E))
        {
            var targetItem = currentTarget.GetComponent<DroppedItem>().dropedItem;
            var temp = Player.localPlayer.inventory.equippedArmors[targetItem.Type];
            
            Player.localPlayer.inventory.EquipArmor(targetItem);
            currentTarget.GetComponent<DroppedItem>().dropedItem = temp;

            OpenTargetEquipmentPanel(currentTarget.GetComponent<DroppedItem>().dropedItem);
            OpenCurrentEquipmentPanel(targetItem);
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
        OpenTargetPanelByAnimation(targetPanel.GetComponent<RectTransform>());
        OpenTargetEquipmentPanel(droppedItem.dropedItem);
        
        currentPanel.SetActive(true);
        OpenCurrentPanelByAnimation(currentPanel.GetComponent<RectTransform>());
        OpenCurrentEquipmentPanel(Player.localPlayer.inventory.equippedArmors[droppedItem.dropedItem.Type]);

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
    
    private void OpenTargetEquipmentPanel(Armor target)
    {
        targetLevelText.text = $"LV. {target.currentStat.level}";
        targetNameText.text = $"{target.data.armorName}";
        targetTierText.text = $"{target.grade.ToString()}";
        targetValueText.text = $"{target.currentStat.defense}";
        SetDescription(target, targetDescriptionTexts);
         
        targetItemImage.sprite = target.data.icon;
    }

    private void OpenCurrentEquipmentPanel(Armor target)
    {
        currentLevelText.text = $"LV. {target.currentStat.level}";
        currentNameText.text = $"{target.data.armorName}";
        currentTierText.text = $"{target.grade.ToString()}";
        currentValueText.text = $"{target.currentStat.defense}";
        SetDescription(target, currentDescriptionTexts);
         
        currentItemImage.sprite = target.data.icon;
    }

    private void OpenTargetPanelByAnimation(RectTransform targetRect)
    {
        targetRect.localScale = Vector3.one * 0.01f;

        // 1초 동안 (EaseOut으로) 스케일 1까지 확대
        targetRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    private void OpenCurrentPanelByAnimation(RectTransform targetRect)
    {
        // 초기 위치: 오른쪽 바깥으로 설정 (예: x = 500)
        targetRect.anchoredPosition = new Vector2(1200f, targetRect.anchoredPosition.y);

        // 등장: 왼쪽으로 슬라이드 이동 (x = 0 위치로)
        targetRect.DOAnchorPosX(750f, 0.5f).SetEase(Ease.OutCubic);
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
