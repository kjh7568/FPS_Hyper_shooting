using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject settingPanel;

    public void OnClickExit()
    {
        buttonPanel.SetActive(true);
        settingPanel.SetActive(false);
    }
    
    //todo 시간 남으면 볼륨 조절, 감도 조절 만들기
}
