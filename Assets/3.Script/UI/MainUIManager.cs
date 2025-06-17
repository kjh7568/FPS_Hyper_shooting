using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    [SerializeField]
    private 
    
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
        
    }

    private void OpenCurrentEquipmentPanel()
    {
        
    }
}
