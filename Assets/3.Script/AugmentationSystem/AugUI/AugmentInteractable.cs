using UnityEngine;

public class AugmentInteractable : MonoBehaviour
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
                Debug.Log("플레이어가 증강 오브젝트에 닿았습니다.");
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
            if (AugmentPanelManager.Instance.HasSelected)
            {
                Debug.Log("이미 증강을 선택했기 때문에 다시 열 수 없습니다.");
                return;
            }
            AugmentPanelManager.Instance.OpenPanel();
            pressETextUI.SetActive(false); // 패널 열면 텍스트 숨김

            // 커서 락 해제 및 보이게 설정
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

           //  FindObjectOfType<PlayerController>().isOpenPanel = true;
           //  WeaponManager.instance.currentWeapon.isOpenPanel = true;
        }
    }

}