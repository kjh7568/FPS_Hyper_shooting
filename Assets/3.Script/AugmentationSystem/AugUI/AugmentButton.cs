// AugmentButton.cs
using UnityEngine;

public class AugmentButton : MonoBehaviour
{
    private AugmentData augmentData;
    
    public void Initialize(AugmentData data)
    {
        augmentData = data;
    }
    
    public void OnClick()
    {
        Debug.Log($"선택한 증강: {augmentData.Type}");

        GameData.Instance.augmentStat.Apply(augmentData);
            
        PlayerController controller = FindObjectOfType<PlayerController>();
        if (controller != null) controller.isOpenPanel = false;

        WeaponManager.currentWeapon.isOpenPanel = false;
        AugmentPanelManager.Instance.ClosePanel();
    }
}

