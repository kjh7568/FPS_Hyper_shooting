using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossUIManager : MonoBehaviour
{
    public int gainedCore;
    
    [SerializeField] private GameObject bossPanel;
    [SerializeField] private TMP_Text coreCountText;
    
    public void OpenBossEndPanel()
    {
        Player.localPlayer.GetComponent<PlayerController>().isOpenPanel = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        //todo 보스 이미지 셋팅도 해야함
        coreCountText.text = gainedCore.ToString();

        bossPanel.SetActive(true);
        bossPanel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }
    
    #region 버튼 클릭 옵션

    public void OnClickBackButton()
    {
        //todo 플레이어 데이터 씽크 맞춰야 함
        Player.localPlayer.GetComponent<PlayerController>().isOpenPanel = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;        

        SceneManager.LoadScene(1);
    }

    #endregion    
}
