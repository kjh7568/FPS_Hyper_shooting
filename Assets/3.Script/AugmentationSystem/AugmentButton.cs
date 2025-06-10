// AugmentButton.cs
using UnityEngine;

public class AugmentButton : MonoBehaviour
{
    public void OnAugmentSelected()
    {
        Debug.Log("증강이 선택되었습니다.");
        AugmentPanelManager.Instance.ClosePanel();
    }
}