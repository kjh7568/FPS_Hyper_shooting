// AugmentButton.cs
using UnityEngine;

public class AugmentButton : MonoBehaviour
{
    public void OnAugmentSelected()
    {
        Debug.Log("증강이 선택되었습니다.");

        FindObjectOfType<PlayerController>().isOpenPanel = false;
        Player.localPlayer.myGun.isOpenPanel = false;
        
        AugmentPanelManager.Instance.ClosePanel();
    }
}