using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerDeadPanel : MonoBehaviour
{
    [FormerlySerializedAs("bossPanel")] [SerializeField]
    private GameObject diePanel;

    public void OpenDiePanel()
    {
        var pc = Player.localPlayer.GetComponent<PlayerController>();
        
        pc.isOpenPanel = false;
        pc.SetDieAnimation();        

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        diePanel.SetActive(true);
        diePanel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    #region 버튼 클릭 옵션

    public void OnClickBackButton()
    {
        var pStat = Player.localPlayer.playerStat;
        var eStat = Player.localPlayer.inventory.EquipmentStat;
        var cStat = Player.localPlayer.coreStat;
        var aStat = GameData.Instance.augmentStat;

        var totalHealth = (pStat.maxHealth + eStat.plusHp + cStat.plusHp + aStat.plusHp) * eStat.increaseHealth;
        Player.localPlayer.playerStat.health = totalHealth;
        
        var pc = Player.localPlayer.GetComponent<PlayerController>();
        
        pc.isOpenPanel = false;
        pc.SetDieAnimation();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(1);
    }

    #endregion
}