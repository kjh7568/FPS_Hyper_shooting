using UnityEngine;
using UnityEngine.UI;

public class AugmentButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    private AugmentData augmentData;

    public void Initialize(AugmentData data)
    {
        augmentData = data;

        if (iconImage != null)
        {
            Sprite icon = augmentData.LoadIcon();
            iconImage.sprite = icon != null ? icon : GetDefaultIcon();
        }
    }

    public void OnClick()
    {
        Debug.Log($"선택한 증강: {augmentData.Type}");

        GameData.Instance.augmentStat.Apply(augmentData);

        PlayerController controller = FindObjectOfType<PlayerController>();
        if (controller != null) controller.isOpenPanel = false;

        WeaponManager.instance.currentWeapon.isOpenPanel = false;
        AugmentPanelManager.Instance.ClosePanel();
    }

    private Sprite GetDefaultIcon()
    {
        return Resources.Load<Sprite>("Icons/DefaultIcon");
    }
}