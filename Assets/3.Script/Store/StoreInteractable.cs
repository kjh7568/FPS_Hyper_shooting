using UnityEngine;

public class StoreInteractable : MonoBehaviour
{
    [SerializeField] private GameObject pressETextUI; // "Press E" 텍스트 오브젝트
    private bool isPlayerNear = false;

    private void Start()
    {
        if (pressETextUI != null)
            pressETextUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (pressETextUI != null)
            {
                pressETextUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            pressETextUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            StorePanelManager.Instance.OpenPanel();
            pressETextUI.SetActive(false); // 패널 열면 텍스트 숨김

            // 커서 락 해제 및 보이게 설정
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            FindObjectOfType<PlayerController>().isOpenPanel = true;
            WeaponManager.instance.currentWeapon.isOpenPanel = true;
        }
    }

}