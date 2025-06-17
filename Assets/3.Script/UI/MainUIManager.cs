using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
        }
    }
    
    private void OpenTargetEquipmentPanel(Armor target)
    {
        if (isOpen) return;
        
        targetPanel.SetActive(true);
        StartCoroutine(OpenTargetPanelByAnimation_coroutine());
    }

    private IEnumerator OpenTargetPanelByAnimation_coroutine()
    {
        
    }

    private void OpenCurrentEquipmentPanel()
    {
        
    }
}
